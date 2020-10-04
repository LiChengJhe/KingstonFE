using System;
using System.Collections.Generic;
using System.Text;

namespace EAP_Library.DTO
{
    public class CassetteInfoDTO: BaseInfoDTO
    {
        public string Id { get; set; }
        public List<LotInfoDTO> Lots { get; set; }
    }
}
