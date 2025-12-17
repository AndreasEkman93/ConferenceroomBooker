using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConferenceroomBooker.test
{
    public class ConferenceRoomUnitTests
    {
        // Helper method to validate a model
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }

        [Fact]
        public void TestValidConferenceRoomModel()
        {
            // Arrange
            var conferenceRoom = new ConferenceroomBooker.Models.ConferenceRoom
            {
                Name = "Valid Room"
            };
            // Act
            var isValid = ValidateModel(conferenceRoom);
            // Assert
            Assert.Empty(isValid);
        }

        [Fact]
        public void TestInvalidConferenceRoomModel()
        {
            // Arrange
            var conferenceRoom = new ConferenceroomBooker.Models.ConferenceRoom
            {
                Name = "" // Invalid: Name is required
            };
            // Act
            var validationResults = ValidateModel(conferenceRoom);
            // Assert
            Assert.Single(validationResults);
            Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Name"));
        }
        }
}
