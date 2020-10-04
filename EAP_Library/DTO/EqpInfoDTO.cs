using System;
using System.Collections.Generic;
using System.Text;

namespace EAP_Library.DTO
{
    public class EqpInfoDTO : BaseInfoDTO
    {
        public string Id { set; get; }
        public string Type { set; get; }
        public EqpStatusInfoDTO StatusInfo { set; get; }
        public CassetteInfoDTO CassetteInfo { set; get; }
        public EqpAlarmInfoDTO AlarmInfo { set; get; }
    }

    public enum EqpType
    {
        Cutting,
        Grinding,
        Tape
    }
}
