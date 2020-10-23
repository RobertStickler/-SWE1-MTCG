using System;
using System.Collections.Generic;
using System.Text;

namespace SWE1_MTCG
{
    public class RequestContext
    {

        public string method;
        public string header;
        public string version;
        public string path;

    }
    public class ResponseContext
    {

        public string status_message;
        public string header;
        public string version;
        public string status_code;

    }
}
