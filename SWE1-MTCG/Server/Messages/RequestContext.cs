﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SWE1_MTCG
{
    public class RequestContext
    {
        public string message;

        public Dictionary<string, string> KeyValues = new Dictionary<string, string>();
    }
    /*
        POST /messages HTTP/1.1
        Content-Type: text/plain; charset=utf-8
        Content-Lenght: 14
        Host: 127.0.0.1:6543
        UserName: riob
        Password: asdfa

        StartTheBattle
    */

}