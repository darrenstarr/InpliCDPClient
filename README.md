# Inpli CDP Client for Linux

This is a proof of concept CDP client for Linux with receive only capabilities.
This should allow IoT devices running on Linux to be able to phone home with location information regarding where they are connected on the network.

This code finds all Ethernet interfaces on the IoT device which it's running, registers the Cisco CDP Multicast MAC address on the interface,
captures and parses the frames and then forwards them via a REST API to a server.


