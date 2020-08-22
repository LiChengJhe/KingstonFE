using System;
using System.Collections.Generic;
using System.Text;

namespace EAP_Library.Models
{
    public class StatusInfo: BaseInfo
    {
        public StatusType CurStatusType { get; set; }
        public DateTime CurStatusTimestamp { get; set; }
        public StatusType? PrevStatusType { get; set; }
        public DateTime? PrevStatusTimestamp { get; set; }
    }

    public enum StatusType
    {
        RUN,
        IDLE,
        ENG,
        PM,
        CHECK,
        UNKNOWN,
        DOWN,
        HOLD,
        OFF
    }
}
