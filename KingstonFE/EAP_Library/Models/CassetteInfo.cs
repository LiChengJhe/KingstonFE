using System;
using System.Collections.Generic;
using System.Text;

namespace EAP_Library.Models
{
    public class CassetteInfo: BaseInfo
    {
        public List<LotInfo> Lots { get; set; }
    }
}
