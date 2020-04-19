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
    public class ImageController : Controller
    {
        private readonly IDataContext<Image> _dataContext;
        private readonly ILogger<ImageController> _logger;
        private readonly IMapper _mapper;

        /// <summary>ImageController
        /// <param name="logger"></param>
        /// <param name="dataContext"></param>
        /// <param name="mapper"></param>
        public ImageController(ILogger<ImageController> logger, IDataContext<Image> dataContext,
            IMapper mapper)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult>  Get(string id)
        {
            if (!ObjectId.TryParse(id, out var objectID)) return base.Problem($"Provided id ({id}) is misformated.");
            var entity = await _dataContext.GetOne(nameof(Image._id), objectID);
            if (entity == null)
            {
                 return  NotFound($"Image with id {id} does not exist");
            }
            return File(entity.Bytes, "image/jpeg");
        }
        [HttpGet]
        [Route("Thumb/{id}")]
        public async Task<IActionResult>  GetThumb(string id)
        {
            if (!ObjectId.TryParse(id, out var objectID)) return base.Problem($"Provided id ({id}) is misformated.");
            var entity = await _dataContext.GetOne(nameof(Image._id), objectID);
            if (entity == null)
            {
                return  NotFound($"Image with id {id} does not exist");
            }
            return File(entity.ThumbBytes, "image/jpeg");
        }
    }
}