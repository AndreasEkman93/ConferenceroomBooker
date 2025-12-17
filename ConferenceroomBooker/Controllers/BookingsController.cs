using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConferenceroomBooker.Data;
using ConferenceroomBooker.Models;
using ConferenceroomBooker.Services;

namespace ConferenceroomBooker.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationUserService applicationUserService;
        private readonly BookingService bookingService;
        private readonly ConferenceRoomService conferenceRoomService;

        public BookingsController(ApplicationUserService applicationUserService, BookingService bookingService, ConferenceRoomService conferenceRoomService)
        {
            this.applicationUserService = applicationUserService;
            this.bookingService = bookingService;
            this.conferenceRoomService = conferenceRoomService;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            return View(await bookingService.GetAllBookingsAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await bookingService.GetBookingByIdAsync(id.Value);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public async Task<IActionResult> Create()
        {
            await PopulateViewBags();
            return View();
        }

        private async Task PopulateViewBags()
        {
            ViewBag.ConferenceRooms = new SelectList(await conferenceRoomService.GetAllAsync(), "Id", "Name");
            ViewBag.ApplicationUsers = new SelectList(await applicationUserService.GetAllUsersAsync(), "Id", "Name");
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ConferenceRoomId,ApplicationUserId,StartTime,EndTime")] Booking booking)
        {
            if(!ModelState.IsValid)
            {
                await PopulateViewBags();
                return View(booking);
            }

            try
            {
                await bookingService.AddBookingAsync(booking);
                return RedirectToAction(nameof(Index));
            }
            catch(ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                
            }
            catch(InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            PopulateViewBags();
            return View(booking);
        }


        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await bookingService.GetBookingByIdAsync(id.Value);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await bookingService.GetBookingByIdAsync(id);
            if (booking != null)
            {
                await bookingService.RemoveBookingAsync(booking.Id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return bookingService.GetBookingByIdAsync(id) != null;
        }
    }
}
