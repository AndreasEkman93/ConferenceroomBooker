using System;
using System.Collections.Generic;
using System.Text;
using ConferenceroomBooker.Data;
using ConferenceroomBooker.Services;
using Microsoft.EntityFrameworkCore;

namespace ConferenceroomBooker.test
{
    public class ConferenceRoomTests
    {

        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        private ApplicationDbContext context;
        private ConferenceRoomService conferenceRoomService;
        public ConferenceRoomTests()
        {
            context = new ApplicationDbContext(dbContextOptions);
            context.Database.EnsureCreated();
            conferenceRoomService = new ConferenceRoomService(context);
        }
    }
}
