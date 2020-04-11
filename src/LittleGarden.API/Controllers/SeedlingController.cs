﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleGarden.API.DTO;
using LittleGarden.Core.Entities;
using LittleGarden.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using AutoMapper;


namespace LittleGarden.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SeedlingController
    {
        private readonly ILogger<SeedlingController> _logger;
        private readonly IDataContext<Seedling> _dataContext;
        private readonly IMapper _mapper;

        public SeedlingController(ILogger<SeedlingController> logger, IDataContext<Seedling> dataContext, IMapper mapper)
        {
            _logger = logger;
            _dataContext = dataContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Return seedlings
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [HttpGet]
        public async Task<IEnumerable<SeedlingDto>> Get(int page = 1, int pageSize = 10)
        {
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));
            var result = await _dataContext.GetAll(new PageConfig(page, pageSize));
            return result.Select(s => _mapper.Map<Seedling, SeedlingDto>(s));
        }
    }
}