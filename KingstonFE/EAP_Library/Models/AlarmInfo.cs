using System;
using System.Collections.Generic;
using System.Text;

namespace EAP_Library.Models
{
    public class AlarmInfo : BaseInfo
    {
        public string AlarmId { get; set; }
        public AlarmType AlarmType { get; set; }
        public string AlarmText { get; set; }
        public DateTime AlarmTimeFrom { get; set; }
        public DateTime? AlarmTimeTo { get; set; }
    }


    public enum AlarmType
    {
        ON_GOING,
        CLEAR
    }
}
