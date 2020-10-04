using System;
using System.Collections.Generic;
using System.Text;

namespace EAP_Library.DTO
{
    public class LotInfoDTO: BaseInfoDTO
    {
        public string Id { get; set; }
        public List<WaferInfoDTO> Wafers { get; set; }
    }
}
