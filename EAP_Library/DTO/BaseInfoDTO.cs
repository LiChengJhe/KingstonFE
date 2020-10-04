using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace EAP_Library.DTO
{
    public class BaseInfoDTO
    {
        public string EventName { set; get; }
        public DateTime EventTime { set; get; }
        public ExpandoObject ExpandData { set; get; }
        public string Version { set; get; }
    }


    public enum BaseEventType
    {
        CassetteIn,
        CassetteOut,
        Unknow
    }
}
