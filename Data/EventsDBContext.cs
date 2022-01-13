using Events.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Events.API.Data
{
    public class EventsDBContext : DbContext
    {
        public EventsDBContext(DbContextOptions<EventsDBContext> options) : base(options) { }
        public DbSet<Person> People { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<PersonEvent> PersonEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonEvent>().HasKey(pe => new { pe.PersonEmail, pe.EventId});

            modelBuilder.Entity<Person>().HasData(
                new Person
                {
                    Email = "RajRurhi@events.com",
                    FirstName = "Raj",
                    LastName = "Rurhi",
                    DateOfBirth = DateTime.Now.AddYears(-35)
                },
                new Person
                {
                    Email = "DavidJones@events.com",
                    FirstName = "David",
                    LastName = "Jones",
                    DateOfBirth = DateTime.Now.AddYears(-50)
                },
                new Person
                {
                    Email = "GeorgeJensen@events.com",
                    FirstName = "Georg",
                    LastName = "Jensen",
                    DateOfBirth = DateTime.Now.AddYears(-100)
                },
                new Person
                {
                    Email = "AmitKumar@events.com",
                    FirstName = "Amit",
                    LastName = "Kumar",
                    DateOfBirth = DateTime.Now.AddYears(-28)
                }
            );

            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = Guid.Parse("11CE940F-0997-463A-B850-6D5A866A07A7"),
                    Title = "Raj's birthday belebration",
                    Description = "Raj's birthday celebration description. Raj's birthday celebration description. Raj's birthday celebration description.",
                    EventType = "Indoor",
                    StartDateTime = DateTime.Now.AddDays(7),
                    EndDateTime = DateTime.Now.AddDays(7).AddHours(2)
                },
                new Event
                {
                    Id = Guid.Parse("21CE940F-0997-463A-B850-6D5A866A07A7"),
                    Title = "David's marriage anniversary celebration",
                    Description = "David's marriage anniversary celebration. David's marriage anniversary celebration. David's marriage anniversary celebration.",
                    EventType = "Outdoor",
                    StartDateTime = DateTime.Now.AddDays(17),
                    EndDateTime = DateTime.Now.AddDays(17).AddHours(4)
                },
                new Event
                {
                    Id = Guid.Parse("31CE940F-0997-463A-B850-6D5A866A07A7"),
                    Title = "Georg's work anniversary celebration",
                    Description = "Georg's work anniversary celebration. Georg's work anniversary celebration. Georg's work anniversary celebration.",
                    EventType = "Indoor",
                    StartDateTime = DateTime.Now.AddDays(10),
                    EndDateTime = DateTime.Now.AddDays(10).AddHours(3)
                }
            );

            modelBuilder.Entity<PersonEvent>().HasData(
                new PersonEvent
                {
                    EventId = Guid.Parse("11CE940F-0997-463A-B850-6D5A866A07A7"),
                    PersonEmail = "RajRurhi@events.com"
                },
                new PersonEvent
                {
                    EventId = Guid.Parse("11CE940F-0997-463A-B850-6D5A866A07A7"),
                    PersonEmail = "DavidJones@events.com"
                },
                new PersonEvent
                {
                    EventId = Guid.Parse("11CE940F-0997-463A-B850-6D5A866A07A7"),
                    PersonEmail = "GeorgeJensen@events.com"
                },
                new PersonEvent
                {
                    EventId = Guid.Parse("21CE940F-0997-463A-B850-6D5A866A07A7"),
                    PersonEmail = "DavidJones@events.com"
                },
                new PersonEvent
                {
                    EventId = Guid.Parse("21CE940F-0997-463A-B850-6D5A866A07A7"),
                    PersonEmail = "GeorgeJensen@events.com"
                },
                new PersonEvent
                {
                    EventId = Guid.Parse("21CE940F-0997-463A-B850-6D5A866A07A7"),
                    PersonEmail = "AmitKumar@events.com"
                },
                new PersonEvent
                {
                    EventId = Guid.Parse("31CE940F-0997-463A-B850-6D5A866A07A7"),
                    PersonEmail = "GeorgeJensen@events.com"
                },
                new PersonEvent
                {
                    EventId = Guid.Parse("31CE940F-0997-463A-B850-6D5A866A07A7"),
                    PersonEmail = "AmitKumar@events.com"
                },
                new PersonEvent
                {
                    EventId = Guid.Parse("31CE940F-0997-463A-B850-6D5A866A07A7"),
                    PersonEmail = "RajRurhi@events.com"
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
