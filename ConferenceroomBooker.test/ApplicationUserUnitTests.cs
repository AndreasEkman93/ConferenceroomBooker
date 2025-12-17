using System;
using System.Collections.Generic;
using System.Text;
using ConferenceroomBooker.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ConferenceroomBooker.test
{

    public class ApplicationUserUnitTests
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
        public void TestValidUserModel()
        {
            // Arrange
            ApplicationUser user = new ApplicationUser
            {
                Name = "Valid User",
                Password = "ValidPassword"
            };
            // Act
            var isValid = ValidateModel(user);
            // Assert
            Assert.Empty(isValid);
        }
        [Fact]
        public void TestInvalidUserModel()
        {
            // Arrange
            ApplicationUser user = new ApplicationUser
            {
                Name = "", // Invalid: Name is required
                Password = "12" // Invalid: Password too short
            };
            // Act
            var validationResults = ValidateModel(user);
            // Assert
            Assert.Equal(2, validationResults.Count);
            Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Name"));
            Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Password"));
        }
    }
}
