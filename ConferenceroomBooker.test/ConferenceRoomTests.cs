using System;
using System.Collections.Generic;
using System.Text;
using ConferenceroomBooker.Data;
using ConferenceroomBooker.Models;
using ConferenceroomBooker.Services;
using Microsoft.EntityFrameworkCore;

namespace ConferenceroomBooker.test
{
    public class ConferenceRoomTests : IDisposable
    {
        private ApplicationDbContext context;
        private ConferenceRoomService conferenceRoomService;

        public ConferenceRoomTests()
        {
            context = CreateInMemoryDbContext();
            conferenceRoomService = new ConferenceRoomService(context);
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        private ApplicationDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unik databas per test
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public void TestConferenceRoomCreateAndEdit()
        {
            // Arrange
            var conferenceRoom = new ConferenceroomBooker.Models.ConferenceRoom
            {
                Name = "Room A"
            };
            // Act
            conferenceRoom.Name = "Updated Room A";
            // Assert
            Assert.Equal("Updated Room A", conferenceRoom.Name);
        }

        [Fact]
        public async Task TestConferenceRoomAddToDatabaseAndRetrieve()
        {
            // Arrange
            var conferenceRoom = new ConferenceRoom
            {
                Name = "Database Room"
            };
            // Act
            await conferenceRoomService.AddConferenceRoomAsync(conferenceRoom);
            var retrievedRoom = await conferenceRoomService.GetConferenceRoomByIdAsync(conferenceRoom.Id);
            // Assert
            Assert.NotNull(retrievedRoom);
            Assert.Equal("Database Room", retrievedRoom.Name);
        }

        [Fact]
        public async Task TestConferenceRoomRetrieveNonExistent()
        {
            // Act
            var retrievedRoom = await conferenceRoomService.GetConferenceRoomByIdAsync(999);
            // Assert
            Assert.Null(retrievedRoom);
        }

        [Fact]
        public async Task TestConferenceRoomAddMultipleAndRetrieve()
        {
            // Arrange
            var room1 = new ConferenceRoom { Name = "Room 1" };
            var room2 = new ConferenceRoom { Name = "Room 2" };

            // Act
            await conferenceRoomService.AddConferenceRoomAsync(room1);
            await conferenceRoomService.AddConferenceRoomAsync(room2);
            var allRooms = await conferenceRoomService.GetAllAsync();

            // Assert
            Assert.Equal(2, allRooms.Count);
        }

        [Fact]
        public async Task TestConferenceRoomRetrieveAndDelete()
        {
            // Arrange
            var room1 = new ConferenceRoom
            {
                Name = "Room to Delete"
            };
            await conferenceRoomService.AddConferenceRoomAsync(room1);

            // Act
            conferenceRoomService.DeleteConferenceRoomAsync(room1.Id);
            var retrievedRoom = await conferenceRoomService.GetConferenceRoomByIdAsync(room1.Id);

            // Assert
            Assert.Null(retrievedRoom);
        }

        [Fact]
        public async Task TestConferenceRoomUpdate()
        {
            // Arrange
            var room = new ConferenceRoom
            {
                Name = "Initial Room"
            };
            await conferenceRoomService.AddConferenceRoomAsync(room);
            // Act
            room.Name = "Updated Room";
            await conferenceRoomService.UpdateAsync(room);
            var updatedRoom = await conferenceRoomService.GetConferenceRoomByIdAsync(room.Id);
            // Assert
            Assert.NotNull(updatedRoom);
            Assert.Equal("Updated Room", updatedRoom.Name);
        }
    }
}
