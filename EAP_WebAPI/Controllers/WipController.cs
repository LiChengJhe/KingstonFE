using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EAP_Library.DTO;
using EAP_WebAPI.Entities;
using EAP_WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EAP_WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class WipController : ControllerBase
    {
        private UnitOfWork _UnitOfWork;
        private IMapper _Mapper;
        public WipController(UnitOfWork unitOfWork, IMapper mapper)
        {
            this._UnitOfWork = unitOfWork;
            this._Mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<EqpWipDTO>> GetAll()
        {
            IEnumerable<EqpWip> wips = this._UnitOfWork.EqpWipRepo.GetAll();
            IEnumerable<EqpWipDTO> wipDTOs = wips.Select(o => this._Mapper.Map<EqpWipDTO>(o));
            return Ok(wipDTOs);
        }
        [HttpGet("{eqpId}")]
        public ActionResult<IEnumerable<EqpWipDTO>> Get(string eqpId)
        {
            IEnumerable<EqpWip> wips = this._UnitOfWork.EqpWipRepo.Get(eqpId);
            IEnumerable<EqpWipDTO> wipDtos= wips.Select(o=>this._Mapper.Map<EqpWipDTO>(o));
            return Ok(wipDtos);
        }


        [HttpGet("daily-statistics/{eqpId}")]
        public ActionResult<IEnumerable<EqpWipDailyStatistics>> GetDailyStatistics(string eqpId)
        {
            IEnumerable<EqpWipDailyStatistics> dailyStatistics = this._UnitOfWork.EqpWipRepo.GetDailyStatistics(eqpId);
            return Ok(dailyStatistics);
        }

        [HttpGet("today-statistics/{eqpId}")]
        public ActionResult<EqpWipDailyStatistics> GetTodayStatistics(string eqpId)
        {
            EqpWipDailyStatistics dailyStatistics = this._UnitOfWork.EqpWipRepo.GetTodayStatistics(eqpId);
            return Ok(dailyStatistics);
        }
    }
}
