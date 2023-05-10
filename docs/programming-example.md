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

| Bit 0-15 | Bit 16-31 |
|----------|-----------|
|0x0000-0xDFFF|not used|
|0xE000-0xE7FF|can be used manufacturer specific|
|0xE800|Emergency Error Code as defined in DS301 or DS4xxx|
|0xE801-0xEDFF|reserved for future standardization|
|0xEE00-0xEFFF|Profile specific|
|0xF000-0xFFFF|not used|

We'll create a struct for it:

```
TYPE ST_DIAGNOSTICCODE :
STRUCT
    eDiagnosticCodeType : E_DIAGNOSTICCODETYPE;
    nCode : UINT;
END_STRUCT
END_TYPE
```

where the `E_DIAGNOSTICCODETYPE` is

```
TYPE E_DIAGNOSTICCODETYPE :
(
    ManufacturerSpecific := 0,
    EmergencyErrorCodeDS301 := 1,
    ProfileSpecific := 2,
    Unspecified := 3
) USINT;
END_TYPE
```

What we're basically doing here is to first look at the first 16 bits, and categorizing them into any of the four possibilities of the enumeration `E_DIAGNOSTICCODETYPE`.
All unknowns (reserved, not used) are set as `E_DIAGNOSTICCODETYPE.Unspecified`.
Then we convert bit 16-31 into nCode. These two together will create the struct `ST_DIAGNOSTICCODE`.

### Flags

The flags have three parameters: "Diagnosis type", "Time stamp type", "Number of parameters in the diagnosis message".
|Bit|Description|
|-|-|
|Bit 0-3|0: Info message<br/> 1: Warning message<br/> 2: Error message<br/> 3-15: Reserved for future use|
|Bit 4|Time stamp is a local time stamp|
|Bit 5-7|Reserved for future use|
|Bit 8-15|Number of parameters in this diagnosis message|

We'll create a struct for it:

```
TYPE ST_FLAGS :
STRUCT
    eDiagnostisType : E_DIAGNOSISTYPE;
    eTimeStampType : E_TIMESTAMPTYPE;
    nNumberOfParametersInDiagnosisMessage : USINT;
END_STRUCT
END_TYPE
```

Where `E_DiagnosisType` and `E_TimeStampType` are respectively:

```
TYPE E_DIAGNOSISTYPE :
(
    InfoMessage := 0,
    WarningMessage := 1,
    ErrorMessage := 2,
    Unspecified := 3
) BYTE;
END_TYPE
```

Where the `Unspecified` value is there in case we would receive one of the values that are reserved for future standardization.

```
TYPE E_TIMESTAMPTYPE :
(
    Local := 0,
    Global := 1
) USINT;
END_TYPE
```

The timestamp is obtained from the local clock of the terminal at the time of the event.
The difference between the global and local timestamp is that the global is based on the DC-clock of the reference clock (and is thus global on the whole DC network), whilst a local clock is only used internally in the EtherCAT slave.
It's interesting to store this information as you probably want to handle the reading of the timestamp differently depending on if it's a local or a global timestamp.

### Text identity
The text identity is just a simple unsigned integer (0-65535) value which is a reference to the diagnosis text file located in the ESI-xml file for the IO-Link master.
This information is valuable if you want to do further analysis on the event, as it will give you more details on the event in a textual format.

### Time stamp
The 64 bit timestamp is either the EtherCAT DC-clock timestamp or the local time stamp of the EtherCAT slave/IO-Link master, depending on whether DC is enabled and/or the IO-Link master supports DC.
The 64-bit value holds data with 1ns resolution.

### The complete diagnostic message
Now that we have all four diagnosis message parameters, we'll finish off by creating a structure for them that our parser will deliver as output once a new diagnosis event has occurred.
Based on the information provided above it will have the following layout:

```
TYPE ST_DIAGNOSTICMESSAGE :
STRUCT
    stDiagnosticCode : ST_DIAGNOSTICCODE;
    stFlags : ST_FLAGS;
    nTextIdentityReferenceToESIFile : UINT;
    sTimeStamp : STRING(29);
END_STRUCT
END_TYPE
```

Here you can obviously have the timestamp as a `DC_Time64`-type instead of the `STRING`-type, as it's normally more interesting to store time/data as strings the closer you come to the operator.
But for the sake of showing the concept of writing unit test cases, we'll stick to strings.
The reason we went for a 29 byte is because this is the size of the string that is returned when doing a `DCTIME64_TO_STRING()` function call.