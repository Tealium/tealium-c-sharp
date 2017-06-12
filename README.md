## Tealium Mobile Library for C#
This guide shows how to add, configure, and track events for a C# application utilizing the Tealium C# Integration library.

## Requirements
Before proceeding, ensure you have satisfied the following requirements:

* Universal Data Hub Account
* Xamarin Community Edition+

## Get the Code
The code for the Tealium C# library is stored in Github. You must retrieve the code to get started.

Download [Tealium for C# from GitHub](https://github.com/Tealium/tealium-c-sharp)

## Code Setup
Import the *Tealium* folder into your Solution.
```csharp
using TealiumCSharp;
```

## Initialization
Once the Tealium for C# is installed you are ready to start coding. Use the following code to initialize with the following parameters:

- account - The name of your account.
- profile - The name of your profile within your account.
- environment - "dev", "qa", or "prod"
- visitorId - A 32 character alphanumeric string identifer that should be unique to a user, app instance.
- ModuleNames - (Optional) Modules to be initialized with library. Default Module if none provided: AppDataModule, CollectModule, LoggerModule.
- datasourceId - (Optional) Datasource ID provided by Universal Data Hub (UDH) setup.
- overrideCollectUrl - (Optional) Custom collect URL.
- optionalData - (Optional) Dictionary for module use. Null acceptable. 

```csharp
// SAMPLE
Config config = new Config("account", 
                           "profile", 
                           "environment",
                           "visitorId",
                           new string[] { <ModuleNames> },
                           "datasouceId", 
                           "overrideCollectUrl",
                           new Dictionary<string, object> optionalData);

Tealium tealium = new Tealium(config);
```

## Tracking
To deliver data to Tealium Collect for further processing and/or forwarding to target vendors, use any of the Tealium track calls:

- eventTitle - Title of tracked event.
- customData - Optional custom data dictionary to send with track call.
- completion - Optional completion block to trigger after track call.

```csharp
// SAMPLE
// Full argument track call - illustrating call for a UI event with a callback. 
tealium.Track("eventTitle", 
              customData, 
              (success, info, error)=>) { 
                  Debug.WriteLine("buttonTapTrackComplete: was successful: " + success); 
              });
```
Below are two convenience track calls:

```csharp
// Convenience track call with only a string identifier for an activity type call.
tealium.Track("eventTitle");

//Convenience track call with event title and optional data dictionary
Dictionary<sting, object> customData = new Dictionary<string, object>()
{
   {"foo1", new string[]{"a","12"}},
   {"foo2", "bar2"},
   {"foo3", "bar3"}
}; 
tealium.Track("eventTitle", customData);
```

## Validation
Use [Live Events](https://community.tealiumiq.com/t5/Universal-Data-Hub/Live-Events-Streams/ta-p/11805) or [Trace](https://community.tealiumiq.com/t5/Universal-Data-Hub/Visitor-Trace/ta-p/12058#Introduction) to validate that Tealium is receiving your tracking calls.

# Function Definitions

## Config()
```csharp
public Config (string account,
               string profile,
               string dev,
               string visitorId,
               string[] modules = null,
               string datasource = null,
               string overrideCollectUrl = null,
               Dictionary<string, object> optionalData = null)
```

**Parameters**  | **Description** | **Example Value**
------------- | ------------- | ------------------
account | Tealium Account | tealium 
profile | Tealium profile | mobile_division
environment | Tealium environment. Currently optional | prod
visitorId | 32-char alphanumeric string identifier that should be unique to a user, app instance, or device | 612040730c8c11e6b4cbacbc329f41b7
modules | Optional modules to be initialed with library | See [Available Modules](#available-modules)
datasource | Optional datasource ID provided by Universal Data Hub (UDH) setup | abc123
overrideCollectUrl | Optional custom collect URL | - 
optionalData | Optional dictionary<string, object> for module use. Null acceptable. Value not typically needed for most setups | -

```csharp
//Sample
Config config = new Config("account", 
                           "profile", 
                           "environment",
                           "visitorId",
                           new string[] { AppDataModule.Name,
                                           CollectModule.Name,
                                           LoggerModule.Name },
                           "datasouceId", 
                           "http://overrideCollectUrl.com/",
                           new Dictionary<string, object> optionalData);
```

NOTE: The config object has a *modules* string[] property that must be populated with the string representations of each module desired to use with library. See the [Available Modules](#available-modules) section.

## Tealium()
```csharp
Tealium(Config config)
```
Primary object constructor

**Parameters**  | **Description** | **Example Value**
------------- | ------------- | ------------------
config | Required Config instance | - 

```csharp
//Sample
Config config = new Config("account", 
                           "profile", 
                           "environment",
                           "visitorId",
                           new string[] { AppDataModule.Name,
                                           CollectModule.Name,
                                           LoggerModule.Name },
                           "datasouceId", 
                           "http://overrideCollectUrl.com/",
                           new Dictionary<string, object> optionalData);
Tealium tealium = new Tealium(config);
```

## Enable()
Automatically called with the initial initialization. Re-enable the Tealium instance and any internal modules if the library had been disabled prior. 

```csharp
//Sample
tealium.Enable()
```

## Disable()
Disable library modules from temporarily processing events. May deinit internal class objects.

```csharp
//Sample
tealium.Disable()
```

## Track()
```csharp
Track(string title,
      Dictionary<string, object> customData,
      TrackCompletion completion)
```

**Parameters**  | **Description** | **Example Value**
------------- | ------------- | ------------------
title | Required string identifier for the track event | "aButtonWasTapped"
customData | Optional Dictionary<string, object> of key-value data to include with track call | -
completion | Optional completion block to be triggered once call has been completed | -

```csharp
//Sample
Dictionary<string, object> data = new Dictionary<string, object>()
{
    {"foo1", new string[]{"a","12"}},
    {"foo2", "bar2"},
    {"foo3", "bar3"}
};
tealium.Track("eventTitle",
              customData,
              (success, info, error) =>
              {
                   Debug.WriteLine("Track action completion block triggered.");
                   Debug.WriteLine("Success: " + success + " info: " + info + " error: " + error);
              });
```

### TrackCompletion
The optional callback for track calls will contain the following parameters:

**Parameters**  | **Description** | **Example Value**
------------- | ------------- | ------------------
successful | Bool status if the track call could be delivered | true
info | Dictionary<string, object> containing track and response data | -
error | Optional error from any dispatch services encountering a delivery or response issue | -

### Available Modules
The following string values can be used to enable modules with the current build.

**Module Name** | **Description**
------------- | -------------
AppDataModule | Adds automatic data points to calls
CollectModule | Delivers processed events to Tealium Collect endpoint
LoggerModule | Provides debug output

```csharp
//Sample
config.Modules = new string[] { AppDataModule.Name,
                                CollectModule.Name,
                                LoggerModule.Name };
```
NOTE: Use the .Name constant of each Module class. 


## Additional Resources
* This readme is mirrored in a [TLC Getting Started Guide](https://community.tealiumiq.com/t5/Mobile-Libraries/Tealium-for-C/ta-p/17670/).  

## Contact Us
* If you have **code questions** or have experienced **errors** please post an issue in the [issues page](../../issues)
* If you have **general questions** or want to network with other users please visit the [Tealium Learning Community](https://community.tealiumiq.com)
* If you have **account specific questions** please contact your Tealium account manager

# Change Log
- 1.0.0 Initial Release


## License
Use of this software is subject to the terms and conditions of the license agreement contained in the file titled "LICENSE.txt".  Please read the license before downloading or using any of the files contained in this repository. By downloading or using any of these files, you are agreeing to be bound by and comply with the license agreement.


---
Copyright (C) 2012-2017, Tealium Inc.
