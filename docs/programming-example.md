---
layout: page
title: Programming example
---

## Introduction
For this example we are going to develop a TwinCAT library with some well defined functionality using test driven development with the TcUnit framework.
The scope of the library will be developing functions to handle certain aspects of the IO-Link communication.
IO-Link is a digital point-to-point (master and slave) serial communication protocol.
It’s used to extend or replace the standard analog (0..10V, 4..20mA, +/- 10V etc) sensors with a digital interface.
With IO-Link you get a significantly improved and extended integration with the lowest level sensors in your system.
We won’t go too much into details specifically about IO-Link, instead it’s highly recommended to read more about it on the [IO-Link website](https://io-link.com/en/index.php).

For this topic however, what is important to know is that IO-Link provides certain services.
One of the functionalities of IO-Link devices is that they can fire off events to the IO-Link master to notify that something has happened, for instance an alarm that something is wrong.
By using an IO-Link master that is using EtherCAT for fieldbus communications to the higher level system (PLC) we can receive the events by doing CoE reads.
When using an EtherCAT-capable IO-Link master such as:

- [Balluff BNI0077](https://www.balluff.com/en-us/products/BNI0077#?data=)
- [Beckhoff EL6224](https://www.beckhoff.com/en-en/products/i-o/ethercat-terminals/el6xxx-communication/el6224.html#:~:text=The%20EL6224%20IO%2DLink%20terminal,the%20terminal%20and%20the%20device.) or [EP6224](https://www.beckhoff.com/en-en/products/i-o/ethercat-box/epxxxx-industrial-housing/ep6xxx-communication/ep6224-0042.html)/[EP6228](https://www.beckhoff.com/en-en/products/i-o/ethercat-box/epxxxx-industrial-housing/ep6xxx-communication/ep6228-0022.html)
- [IFM AL1332](https://www.ifm.com/de/en/product/AL1332)
- [Omron GX-ILM08C](https://www.ia.omron.com/products/family/3541/)

which all support the CoE diagnosis history object (0x10F3), all IO-Link events are stored in the memory of the device.
It’s important to note that the diagnosis history object (0x10F3) can be implemented by any EtherCAT slave to store diagnostic type of data, not only IO-Link events.
Note that the implementation of the diagnosis history object is optional by the manufacturer.
Whether the diagnosis history object is implemented or not is reported by a flag in the ESI-file of the EtherCAT slave.
According to EtherCAT Technology Group document "[ETG1020 – EtherCAT Protocol Enhancements](https://www.ethercat.org/en/downloads/downloads_00600A7B120E41D385DC0AF19C034434.htm)", each message logged in the diagnostic history object has the following data:

- Diagnosis code (4 bytes) – mandatory
- Flags (2 bytes) – mandatory
- Text ID (2 bytes) – mandatory
- Timestamp (8 bytes) – mandatory
- Optional parameters – optional

This is only a description of the data on a high level, for all the details on what’s exactly included on a bit-level all information can be found in ETG1020.
The number of optional parameters can be varying (zero parameters as well) depending on the diagnosis message itself.

## Data to be parsed
What we will do here is to create test cases to parse each and one of the mandatory fields.
Each field will be parsed by its own function block that will provide the data above in a structured manner.
Looking at the diagnosis history object, the diagnosis messages themselves are an array of bytes that are read by SDO read. For this particular example, we assume we have the stream of bytes already prepared by the SDO read.
We will not focus on creating test cases for the SDO read, but rather focus on the parsing.
In one of the tested IO-Link masters, the diagnosis history message object is up to 28 bytes, which means that the IO-Link master on top of the above data also supports a certain number of parameters.
As said above however, we’ll focus on the parsing of the first 4+2+2+8 = 16 bytes, as the first four parameters in the diagnosis message are mandatory whilst the parameters are optional.
What we need to do now is to create a data structure for each of the data fields above.

Before we start to dwell too deep into the code, it’s good to know that all the source code for the complete example is available [on GitHub](https://github.com/tcunit/ExampleProjects/tree/master/AdvancedExampleProject), as it might be preferred to look at the code in the Visual Studio IDE rather than on a webpage.

### Diagnosis code
The diagnosis code looks like this: