using System;
using System.Collections.Generic;
using System.Text;

namespace EAP_Library.Models
{
    public class LotInfo: BaseInfo
    {
        public string LotId { get; set; }
        public List<int> WaferIds { get; set; }
    }
}
