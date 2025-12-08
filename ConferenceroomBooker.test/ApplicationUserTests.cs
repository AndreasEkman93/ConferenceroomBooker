using ConferenceroomBooker.Data;
using ConferenceroomBooker.Models;
using ConferenceroomBooker.Services;
using Microsoft.EntityFrameworkCore;

namespace ConferenceroomBooker.test
{
    public class ApplicationUserTests : IDisposable
    {
        private readonly ApplicationDbContext context;
        private readonly ApplicationUserService applicationUserService;

        public ApplicationUserTests()
        {
            context = CreateInMemoryDbContext();
            applicationUserService = new ApplicationUserService(context);
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

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
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
            applicationUserService.AddUserAsync(user);
            var retrievedUser = await applicationUserService.GetUserAsync(user.Id);
            // Assert
            Assert.NotNull(retrievedUser);
            Assert.Equal("Database User", retrievedUser.Name);
        }

        [Fact]
        public async Task TestUserUpdateAndRetrieve()
        {
            // Arrange
            ApplicationUser user = new ApplicationUser
            {
                Name = "Initial User",
                Password = "InitialPassword"
            };
            applicationUserService.AddUserAsync(user);
            // Act
            user.Name = "Updated Database User";
            await applicationUserService.UpdateUserAsync(user);
            var updatedUser = await applicationUserService.GetUserAsync(user.Id);
            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal("Updated Database User", updatedUser.Name);
        }

        [Fact]
        public async Task TestRetrieveNonExistentUser()
        {
            // Act
            var nonExistentUser = await applicationUserService.GetUserAsync(999);
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
            applicationUserService.AddUserAsync(user);
            await applicationUserService.DeleteUserAsync(user.Id);
            var deletedUser = await applicationUserService.GetUserAsync(user.Id);

            // Assert
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task TestUserGetAll()
        {
            // Arrange
            ApplicationUser user1 = new ApplicationUser
            {
                Name = "User One",
                Password = "PasswordOne"
            };
            ApplicationUser user2 = new ApplicationUser
            {
                Name = "User Two",
                Password = "PasswordTwo"
            };
            applicationUserService.AddUserAsync(user1);
            applicationUserService.AddUserAsync(user2);

            // Act
            List<ApplicationUser> allUsers = await applicationUserService.GetAllAsync();

            // Assert
            Assert.Equal(2, allUsers.Count);
            Assert.NotNull(allUsers[0]);
            Assert.NotNull(allUsers[1]);





        }
    }
}
