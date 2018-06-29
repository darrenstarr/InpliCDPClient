using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace libciscocdp.tests
{
    public class JsonCodec
    {
        public static readonly string Sample1 = @"
{
    ""receivedOn"": ""eth1"",
    ""receivedAt"": ""2018-06-29T13:37:18.3550893+02:00"",
    ""sourceMac"": ""00155D1C8E0C"",
    ""packet"": {
        ""version"": 2,
        ""ttl"": 180,
        ""deviceId"": ""csr.log.local"",
        ""softwareVersion"": ""Cisco IOS Software [Fuji], Virtual XE Software (X86_64_LINUX_IOSD-UNIVERSALK9-M), Version 16.7.2, RELEASE SOFTWARE (fc3)\nTechnical Support: http://www.cisco.com/techsupport\nCopyright (c) 1986-2018 by Cisco Systems, Inc.\nCompiled Tue 29-May-18 19:23 by mcpre"",
        ""platform"": ""cisco CSR1000V"",
        ""addresses"": [
            ""192.168.70.3"",
            ""2001:db8:70::3"",
            ""fe80::215:5dff:fe1c:8e0c""
        ],
        ""portId"": ""GigabitEthernet2"",
        ""hasCapabilities"": true,
        ""canRoute"": true,
        ""canTransparentBridge"": false,
        ""canSourceRouteBridge"": false,
        ""canSwitch"": false,
        ""isHost"": false,
        ""isIGMPCapable"": true,
        ""isRepeater"": false,
        ""duplex"": 1,
        ""managementAddresses"": [
            ""192.168.70.3""
        ],
        ""odrPrefixes"": [
            {
                ""network"": ""10.90.90.0"",
                ""length"": 24
            },
            {
                ""network"": ""172.21.61.96"",
                ""length"": 28
            }
        ]
    }
}
";

        /// <summary>
        /// Message model transmitted via REST to the server
        /// </summary>
        public class CdpApiMessage
        {
            /// <summary>
            /// The interface name which the packet was received on
            /// </summary>
            public string ReceivedOn { get; set; }

            /// <summary>
            /// The date and time which the packet was received
            /// </summary>
            public DateTimeOffset ReceivedAt { get; set; }

            /// <summary>
            /// The transmitting device's source MAC address
            /// </summary>
            public string SourceMac { get; set; }

            /// <summary>
            /// The contents of the parsed packet
            /// </summary>
            public CdpPacket Packet { get; set; }
        }

        [Fact]
        public void DecodePacket()
        {
            ITraceWriter traceWriter = new MemoryTraceWriter { LevelFilter = System.Diagnostics.TraceLevel.Verbose };

            JsonSerializerSettings serializerSettings =
                           new JsonSerializerSettings()
                           {
                               MissingMemberHandling = MissingMemberHandling.Ignore,
                               DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                               TraceWriter = traceWriter
                           };

            serializerSettings.Converters.Add(new IPAddressConverter());
            serializerSettings.Converters.Add(new IPEndPointConverter());

            var message = JsonConvert.DeserializeObject<CdpApiMessage>(Sample1, serializerSettings);

            //throw new Exception(traceWriter.ToString());

            Assert.NotNull(message);
            Assert.NotNull(message.Packet);
            Assert.NotNull(message.Packet.DeviceId);
            Assert.NotEmpty(message.Packet.DeviceId);
        }
    }
}
