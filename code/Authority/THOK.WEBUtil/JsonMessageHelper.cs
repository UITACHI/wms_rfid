using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.WebUtil
{
    public static class JsonMessageHelper
    {
        public static object getJsonMessage(bool success, string msg, object data)
        {
            return new { success = success, msg = msg, data = data };
        }

        public static object getJsonMessage(bool success, string msg)
        {
            return getJsonMessage(success, msg, null);
        }
    }
}
