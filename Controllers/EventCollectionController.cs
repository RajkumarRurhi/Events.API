using AutoMapper;
using Events.API.Data;
using Events.API.Entities;
using Events.API.Helpers;
using Events.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Events.API.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class EventCollectionController : ControllerBase
    {
        private readonly IEventRepo eventRepo;
        private readonly ILogger<EventCollectionController> logger;
        private readonly IMapper mapper;

        public EventCollectionController(IEventRepo eventRepo, ILogger<EventCollectionController> logger, IMapper mapper)
        {
            this.eventRepo = eventRepo;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet("({ids})", Name ="GetEventCollection")]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetEventCollection(
            [FromRoute] 
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if(ids == null)
            {
                return BadRequest();
            }

            var eventCollection = await eventRepo.GetEventsAsync(ids);

            if(eventCollection.Count() != ids.Count())
            {
                return NotFound();
            }

            var eventCollectionModel = mapper.Map<IEnumerable<EventModel>>(eventCollection);

            return Ok(eventCollectionModel);
        }

        public async Task<ActionResult<IEnumerable<EventModel>>> CreateEventCollection(IEnumerable<EventForCreationModel> eventForCreationModels)
        {
            var eventsCollection = mapper.Map<IEnumerable<Event>>(eventForCreationModels);
            foreach(var evnt in eventsCollection)
            {
                eventRepo.Add(evnt);
            }
            await eventRepo.SaveChangesAsync();

            var ids = eventsCollection.Select(e => e.Id);
            eventsCollection = await eventRepo.GetEventsAsync(ids);
            var eventModelCollection = mapper.Map<IEnumerable<EventModel>>(eventsCollection);
            string stringIds = string.Join(",", ids);
            return CreatedAtRoute("GetEventCollection", new { ids = stringIds }, eventModelCollection); 
        }
    }
}
