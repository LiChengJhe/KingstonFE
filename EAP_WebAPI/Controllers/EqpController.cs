using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EAP_Library.DTO;
using EAP_WebAPI.Entities;
using EAP_WebAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EAP_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EqpController : ControllerBase
    {
        private UnitOfWork _UnitOfWork;
        private IMapper _Mapper;
        private EqpCachedRepository _CachedRepository;
        public EqpController(UnitOfWork unitOfWork, IMapper mapper, EqpCachedRepository cachedRepository)
        {
            this._UnitOfWork = unitOfWork;
            this._Mapper = mapper;
            this._CachedRepository = cachedRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EqpInfoDTO>>> GetAllAsync()
        {
            Dictionary<string, EqpInfoDTO> keyValues = await this._CachedRepository.GetAllAsync();
            IEnumerable<EqpInfoDTO> eqpDtos = keyValues.Values.ToList().OrderBy(o=>o.Id);
            return Ok(eqpDtos);
        }
       
    }
}
