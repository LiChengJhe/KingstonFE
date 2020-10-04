using System;
using System.Collections.Generic;
using System.Text;

namespace EAP_Library.DTO
{
    public class EqpStatusInfoDTO: BaseInfoDTO
    {
        public string CurType { get; set; }
        public DateTime CurTime { get; set; }
        public string PrevType { get; set; }
        public DateTime? PrevTime { get; set; }
    }

    public enum EqpStatusType
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
