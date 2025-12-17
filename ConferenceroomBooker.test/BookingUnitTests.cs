using System;
using System.Collections.Generic;
using System.Text;
using ConferenceroomBooker.Models;
using ConferenceroomBooker.Services;

namespace ConferenceroomBooker.test
{
    public class BookingUnitTests
    {
        [Fact]
        public void TestBookingDateEndAfterStart()
        {
            // Arrange
            var booking = new Booking
            {
                ConferenceRoomId = 1,
                ApplicationUserId = 1,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2) // End time after start time
            };
            // Act & Assert
            var result = booking.DateValidation(booking.StartTime, booking.EndTime);
            Assert.True(result);
        }
        [Fact]
        public void TestBookingDateEndBeforeStart()
        {
            // Arrange
            var booking = new Booking
            {
                ConferenceRoomId = 1,
                ApplicationUserId = 1,
                StartTime = new DateTime(2024, 7, 10, 14, 0, 0),
                EndTime = new DateTime(2024, 7, 10, 13, 0, 0) // End time before start time
            };
            // Act
            var ex = Assert.Throws<ArgumentException>(() => booking.DateValidation(booking.StartTime, booking.EndTime));
            Assert.Equal("EndTime must be after StartTime", ex.Message);
        }
        [Fact]
        public void TestBookingDateStartInPast()
        {
            // Arrange
            var booking = new Booking
            {
                ConferenceRoomId = 1,
                ApplicationUserId = 1,
                StartTime = DateTime.Now.AddHours(-2), // Start time in the past
                EndTime = DateTime.Now.AddHours(1)
            };
            // Act
            var ex = Assert.Throws<ArgumentException>(() => booking.DateValidation(booking.StartTime, booking.EndTime));
            Assert.Equal("StartTime must be in the future", ex.Message);
        }
    }
}
