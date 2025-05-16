using AutoMapper;
using Booking.Core.DTOs.Category;
using Booking.Core.Enities;
using Booking.Core.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IUnitOfWork _unit, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _unit.Categories.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Editor")]

        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _unit.Categories.GetOneAsync(e => e.Id == id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _mapper.Map<Category>(categoryDTO);
            await _unit.Categories.AddAsync(category);
            await _unit.CompleteAsync();
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _unit.Categories.GetOneAsync(e => e.Id == id);
            if (category == null)
                return NotFound();

            _mapper.Map(categoryDTO, category);
            await _unit.CompleteAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _unit.Categories.GetOneAsync(e => e.Id == id);
            if (category == null)
                return NotFound();
            _unit.Categories.Delete(category);
            await _unit.CompleteAsync();
            return NoContent();
        }
    }
}
