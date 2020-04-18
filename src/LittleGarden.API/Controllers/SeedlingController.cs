using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LittleGarden.API.DTO;
using LittleGarden.Core.Entities;
using LittleGarden.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LittleGarden.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SeedlingController
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
        public async Task<IEnumerable<SeedlingDto>> Get(int page = 1, int pageSize = 10)
        {
            _logger.LogDebug($"Get page : {page}, pageSize : {pageSize}");
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));
            var result = await _dataContext.GetAll(new PageConfig(page, pageSize));
            return result.Select(s => _mapper.Map<Seedling, SeedlingDto>(s));
        }
    }
}