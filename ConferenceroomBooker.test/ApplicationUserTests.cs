using ConferenceroomBooker.Data;
using ConferenceroomBooker.Models;
using ConferenceroomBooker.Services;
using Microsoft.EntityFrameworkCore;

namespace ConferenceroomBooker.test
{
    public class ApplicationUserTests: IDisposable
    {
        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        private ApplicationDbContext context;
        private ApplicationUserService applicationUserService;
        public ApplicationUserTests()
        {
            context = new ApplicationDbContext(dbContextOptions);
            context.Database.EnsureCreated();
            applicationUserService = new ApplicationUserService(context);
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void TestUserCreateAndEdit()
        {
            // Arrange
            ApplicationUser user = new ApplicationUser
            {
                Name = "Test User",
                Password = "TestPassword"
            };
            // Act

            user.Name = "Updated User";
            // Assert

            Assert.Equal("Updated User", user.Name);
        }

        [Fact]
        public async Task TestUserAddToDatabaseAndRetrieve()
        {
            // Arrange
            ApplicationUser user = new ApplicationUser
            {
                Name = "Database User",
                Password = "DBPassword"
            };
            // Act
            applicationUserService.AddUser(user);
            var retrievedUser = await applicationUserService.GetUser(user.Id);
            // Assert
            Assert.NotNull(retrievedUser);
            Assert.Equal("Database User", retrievedUser.Name);
        }

        [Fact]
        public async Task TestUserUpdateAndRetrieve() {
            // Arrange
            ApplicationUser user = new ApplicationUser
            {
                Name = "Initial User",
                Password = "InitialPassword"
            };
            applicationUserService.AddUser(user);
            // Act
            user.Name = "Updated Database User";
            await applicationUserService.UpdateUser(user);
            var updatedUser = await applicationUserService.GetUser(user.Id);
            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal("Updated Database User", updatedUser.Name);
        }

        [Fact]
        public async Task TestRetrieveNonExistentUser() {
            // Act
            var nonExistentUser = await applicationUserService.GetUser(999);
            // Assert
            Assert.Null(nonExistentUser);
        }

        [Fact]
        public async Task TestUserDelete()
        {
            // Arrange
            ApplicationUser user = new ApplicationUser
            {
                Name = "User To Delete",
                Password = "DeletePassword"
            };

            // Act
            applicationUserService.AddUser(user);
            await applicationUserService.DeleteUser(user.Id);
            var deletedUser = await applicationUserService.GetUser(user.Id);

            // Assert
            Assert.Null(deletedUser);
        }



    }
}
