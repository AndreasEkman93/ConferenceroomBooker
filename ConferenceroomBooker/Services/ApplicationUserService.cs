using ConferenceroomBooker.Data;
using ConferenceroomBooker.Models;

namespace ConferenceroomBooker.Services
{
    public class ApplicationUserService
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async void AddUser(ApplicationUser user)
        {
            await context.ApplicationUsers.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await context.ApplicationUsers.FindAsync(id);
            if (user != null)
            {
                context.ApplicationUsers.Remove(user);
                await context.SaveChangesAsync();
            }

        }

        public async Task<ApplicationUser?> GetUser(int id)
        {
            return await context.ApplicationUsers.FindAsync(id);
        }

        public async Task UpdateUser(ApplicationUser user)
        {
            context.ApplicationUsers.Update(user);
            await context.SaveChangesAsync();
        }
    }
}
