using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiVendor_WebApiServer.Models.DTOs;
using MultiVendor_WebApiServer.Services;

namespace MultiVendor_WebApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await unitOfWork.Categories.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var cat = await unitOfWork.Categories.GetWithProperties(id);
            return  cat == null ? NotFound() : Ok(cat);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto dto)
        {
            await unitOfWork.Categories.Create(dto);
            await unitOfWork.SaveAsAsync();
            return Ok("Category Created");
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(Guid Id,CategoryDto dto)
        {
            await unitOfWork.Categories.Update(Id, dto);
            await unitOfWork.SaveAsAsync();
            return Ok("Category Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var c = await unitOfWork.Categories.GetByIdAsync(id);
            if(c==null)
                return NotFound();

            unitOfWork.Categories.RemoveAsync(c);
            await unitOfWork.SaveAsAsync();
            return Ok("Category Deleted");
        }



    }
}
