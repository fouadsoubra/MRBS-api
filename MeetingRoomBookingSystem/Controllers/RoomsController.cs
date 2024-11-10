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
using MRBS.Services.Interfaces;

namespace MeetingRoomBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService; 
        private readonly IMapper _mapper;

        public RoomsController(IRoomService roomService, IMapper mapper)
        {
            this._mapper = mapper;
            this._roomService = roomService;
        }

        // GET: api/Companies
        [HttpGet("getAllRooms")]
        public async Task<ActionResult<IEnumerable<RoomResource>>> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRooms();
            var roomResources = _mapper.Map<IEnumerable<Room>, IEnumerable<RoomResource>>(rooms);

            return Ok(roomResources);
        }

        // GET: api/Companies/5
        [HttpGet("getRoomById")]
        public async Task<ActionResult<RoomResource>> GetRoomById(int id)
        {
            var room = await _roomService.GetRoomsById(id);
            var roomResource = _mapper.Map<Room, RoomResource>(room);

            return Ok(roomResource);
        }




        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("editRoom")]
        public async Task<ActionResult<RoomResource>> UpdateRoom(int id, [FromBody] SaveRoomResource saveRoomResource)
        {
            var validator = new SaveRoomResourceValidator();
            var validationResult = await validator.ValidateAsync(saveRoomResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok

            var roomToBeUpdated = await _roomService.GetRoomsById(id);

            if (roomToBeUpdated == null)
                return NotFound();

            var room = _mapper.Map<SaveRoomResource, Room>(saveRoomResource);

            await _roomService.UpdateRoom(roomToBeUpdated, room);

            var updatedRoom = await _roomService.GetRoomsById(id);

            var updatedRoomResource = _mapper.Map<Room, RoomResource>(updatedRoom);

            return Ok(updatedRoomResource);
        }
        [HttpPost("addRoom")]
        public async Task<ActionResult<RoomResource>> CreateRoom([FromBody] SaveRoomResource saveRoomResource)
        {
            var validator = new SaveRoomResourceValidator();
            var validationResult = await validator.ValidateAsync(saveRoomResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok
            if(await _roomService.RoomExistsAsync(saveRoomResource.Name))
            {
                return BadRequest("Room already exist");
            }

            var roomToCreate = _mapper.Map<SaveRoomResource, Room>(saveRoomResource);

            var newRoom = await _roomService.CreateRoom(roomToCreate);

            var room = await _roomService.GetRoomsById(newRoom.Id);

            var roomResource = _mapper.Map<Room, RoomResource>(room);

            return Ok(roomResource);
        }

        // DELETE: api/Companies/5
        [HttpDelete("deleteRoom")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _roomService.GetRoomsById(id);

            await _roomService.DeleteRoom(room);

            return NoContent();
        }

    }
}
