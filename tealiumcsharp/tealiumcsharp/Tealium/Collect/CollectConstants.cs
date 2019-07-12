using System;
namespace TealiumCSharp
{
    public class CollectConstants
    {
        public CollectConstants() { }

        public const string ENCODED_URL = "encoded_url";
        public const string OVERRIDE_COLLECT_URL = "tealium_override_collect_url";
        public const string PAYLOAD = "payload";
        public const string RESPONSE_HEADERS = "response_headers";
        public const string DISPATCH_SERVICE = "dispatch_service";
    }

    public enum Method
    {
        POST,
        GET
    }
}

