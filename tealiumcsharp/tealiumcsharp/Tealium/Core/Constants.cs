using System;
using System.Collections.Generic;

namespace TealiumCSharp
{
    public class Constants
    {
        public Constants() { }

        public const string ACCOUNT = "tealium_account";
        public const string PROFILE = "tealium_profile";
        public const string ENVIRONMENT = "tealium_environment";
        public const string EVENT = "tealium_event";
        public const string DATASOURCE = "tealium_datasource";
        public const string VISITOR_ID = "tealium_visitor_id";

        public const string LIBRARY_NAME = "tealium_library_name";
        public const string LIBRARY_VERSION = "tealium_library_version";

        public const string LIBRARY_NAME_VALUE = "csharp";
        public const string LIBRARY_VERSION_VALUE = "1.0";

        public static readonly string[] DEFAULT_MODULES = {"TealiumCSharp.AppDataModule",
                                                    "TealiumCSharp.CollectModule",
                                                    "TealiumCSharp.LoggerModule" };
    }

    public class Track
    {
        public Dictionary<string, object> data { get; set; }
        public Dictionary<string, object> info;
        public TrackCompletion completion;

        public Track(Dictionary<string, object> data,
                            Dictionary<string, object> info = null,
                            TrackCompletion completion = null)
        {
            this.data = data;
            this.info = info;
            this.completion = completion;

        }
    }

    public enum ProcessType
    {
        Enable,
        Disable,
        Track
    }


    public delegate void TrackCompletion(bool successful,
                                         Dictionary<string, object> info,
                                         Exception error);

    public struct Process
    {
        public ProcessType type;
        public bool successful;
        public Track track;
        public Exception error;

        public Process(ProcessType type,
                      bool successful,
                      Track track = null,
                      Exception error = null)
        {
            this.type = type;
            this.successful = successful;
            this.track = track;
            this.error = error;

        }
    }




}
