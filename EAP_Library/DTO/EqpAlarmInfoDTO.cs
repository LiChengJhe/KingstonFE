using System;
using System.Collections.Generic;
using System.Text;

namespace EAP_Library.DTO
{
    public class EqpAlarmInfoDTO : BaseInfoDTO
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime? TimeTo { get; set; }
    }


    public enum EqpAlarmType
    {
        ON_GOING,
        CLEAR
    }
}
