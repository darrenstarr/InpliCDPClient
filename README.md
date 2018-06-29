# Inpli CDP Client for Linux

This is a proof of concept CDP client for Linux with receive only capabilities.
This should allow IoT devices running on Linux to be able to phone home with location information regarding where they are connected on the network.

This code finds all Ethernet interfaces on the IoT device which it's running, registers the Cisco CDP Multicast MAC address on the interface,
captures and parses the frames and then forwards them via a REST API to a server.

## Status

At the time of this writing, the parser is pretty solid. I'm in the process of talking with people who better understand the P/Invoke mechanisms of
C# than I do so that I hopefully can remove all code marked as unsafe from my system.

An architectual decision I made was to use C# exclusively and make system calls from C# as opposed to producing a seperate native code library to
bridge the gap. While I believe I've abstracted everything correctly, in order to make mappings of the C data structures which contain fixed sized
buffers, I've used the "fixed" keyword which is needed for producing libc compatible buffers.

If I can't find an alternative method of producing the structures than to use packed structures with the unsafe keyword, I'll write serializers instead.

### TLVs

I haven't tested all the CDP TLVs extensively. But I have parsed as best I can from what I've managed to understand from viewing packets in Wireshark. All
the important TLVs I've encountered are handled well.

## Requirements

The code can be checked out, compiled and run using .NET Core 2.0 and later.

### Required kernel modules

Make sure that LLC is available by modprobing llc2

### Running as root

I don't know yet how to make this code access AF_LLC without root privleges. If anyone knows how to do this, please shoot me a message once you verify it.
