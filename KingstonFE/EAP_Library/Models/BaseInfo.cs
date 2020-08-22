using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace EAP_Library.Models
{
   public class BaseInfo
    {
        public string EventName { set; get; }
        public ExpandoObject ExpandData { set; get; }
        public DateTime Timestamp { set; get; }
        public string Version { set; get; }
    }
}
