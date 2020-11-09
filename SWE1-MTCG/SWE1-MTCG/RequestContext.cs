using System;
using System.Collections.Generic;
using System.Text;

namespace SWE1_MTCG
{
    public class RequestContext
    {
        public string unique_id = "0";
        public string method;
        public string header;
        public string version;
        public string path;
        public string message;

    }
    public class ResponseContext
    {
        public string version;
        public string status_code;
        public string status_message;
        public string header;
        public string message;
    }
}
