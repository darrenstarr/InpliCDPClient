namespace InpliCDPClient
{
    using libciscocdp;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.NetworkInformation;
    using System.Threading.Tasks;

    class Program
    {
        static readonly PhysicalAddress CdpAddress = PhysicalAddress.Parse("01-00-0C-0C-0C-0C");

        static void Main(string[] args)
        {
            // Find all Ethernet adapters on the machine.
            var ethernetInterfaces =
                from
                    nic in NetworkInterface.GetAllNetworkInterfaces()
                where
                    nic.OperationalStatus == OperationalStatus.Up &&
                    nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    nic.NetworkInterfaceType == NetworkInterfaceType.FastEthernetFx ||
                    nic.NetworkInterfaceType == NetworkInterfaceType.FastEthernetT ||
                    nic.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet
                select
                    nic
                    ;

            // 
            var tasks = new List<Task>();
            foreach(var nic in ethernetInterfaces)
            {
                Console.WriteLine("Starting CDP for interface " + nic.Name + " of type " + nic.NetworkInterfaceType.ToString());

                Task.Factory.StartNew(
                        async () =>
                        {
                            var llcSocket = new LLCSocket(nic.GetPhysicalAddress());
                            llcSocket.RegisterMacOnInterface(nic.Name, CdpAddress);
                            while(true)
                            {
                                var packet = llcSocket.ReceiveFrom(out PhysicalAddress remoteHost);
                                try
                                {
                                    var parsed = CdpParser.Parse(packet);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Exception while parsing: " + e.Message);
                                }

                                if(packet != null)
                                {
                                    JsonSerializerSettings serializerSettings =
                                        new JsonSerializerSettings()
                                        {
                                            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                                            DateTimeZoneHandling = DateTimeZoneHandling.Utc
                                        };

                                    serializerSettings.Converters.Add(new IPAddressConverter());
                                    serializerSettings.Converters.Add(new IPEndPointConverter());
                                    var dataAsString = JsonConvert.SerializeObject(packet, serializerSettings);
                                    var content = new StringContent(dataAsString);
                                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                                    var client = new HttpClient();
                                    await client.PostAsync("http://192.168.70.1:51954", content);
                                }
                            }
                        },
                        TaskCreationOptions.LongRunning
                    );
            }
        }
    }
}
