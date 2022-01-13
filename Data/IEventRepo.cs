using Events.API.Entities;
using Events.API.Helpers;
using Events.API.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Events.API.Data
{
    public interface IEventRepo
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        Task<bool> EventExists(Guid eventId);
        Task<PagedList<Event>> GetEventsAsync(EventResourceParameters eventResourceParameters);
        Task<IEnumerable<Event>> GetEventsAsync(IEnumerable<Guid> eventIds);
        Task<Event> GetEventAsync(Guid eventId, bool includeGuest = false);
        Task<Event> GetEventAsyncTracked(Guid eventId);
        void CreateEvent(Event evnt);

        Task<PagedList<Person>> GetGuestsAsync(Guid eventId, GuestsResourceParameters guestsResourceParameters);
        Task<PersonEvent> GetGuestAsync(Guid eventId, string personEmail);
        Task<PersonEvent> GetGuestAsyncTracked(Guid eventId, string personEmail);

        Task<Person> GetPersonAsyncTracked(string personEmail);
    }
}
