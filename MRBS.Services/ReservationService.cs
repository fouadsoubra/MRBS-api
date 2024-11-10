using Microsoft.EntityFrameworkCore;
using MRBS.Core.Interfaces;
using MRBS.Core.Models;
using MRBS.Core.Repositories;
using MRBS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRBS.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReservationService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Reservation> CreateReservation(Reservation newReservation)
        {
            await _unitOfWork.Reservations.AddAsync(newReservation);
            await _unitOfWork.CommitAsync();
            return newReservation;
        }

        public async Task DeleteReservation(Reservation reservation)
        {
            _unitOfWork.Reservations.Remove(reservation);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllReservations()
        {
            return await _unitOfWork.Reservations
                .GetAllReservationsAsync();
        }

        public async Task<Reservation> GetReservationsById(int id)
        {
            return await _unitOfWork.Reservations
                .GetReservationsByIdAsync(id);
        }

        public async Task UpdateReservation(Reservation reservationToBeUpdated, Reservation reservation)
        {
            reservationToBeUpdated.StartTime = reservation.StartTime;
            reservationToBeUpdated.EndTime = reservation.EndTime;
            reservationToBeUpdated.DateOfMeeting = reservation.DateOfMeeting;

            await _unitOfWork.CommitAsync();
        }

        public async Task<List<Reservation>> GetConflictingReservations(DateTime startTime, DateTime endTime)
        {
            var conflictingReservations = new List<Reservation>();

            var reservations = await _unitOfWork.Reservations.GetAllReservationsAsync();

            foreach (var reservation in reservations)
            {
                if ((reservation.StartTime < endTime && reservation.EndTime > startTime) ||
                    (reservation.StartTime == endTime || reservation.EndTime == startTime))
                {
                    conflictingReservations.Add(reservation);
                }
            }

            return conflictingReservations;
        }
        public async Task<bool> HasTimeConflict(Reservation reservation)
        {
            // Check if there are any existing reservations that conflict with the provided reservation's time slot
            var conflictingReservations = await GetConflictingReservations(reservation.StartTime, reservation.EndTime);

            // If there are any conflicting reservations, return true
            if (conflictingReservations.Any())
            {
                return true;
            }

            return false;
        }
    }
}
