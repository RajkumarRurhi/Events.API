using Events.API.Entities;
using Events.API.Helpers;
using Events.API.Models;
using Events.API.ResourceParameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Events.API.Data
{
    public class EventRepo : IEventRepo
    {
        private readonly EventsDBContext dbContext;
        private readonly ILogger<EventRepo> logger;
        private readonly IPropertyMapper propertyMapper;

        public EventRepo(EventsDBContext DBContext, ILogger<EventRepo> Logger, IPropertyMapper propertyMapper)
        {
            dbContext = DBContext;
            logger = Logger;
            this.propertyMapper = propertyMapper;
        }

        public void Add<T>(T entity) where T : class
        {
            dbContext.Add(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await dbContext.SaveChangesAsync()) > 0;
        }

        public void Delete<T>(T entity) where T : class
        {
            dbContext.Remove(entity);
        }

        public async Task<bool> EventExists(Guid eventId)
        {
            return await dbContext.Events.AsNoTracking().AnyAsync(e => e.Id == eventId);
        }

        public async Task<Event> GetEventAsync(Guid id, bool includeGuest = false)
        {
            var query = dbContext.Events.AsQueryable();
            if (includeGuest)
            {
                query = query.Include(e => e.Guests).ThenInclude(g => g.Person);
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event> GetEventAsyncTracked(Guid id)
        {
            return await dbContext.Events.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<PagedList<Event>> GetEventsAsync(EventResourceParameters eventResourceParameters)
        {
            if(eventResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(eventResourceParameters));
            }
            var lstEvent = dbContext.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(eventResourceParameters.Type))
            {
                string eventType = eventResourceParameters.Type.Trim();
                lstEvent = lstEvent.Where(e => e.EventType.Equals(eventType));
            }

            if (!string.IsNullOrWhiteSpace(eventResourceParameters.SearchQuery))
            {
                string searchQuery = eventResourceParameters.SearchQuery.Trim();
                lstEvent = lstEvent.Where(e => e.Title.Contains(searchQuery) || e.Description.Contains(searchQuery));
            }

            if (!string.IsNullOrWhiteSpace(eventResourceParameters.OrderBy))
            {
                var eventMappingDictionary = propertyMapper.GetPropertyMapping<EventModel, Event>();
                lstEvent = lstEvent.ApplySort(eventResourceParameters.OrderBy, eventMappingDictionary);
            }

            return await PagedList<Event>.Create(lstEvent.AsNoTracking(), eventResourceParameters.PageNumber, eventResourceParameters.PageSize);

            //return await lstEvent.Skip((eventResourceParameters.PageNumber - 1) * eventResourceParameters.PageSize)
            //    .Take(eventResourceParameters.PageSize).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(IEnumerable<Guid> eventIds)
        {
            if(eventIds == null)
            {
                throw new ArgumentNullException(nameof(eventIds));
            }

            return await dbContext.Events.Where(e => eventIds.Contains(e.Id)).Include(e => e.Guests).ThenInclude(g => g.Person).AsNoTracking().ToListAsync();
        }

        public void CreateEvent(Event evnt)
        {
            dbContext.Events.Add(evnt);
        }



        public async Task<PagedList<Person>> GetGuestsAsync(Guid eventId, GuestsResourceParameters guestsResourceParameters)
        {
            if (guestsResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(guestsResourceParameters));
            }
            var people = dbContext.People.AsQueryable();

            if (!string.IsNullOrWhiteSpace(guestsResourceParameters.SearchQuery))
            {
                string searchQuery = guestsResourceParameters.SearchQuery.Trim();
                people = people.Where(p => p.FirstName.Contains(searchQuery) || p.LastName.Contains(searchQuery) || p.Email.Contains(searchQuery));
            }

            if (!string.IsNullOrWhiteSpace(guestsResourceParameters.OrderBy))
            {
                var guestMappingDictionary = propertyMapper.GetPropertyMapping<GuestModel, Person>();
                people = people.ApplySort(guestsResourceParameters.OrderBy, guestMappingDictionary);
            }

            return await PagedList<Person>.Create(people.AsNoTracking().Where(p => p.EventsToAttend.Any(pe => pe.EventId == eventId)), guestsResourceParameters.PageNumber, guestsResourceParameters.PageSize);

            //return await dbContext.People.AsNoTracking().Where(p => p.EventsToAttend.Any(pe => pe.EventId == eventId)).ToListAsync();
        }

        public async Task<PersonEvent> GetGuestAsync(Guid eventId, string personEmail)
        {
            return await dbContext.PersonEvents.Include(pe => pe.Person).AsNoTracking().FirstOrDefaultAsync(pe => personEmail == pe.PersonEmail && pe.EventId == eventId);
            
        }

        public async Task<PersonEvent> GetGuestAsyncTracked(Guid eventId, string personEmail)
        {
            return await dbContext.PersonEvents.Include(pe => pe.Person).FirstOrDefaultAsync(pe => personEmail == pe.PersonEmail && pe.EventId == eventId);
            
        }

        public async Task<Person> GetPersonAsyncTracked(string personEmail)
        {
            return await dbContext.People.FirstOrDefaultAsync(p => personEmail == p.Email);
        }

    }
}
