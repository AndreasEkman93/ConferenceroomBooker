using ConferenceroomBooker.Data;
using ConferenceroomBooker.Models;
using Microsoft.EntityFrameworkCore;

namespace ConferenceroomBooker.Services
{
    public class ApplicationUserService
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddUserAsync(ApplicationUser user)
        {
            await context.ApplicationUsers.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await context.ApplicationUsers.FindAsync(id);
            if (user != null)
            {
                context.ApplicationUsers.Remove(user);
                await context.SaveChangesAsync();
            }

        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await context.ApplicationUsers.ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserAsync(int id)
        {
            return await context.ApplicationUsers.FindAsync(id);
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            context.ApplicationUsers.Update(user);
            await context.SaveChangesAsync();
        }
    }
}
