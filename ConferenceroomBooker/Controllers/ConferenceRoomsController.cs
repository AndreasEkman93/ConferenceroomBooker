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
    public class ConferenceRoomsController : Controller
    {
        private readonly ConferenceRoomService conferenceRoomService;

        public ConferenceRoomsController(ConferenceRoomService conferenceRoomService)
        {
            this.conferenceRoomService = conferenceRoomService;
        }

        // GET: ConferenceRooms
        public async Task<IActionResult> Index()
        {
            return View(await conferenceRoomService.GetAllAsync());
        }

        // GET: ConferenceRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conferenceRoom = await conferenceRoomService.GetConferenceRoomByIdAsync(id.Value);
            if (conferenceRoom == null)
            {
                return NotFound();
            }

            return View(conferenceRoom);
        }

        // GET: ConferenceRooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConferenceRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ConferenceRoom conferenceRoom)
        {
            if (ModelState.IsValid)
            {
                await conferenceRoomService.AddConferenceRoomAsync(conferenceRoom);
                return RedirectToAction(nameof(Index));
            }
            return View(conferenceRoom);
        }

        // GET: ConferenceRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conferenceRoom = await conferenceRoomService.GetConferenceRoomByIdAsync(id.Value);
            if (conferenceRoom == null)
            {
                return NotFound();
            }
            return View(conferenceRoom);
        }

        // POST: ConferenceRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ConferenceRoom conferenceRoom)
        {
            if (id != conferenceRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await conferenceRoomService.UpdateAsync(conferenceRoom);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConferenceRoomExists(conferenceRoom.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(conferenceRoom);
        }

        // GET: ConferenceRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conferenceRoom = await conferenceRoomService.GetConferenceRoomByIdAsync(id.Value);
            if (conferenceRoom == null)
            {
                return NotFound();
            }

            return View(conferenceRoom);
        }

        // POST: ConferenceRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conferenceRoom = await conferenceRoomService.GetConferenceRoomByIdAsync(id);
            if (conferenceRoom != null)
            {
                conferenceRoomService.DeleteConferenceRoomAsync(conferenceRoom.Id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ConferenceRoomExists(int id)
        {
            return conferenceRoomService.GetConferenceRoomByIdAsync(id).Result != null;
        }
    }
}
