using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MeetingRoomBookingSystem.Resources;
using MeetingRoomBookingSystem.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MRBS.Core.Models;
using MRBS.Services;
using MRBS.Services.Interfaces;

namespace MeetingRoomBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService; private readonly IMapper _mapper; private readonly IRoomService _roomService;

        public ReservationsController(IReservationService reservationService, IMapper mapper, IRoomService roomService)
        {
            this._mapper = mapper;
            this._reservationService = reservationService;
            this._roomService = roomService;
        }

        // GET: api/Companies
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ReservationResource>>> GetAllReservations()
        {
            var reservations = await _reservationService.GetAllReservations();
            var reservationResources = _mapper.Map<IEnumerable<Reservation>, IEnumerable<ReservationResource>>(reservations);

            return Ok(reservationResources);
        }

        // GET: api/Companies/5
        [HttpGet("gerReservationById")]
        public async Task<ActionResult<ReservationResource>> GetReservationById(int id)
        {
            var reservation = await _reservationService.GetReservationsById(id);
            var reservationResource = _mapper.Map<Reservation, ReservationResource>(reservation);

            return Ok(reservationResource);
        }

        [HttpPut("editReservation")]
        public async Task<ActionResult<ReservationResource>> UpdateReservation(int id, [FromBody] SaveReservationResource saveReservationResource)
        {
            var validator = new SaveReservationResourceValidator();
            var validationResult = await validator.ValidateAsync(saveReservationResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok

            var reservationToBeUpdated = await _reservationService.GetReservationsById(id);

            if (reservationToBeUpdated == null)
                return NotFound();

            var reservation = _mapper.Map<SaveReservationResource, Reservation>(saveReservationResource);

            await _reservationService.UpdateReservation(reservationToBeUpdated, reservation);

            var updatedReservation = await _reservationService.GetReservationsById(id);

            var updatedReservationResource = _mapper.Map<Reservation, ReservationResource>(updatedReservation);

            return Ok(updatedReservationResource);
        }

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPost("addReservation")]
        public async Task<ActionResult<ReservationResource>> CreateReservation([FromBody] SaveReservationResource saveReservationResource)
        {
            var validator = new SaveReservationResourceValidator();
            var validationResult = await validator.ValidateAsync(saveReservationResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok

            var reservationToCreate = _mapper.Map<SaveReservationResource, Reservation>(saveReservationResource);

            var newReservation = await _reservationService.CreateReservation(reservationToCreate);

            var reservation = await _reservationService.GetReservationsById(newReservation.Id);

            var reservationResource = _mapper.Map<Reservation, ReservationResource>(reservation);

            return Ok(reservationResource);
        }*/

        /*[HttpPost("addReservation")]
        public async Task<ActionResult<ReservationResource>> CreateReservation([FromBody] SaveReservationResource saveReservationResource)
        {
            var validator = new SaveReservationResourceValidator();
            var validationResult = await validator.ValidateAsync(saveReservationResource);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var reservationToCreate = _mapper.Map<SaveReservationResource, Reservation>(saveReservationResource);

            try
            {
                bool hasTimeConflict = await _reservationService.HasTimeConflict(reservationToCreate);

                if (hasTimeConflict)
                {
                    return BadRequest("Time conflict: The requested time slot is already booked.");
                }

                var newReservation = await _reservationService.CreateReservation(reservationToCreate);

                var reservation = await _reservationService.GetReservationsById(newReservation.Id);

                var reservationResource = _mapper.Map<Reservation, ReservationResource>(reservation);

                return Ok(reservationResource);
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., database errors) and return an appropriate error response.
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }*/

        [HttpPost("addReservation")]
        public async Task<ActionResult<ReservationResource>> CreateReservation([FromBody] SaveReservationResource saveReservationResource)
        {
            var validator = new SaveReservationResourceValidator();
            var validationResult = await validator.ValidateAsync(saveReservationResource);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var reservationToCreate = _mapper.Map<SaveReservationResource, Reservation>(saveReservationResource);

            try
            {
                // Get the room associated with the reservation
                var room = await _roomService.GetRoomsById(reservationToCreate.RoomId);
                if (room == null)
                {
                    return BadRequest("Room not found: You either did not enter a room Number or the requested room does not exist.");
                }

                // Check if the room capacity is enough for the number of attendees
                if (room.Capacity < reservationToCreate.NumberAttendees)
                {
                    return BadRequest("Room capacity exceeded: The requested room cannot fit the number of attendees.");
                }

                bool hasTimeConflict = await _reservationService.HasTimeConflict(reservationToCreate);

                if (hasTimeConflict)
                {
                    return BadRequest("Time conflict: The requested time slot is already booked.");
                }

                var newReservation = await _reservationService.CreateReservation(reservationToCreate);

                var reservation = await _reservationService.GetReservationsById(newReservation.Id);

                var reservationResource = _mapper.Map<Reservation, ReservationResource>(reservation);

                return Ok(reservationResource);
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., database errors) and return an appropriate error response.
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _reservationService.GetReservationsById(id);

            await _reservationService.DeleteReservation(reservation);

            return NoContent();
        }


    }
}
