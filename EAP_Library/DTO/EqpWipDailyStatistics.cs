using System;
using System.Collections.Generic;
using System.Text;

namespace EAP_Library.DTO
{
    public class EqpWipDailyStatistics
    {
        public string EqpId { get; set; }
        public int WipCount { get; set; }
        public DateTime Date { get; set; }
    }
}
