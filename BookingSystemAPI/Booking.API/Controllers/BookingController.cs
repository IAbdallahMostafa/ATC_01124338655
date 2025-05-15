using AutoMapper;
using Booking.Core.DTOs.Booking;
using Booking.Core.Enities;
using Booking.Core.Enities.Authentication;
using Booking.Core.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Booking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController (IUnitOfWork _unit, IMapper _mapper, UserManager<ApplicationUser> _userManager) : ControllerBase
    {
        private string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId!;
        }
        [HttpGet]
        public async Task<IActionResult> GetMyBookings()
        {
            string userId = GetUserId();
            var bookings = await _unit.Books.GetAllAsync(e => e.ApplicationUserId == GetUserId());
            return Ok(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> BookingEvent([FromBody] BookingEventDTO bookingDTO)
        {
            if (ModelState.IsValid)
            {
                if (!IsValidUser(bookingDTO.ApplicationUserId))
                    return BadRequest("You are not authorized to book this event");
               
                if (!await _unit.Events.IsValid(bookingDTO.EventId))
                    return BadRequest("Invalid event");
                
                var alreadyBooked = await _unit.Books.GetOneAsync(e => e.ApplicationUserId == bookingDTO.ApplicationUserId && e.EventId == bookingDTO.EventId);
                if (alreadyBooked != null)
                    return BadRequest("You have already booked this event");

                var booking = _mapper.Map<Book>(bookingDTO);
                await _unit.Books.AddAsync(booking);
                await _unit.CompleteAsync();    

                return Ok(booking);
            }
            return BadRequest(ModelState);
        }
        private bool IsValidUser(string userId)
        {
            return userId == GetUserId();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var bookingToDelete = await _unit.Books.GetOneAsync(e => e.Id == id);
            if (bookingToDelete == null)
                return NotFound();
            _unit.Books.Delete(bookingToDelete);
            await _unit.CompleteAsync();
            return NoContent();
        }
    }
}
