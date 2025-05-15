using AutoMapper;
using Booking.API.Helpers;
using Booking.Core.DTOs.Event;
using Booking.Core.Enities;
using Booking.Core.Intefaces;
using Microsoft.AspNetCore.Mvc;
    
namespace Booking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvenetController(IUnitOfWork _unit, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _unit.Events.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventToget = await _unit.Events.GetOneAsync(e => e.Id == id);
            if (eventToget == null)
                return NotFound();
            return Ok(eventToget);
        }

       
        [HttpPost]
        
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventDTO eventDTO)
        {
            if (ModelState.IsValid)
            {
                if (!IsValidImage(eventDTO.Image))
                    return BadRequest("Invalid Image,\n Extensions allowed (.JPG, .JPEG, PNG) only and size should be less than 1MB");
                
                if (!await _unit.Categories.IsValid(eventDTO.CategoryId))
                    return BadRequest("Inavlid category");

                using var dataStream = new MemoryStream();
                await eventDTO.Image.CopyToAsync(dataStream);

                var eventToAdd = _mapper.Map<Event>(eventDTO);
                eventToAdd.Image = dataStream.ToArray();
                await _unit.Events.AddAsync(eventToAdd);
                await _unit.CompleteAsync();

                return Ok(eventToAdd);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromForm] UpdateEventDTO eventDTO)
        {
            var eventToUpdate = await _unit.Events.GetOneAsync(e => e.Id == id);
            
            if (eventToUpdate == null)
                return NotFound();
            
            if (!await _unit.Categories.IsValid(eventDTO.CategoryId))
                return BadRequest("Inavlid category");

            if (eventDTO.Image is not null)
            {
                if (!IsValidImage(eventDTO.Image))
                    return BadRequest("Invalid Image,\n Extensions allowed (.JPG, .JPEG, PNG) only and size should be less than 1MB");

                using var dataStream = new MemoryStream();
                await eventDTO.Image.CopyToAsync(dataStream);
                eventToUpdate.Image = dataStream.ToArray();
            }

            _mapper.Map(eventDTO, eventToUpdate);
            _unit.Events.Update(eventToUpdate);
            await _unit.CompleteAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventToDelete = await _unit.Events.GetOneAsync(e => e.Id == id);
            if (eventToDelete == null)
                return NotFound();
            _unit.Events.Delete(eventToDelete);
            await _unit.CompleteAsync();
            return NoContent();
        }

        private bool IsValidImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return false;
            var allowedExtensions = Utilities.AllowedImagesExtentsions;
            var fileExtension = Path.GetExtension(image.FileName).ToLower();
            return allowedExtensions.Contains(fileExtension) && image.Length <= Utilities.MaxAllowedImageSize;
        }

    }
}
