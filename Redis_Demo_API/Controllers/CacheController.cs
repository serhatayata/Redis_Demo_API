using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Redis_Demo_API.Entities;
using Redis_Demo_API.Services.Abstract;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace Redis_Demo_API.Controllers
{
    public class CacheController : Controller
    {
        private readonly ICacheService _cacheService;
        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        #region Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var models = await _cacheService.GetAsync<List<TestModel>>("models") ?? new List<TestModel>();
            var model = new TestModel(Faker.Name.First(), Faker.Name.Last());
            models.Add(model);
            await _cacheService.SetAsync("models", models);
            return Ok(model);

        }
        #endregion
        #region GetById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var models = await _cacheService.GetAsync<List<TestModel>>("models");
            if (models != null)
            {
                try
                {
                    var guidId = new Guid(id);
                    var model = models.FirstOrDefault(p => p.Id == guidId);
                    if (model != null)
                    {
                        return Ok(model);
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
            }
            return BadRequest("Not Found");
        }
        #endregion
        #region Get
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var cache = await _cacheService.GetAsync<List<TestModel>>("models");
            return Ok(cache);
        }
        #endregion

    }
}
