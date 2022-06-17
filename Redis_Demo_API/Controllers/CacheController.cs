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
        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            //Models1
            //var models = await _cacheService.GetAsync<List<TestModel>>("models") ?? new List<TestModel>();
            //var model = new TestModel(Faker.Name.First(), Faker.Name.Last());
            //models.Add(model);
            //await _cacheService.SetAsync("models", models);
            //return Ok(model);

            //Models2
            var models = await _cacheService.GetAsync<List<TestModel2>>("models3") ?? new List<TestModel2>();
            var products = new List<Product>()
            {
                new Product(){ Id=1, Name="Product_1", Price=50, CategoryId=1},
                new Product(){ Id=2, Name="Product_2", Price=150, CategoryId=2},
                new Product(){ Id=3, Name="Product_3", Price=250, CategoryId=2},
                new Product(){ Id=4, Name="Product_4", Price=350, CategoryId=1},
                new Product(){ Id=5, Name="Product_5", Price=350, CategoryId=1},
                new Product(){ Id=6, Name="Product_6", Price=350, CategoryId=1}
            };
            var categories = new List<Category>()
            {
                new Category(){ Id=1, Name="Category_1"},
                new Category(){ Id=2, Name="Category_2"},
                new Category(){ Id=3, Name="Category_3"},
                new Category(){ Id=4, Name="Category_4"},
                new Category(){ Id=5, Name="Category_5"}
            };
            var model = new TestModel2() { Products=products, Categories=categories};
            models.Add(model);
            await _cacheService.SetAsync("models3", models);
            return Ok(model);

        }
        #endregion
        #region Add
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]TestModel2 model)
        {
            if (model == null || model?.Products == null || model?.Categories == null)
            {
                return BadRequest();
            }
            var models = await _cacheService.GetAsync<List<TestModel2>>("models3") ?? new List<TestModel2>();
            models.Add(model);
            await _cacheService.SetAsync("models3", models);
            return Ok(models);
        }
        #endregion
        #region GetById
        [HttpGet]
        [Route("GetByProductId")]
        public async Task<IActionResult> GetByProductId(int id)
        {
            var models = await _cacheService.GetAsync<List<TestModel2>>("models3");
            if (models != null && models?.Count() > 0)
            {
                try
                {
                    var model = models[0].Products.FirstOrDefault(x => x.Id == id);
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
        #region GetProductsByCategoryId
        [HttpGet]
        [Route("GetProductsByCategoryId")]
        public async Task<IActionResult> GetProductsByCategoryId(int id)
        {
            var models = await _cacheService.GetAsync<List<TestModel2>>("models3");
            if (models != null && models?.Count() > 0)
            {
                try
                {
                    var model = models[0].Products.Where(x => x.CategoryId == id);
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
        #region GetCategories
        [HttpGet]
        [Route("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var models = await _cacheService.GetAsync<List<TestModel2>>("models3");
            if (models != null && models?.Count() > 0)
            {
                try
                {
                    var model = models[0].Categories;
                    if (model != null && model.Count() > 0)
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
        #region GetCategories
        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var models = await _cacheService.GetAsync<List<TestModel2>>("models3");
            if (models != null && models?.Count() > 0)
            {
                try
                {
                    var model = models[0].Products;
                    if (model != null && model.Count() > 0)
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
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> Get()
        {
            var cache = await _cacheService.GetAsync<List<TestModel2>>("models3");
            return Ok(cache);
        }
        #endregion
        #region ScanKeysAsync
        [HttpGet]
        [Route("ScanKeysAsync")]
        public async Task<IActionResult> ScanKeysAsync(string match, string count)
        {
            var keys = await _cacheService.ScanKeysAsync(match, count);
            return Ok(keys);
        }
        #endregion

    }
}
