using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Password_manager.Models;
using Password_manager.Services.Interface;

namespace Password_manager.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;
        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }
        /// <summary>
        /// 取資料
        /// </summary>
        [HttpGet]
        public IActionResult GetAll()
        {
            List<DataItem> datalist = _dataService.GetAll();
            return Ok(datalist);
        }
        /// <summary>
        /// 取篩選資料
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            DataItem? item = _dataService.GetById(id);
            return item == null ? NotFound() : Ok(item);
        }
        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        public IActionResult Add([FromBody] DataItem item)
        {
            _dataService.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }
        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] DataItem item)
        {
            if (id != item.Id) return BadRequest();
            _dataService.Update(item);
            return Ok();
        }
        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public IActionResult DeleteById(string id) 
        {
            _dataService.Delete(id);
            return Ok();
        }
    }
}
