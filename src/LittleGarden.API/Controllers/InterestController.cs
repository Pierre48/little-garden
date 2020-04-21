using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LittleGarden.API.DTO;
using LittleGarden.Core.Entities;
using LittleGarden.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace LittleGarden.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class InterestController : Controller
    {
        private readonly IDataContext<Interest> _dataContext;
        private readonly ILogger<InterestController> _logger;
        private readonly IMapper _mapper;

        /// <summary>ImageController
        /// <param name="logger"></param>
        /// <param name="dataContext"></param>
        /// <param name="mapper"></param>
        public InterestController(ILogger<InterestController> logger, IDataContext<Interest> dataContext,
            IMapper mapper)
        {
            _logger = logger;
            _dataContext = dataContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult>  Get()
        {
            var entities = await _dataContext.GetAll();
            return Ok(entities.Select(x=>_mapper.Map<Interest,InterestDto>(x)));
        }
    }
}