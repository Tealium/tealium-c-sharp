## Tealium Mobile Library for C#
This guide shows how to add, configure, and track events for a C# application utilizing the Tealium C# Integration library.

## Built-in Data Layer
This library populates the following event attributes for mapping:

**Event Attribute**  | **Description** | **Sample** | **Type**
------------- | ------------------------- | ------------ | -------------
tealium_account | Tealium account name used to initalize library | "Tealium" | string
tealium_profile | Tealium profile name used to initalize library | "demo" | string
tealium_environment | Tealium environment used to initialize library (usually 'dev', 'qa', or 'prod') | "prod" | string
tealium_event | Title of event. Populated by eventTitle argument of the Tealium track calls | "some_title" | string
tealium_datasource | 6-char alphanumeric identifer provided by Universal Data Hub (UDH) | "abc123" | string
tealium_visitor_id | Unique device visitor id that differentiates users. Required by Collect Service | "612040730c8c11e6b4cbacbc329f41b7" | string
tealium_library_name | Name of the platform integration library | "csharp" | string
tealium_library_version | Version of the platform integration library | "1.0.0" | string
tealium_random | Random 16 digit number generated per event | "1234567890123456" | number
tealium_session_id | Unix epoch timestamp in milliseconds, no decimals, of first event. Resets after a new session starts, typically a new launch | "1496350751928" | number
tealium_timestamp_epoch | Unix timestamp at UTC | "1496350751" | number

NOTE: All values are converted to strings for delivery.  Variables/event attributes listed as 'number' types are originally number values and could be coerced back into numbers from these stringified values.

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
- method - (Optional) HTTP Method for the Collect Endpoint

```csharp
// SAMPLE
Config config = new Config("account", 
                           "profile", 
                           "environment",
                           "visitorId",
                           new string[] { <ModuleNames> },
                           "datasouceId", 
                           "overrideCollectUrl",
                           new Dictionary<string, object> optionalData,
                           Method.POST);

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
               Dictionary<string, object> optionalData = null,
               Method method = Method.POST)
```

**Parameters**  | **Description** | **Example Value**
------------- | ------------- | ------------------
account | Tealium Account | tealium 
profile | Tealium profile | mobile_division
environment | Tealium environment. Currently optional.| prod
visitorId | 32-char alphanumeric string identifier that should be unique to a user, app instance, or device | 612040730c8c11e6b4cbacbc329f41b7
modules | Optional modules to be initialed with library | See [Available Modules](#available-modules)
datasource | Optional datasource ID provided by Universal Data Hub (UDH) setup | abc123
overrideCollectUrl | Optional custom collect URL | - 
optionalData | Optional dictionary<string, object> for module use. Null acceptable. Value not typically needed for most setups | -
method | Optional HTTP method, POST or GET, to determine how to send the data to the Collect Endpoint | Method.POST, Method.GET
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
                           new Dictionary<string, object> optionalData,
                           Method.POST);
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


## JoinTrace()
Joins a Trace using the supplied Id. The `tealium_trace_id` will be present in all future Track calls until the LeaveTrace() method is called.

```csharp
//Sample
tealium.JoinTrace("01234");
```

**Parameters**  | **Description** | **Example Value**
------------- | ------------- | ------------------
traceId | Required string identifying the trace to join | "12345"

## LeaveTrace()
Leaves a trace if one has been joined. The `tealium_trace_id` will no longer be populated automatically on future events.

```csharp
//Sample
tealium.LeaveTrace();
```

## KillTraceSession()
Sends an additional event to Tealium to end the visitor session at the server. This is useful for testing end-of-visit configuration.
Also calls LeaveTrace() to stop any more events being sent with the previous trace id.

```csharp
//Sample
tealium.KillTraceSession();
```

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
- 1.1.0 Event Endpoint
  - Support for sending data to the /event endpoint via both POST and GET methods
  - Extra methods for joining/leaving traces added
  - Fix: OptionalData supplied wasn't being sent
- 1.0.0 Initial Release


## License
Use of this software is subject to the terms and conditions of the license agreement contained in the file titled "LICENSE.txt".  Please read the license before downloading or using any of the files contained in this repository. By downloading or using any of these files, you are agreeing to be bound by and comply with the license agreement.


---
Copyright (C) 2012-2019, Tealium Inc.
