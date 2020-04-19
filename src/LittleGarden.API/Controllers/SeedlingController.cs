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
    public class SeedlingController : Controller
    {
        private readonly IDataContext<Seedling> _dataContext;
        private readonly ILogger<SeedlingController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dataContext"></param>
        /// <param name="mapper"></param>
        public SeedlingController(ILogger<SeedlingController> logger, IDataContext<Seedling> dataContext,
            IMapper mapper)
        {
            _logger = logger;
            _dataContext = dataContext;
            _mapper = mapper;
        }

        /// <summary>
        ///     Return seedlings
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10)
        {
            _logger.LogDebug($"Get page : {page}, pageSize : {pageSize}");
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));
            var result = await _dataContext.GetAll(new PageConfig(page, pageSize));
            return Ok(result.Select(s => _mapper.Map<Seedling, SeedlingDto>(s)));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult>  Get(string id)
        {
            ObjectId objectID;
            if (!ObjectId.TryParse(id, out objectID)) return base.Problem($"Provided id ({id}) is misformated.");
            var entity = await _dataContext.GetOne(nameof(Seedling._id), ObjectId.Parse(id));
            if (entity == null)
            {
                 return  NotFound($"Seedling with id {id} does not exist");
            }

            return Ok(_mapper.Map<Seedling, SeedlingDto>(entity));
        }
    }
}