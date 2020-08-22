using System;
using System.Collections.Generic;
using System.Text;

namespace EAP_Library.Models
{
    public class EqpInfo : BaseInfo
    {
        public string EqpId { set; get; }
        public EqpType EqpType { set; get; }
        public StatusInfo EqpStatus { set; get; }
        public CassetteInfo CassetteInfo { set; get; }
        public AlarmInfo AlarmInfo { set; get; }
    }

    public enum EqpType
    {
        Cutting,
        Grinding,
        Picking
    }
}
