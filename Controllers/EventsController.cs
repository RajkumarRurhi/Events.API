using AutoMapper;
using Events.API.ActionConstraints;
using Events.API.Data;
using Events.API.Entities;
using Events.API.Helpers;
using Events.API.Models;
using Events.API.ResourceParameters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Events.API.Controllers
{
    [ApiController]
    [Route("api/events")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepo eventRepo;
        private readonly ILogger<EventsController> logger;
        private readonly IMapper mapper;
        private readonly IPropertyMapper propertyMapper;
        private readonly IPropertyChecker propertyChecker;

        public EventsController(IEventRepo eventRepo, ILogger<EventsController> logger, IMapper mapper, IPropertyMapper propertyMapper, IPropertyChecker propertyChecker)
        {
            this.eventRepo = eventRepo;
            this.logger = logger;
            this.mapper = mapper;
            this.propertyMapper = propertyMapper;
            this.propertyChecker = propertyChecker;
        }

        [Produces("application/json",
            "application/vnd.raj.hateoas+json")]
        [HttpGet(Name = "GetEvents")]
        [HttpHead]
        public async Task<IActionResult> GetEvents([FromQuery] EventResourceParameters eventResourceParameters, [FromHeader(Name = "Accept")] string mediaType)
        {
            if(!propertyMapper.ValidatePropertyMapping<EventModel, Event>(eventResourceParameters.OrderBy) ||
                !propertyChecker.HasProperties<EventModel>(eventResourceParameters.Fields) ||
                !MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            var lstEventFromRepo = await eventRepo.GetEventsAsync(eventResourceParameters);

            var paginationMetadata = new
            {
                totalCount = lstEventFromRepo.TotalCount,
                pageSize = lstEventFromRepo.PageSize,
                currentPage = lstEventFromRepo.CurrentPage,
                totalPages = lstEventFromRepo.TotalPages
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = CreateLinksForEvents(eventResourceParameters, lstEventFromRepo.HasNext, lstEventFromRepo.HasPrevious);

            var shapedEvents = mapper.Map<IEnumerable<EventModel>>(lstEventFromRepo).ShapeData(eventResourceParameters.Fields);

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
               .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            if (includeLinks)
            {
                var shapedEventsWithLinks = shapedEvents.Select(evnt =>
                {
                    var evntAsDict = evnt as IDictionary<string, object>;
                    var evntLinks = CreateLinksForEvent((Guid)evntAsDict["Id"], eventResourceParameters.Fields);
                    evntAsDict.Add("links", evntLinks);
                    return evntAsDict;
                });

                var ShapedEventsWithLinksToReturn = new
                {
                    values = shapedEventsWithLinks,
                    links
                };

                return Ok(ShapedEventsWithLinksToReturn);
            }

            return Ok(shapedEvents);
        }

        [Produces("application/json",
            "application/vnd.raj.hateoas+json")]
        [HttpGet("{eventId}", Name ="GetEvent")]
        [HttpHead("{eventId}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEvent(Guid eventId, string fields, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (eventId == Guid.Empty || !propertyChecker.HasProperties<EventModel>(fields) || 
                !MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            var eventFromRepo = await eventRepo.GetEventAsync(eventId);
            if(eventFromRepo == null)
            {
                return NotFound();
            }

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
               .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            var shapedEvent = mapper.Map<EventModel>(eventFromRepo).ShapeData(fields) as IDictionary<string, object>;
            if (includeLinks)
            {
                var links = CreateLinksForEvent(eventId, fields);
                shapedEvent.Add("links", links);
            }

            return Ok(shapedEvent);

        }

        [Produces("application/json",
            "application/vnd.raj.hateoas+json")]
        [HttpGet("{eventId}", Name = "GetEvent")]
        [HttpHead("{eventId}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetEventWithGuest(Guid eventId, string fields, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (eventId == Guid.Empty || !propertyChecker.HasProperties<EventModel>(fields) ||
                !MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            var eventFromRepo = await eventRepo.GetEventAsync(eventId, true);
            if (eventFromRepo == null)
            {
                return NotFound();
            }

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
               .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            var shapedEvent = mapper.Map<EventModel>(eventFromRepo).ShapeData(fields) as IDictionary<string, object>;
            if (includeLinks)
            {
                var links = CreateLinksForEvent(eventId, fields);
                shapedEvent.Add("links", links);
            }

            return Ok(shapedEvent);

        }


        [HttpPost(Name = "CreateEvent")]
        [RequestHeaderMatcher("Content-Type", "application/json", "application/vnd.raj.eventforcreation+json")]
        [Consumes("application/json", "application/vnd.raj.eventforcreation+json")]
        public async Task<ActionResult<EventModel>> CreateEvent(EventForCreationModel evnt)
        {
            if(evnt == null)
            {
                return BadRequest();
            }

            Event eventToCreate = mapper.Map<Event>(evnt);
            eventRepo.CreateEvent(eventToCreate);
            await eventRepo.SaveChangesAsync();

            Event eventFromRepo = await eventRepo.GetEventAsync(eventToCreate.Id, true);
            //var createdEventModel = mapper.Map<EventModel>(eventFromRepo);

            var links = CreateLinksForEvent(eventFromRepo.Id, null);
            var shapedDataWithLinks = mapper.Map<EventModel>(eventFromRepo).ShapeData(null) as IDictionary<string, object>;
            shapedDataWithLinks.Add("links", links);

            return CreatedAtRoute("GetEvent", new { eventId = shapedDataWithLinks["Id"] }, shapedDataWithLinks);
        }

        [HttpPost(Name = "CreateEventWithAddress")]
        [RequestHeaderMatcher("Content-Type", "application/vnd.raj.eventforcreationwithaddress+json")]
        [Consumes("application/vnd.raj.eventforcreationwithaddress+json")]
        public async Task<ActionResult<EventModel>> CreateEventWithAddress(EventForCreationDtoAddress evnt)
        {
            if (evnt == null)
            {
                return BadRequest();
            }

            Event eventToCreate = mapper.Map<Event>(evnt);
            eventRepo.CreateEvent(eventToCreate);
            await eventRepo.SaveChangesAsync();

            Event eventFromRepo = await eventRepo.GetEventAsync(eventToCreate.Id, true);
            //var createdEventModel = mapper.Map<EventModel>(eventFromRepo);

            var links = CreateLinksForEvent(eventFromRepo.Id, null);
            var shapedDataWithLinks = mapper.Map<EventModel>(eventFromRepo).ShapeData(null) as IDictionary<string, object>;
            shapedDataWithLinks.Add("links", links);

            return CreatedAtRoute("CreateEventWithAddress", new { eventId = shapedDataWithLinks["Id"] }, shapedDataWithLinks);
        }

        [HttpOptions]
        public IActionResult GetEventsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,PUT,PATCH,DELETE,HEAD");
            return Ok();
        }

        [HttpPut("{eventId}")]
        public async Task<ActionResult<EventModel>> UpdateEvent(Guid eventId, EventForUpdateModel eventForUpdateModel)
        {
            if (eventId == Guid.Empty)
            {
                return BadRequest();
            }

            var eventFromRepo = await eventRepo.GetEventAsyncTracked(eventId);

            if(eventFromRepo == null)
            {
                return NotFound();
            }

            mapper.Map(eventForUpdateModel, eventFromRepo);
            await eventRepo.SaveChangesAsync();

            return Ok(mapper.Map<EventModel>(eventFromRepo));
        }

        [HttpPatch("{eventId}")]
        public async Task<ActionResult> PartiallyUpdateEvent(Guid eventId, JsonPatchDocument<EventForUpdateModel> patchDocument)
        {
            if (eventId == Guid.Empty)
            {
                return BadRequest();
            }

            var eventFromRepo = await eventRepo.GetEventAsyncTracked(eventId);

            if (eventFromRepo == null)
            {
                return NotFound();
            }

            var eventToPatch = mapper.Map<EventForUpdateModel>(eventFromRepo);
            patchDocument.ApplyTo(eventToPatch, ModelState);

            if (!TryValidateModel(eventToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(eventToPatch, eventFromRepo);

            await eventRepo.SaveChangesAsync();

            return Ok(mapper.Map<EventModel>(eventFromRepo));
        }

        [HttpDelete("{eventId}", Name ="DeleteEvent")]
        public async Task<IActionResult> DeleteEvent(Guid eventId)
        {
            if (eventId == Guid.Empty)
            {
                return BadRequest();
            }

            var eventFromRepo = await eventRepo.GetEventAsyncTracked(eventId);

            if (eventFromRepo == null)
            {
                return NotFound();
            }

            eventRepo.Delete(eventFromRepo);
            await eventRepo.SaveChangesAsync();

            return NoContent();
        }

        private string GenerateEventResourceUri(EventResourceParameters eventResourceParameters, ResourceUriType resourceUriType)
        {
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetEvents",
                        new
                        {
                            fields = eventResourceParameters.Fields,
                            orderBy = eventResourceParameters.OrderBy,
                            pageNumber = eventResourceParameters.PageNumber - 1,
                            pageSize = eventResourceParameters.PageSize,
                            searchQuery = eventResourceParameters.SearchQuery,
                            type = eventResourceParameters.Type
                        });
                case ResourceUriType.NextPage:
                    return Url.Link("GetEvents",
                        new
                        {
                            fields = eventResourceParameters.Fields,
                            orderBy = eventResourceParameters.OrderBy,
                            pageNumber = eventResourceParameters.PageNumber + 1,
                            pageSize = eventResourceParameters.PageSize,
                            searchQuery = eventResourceParameters.SearchQuery,
                            type = eventResourceParameters.Type
                        });
                case ResourceUriType.Current:
                default:
                    return Url.Link("GetEvents",
                        new
                        {
                            fields = eventResourceParameters.Fields,
                            orderBy = eventResourceParameters.OrderBy,
                            pageNumber = eventResourceParameters.PageNumber,
                            pageSize = eventResourceParameters.PageSize,
                            searchQuery = eventResourceParameters.SearchQuery,
                            type = eventResourceParameters.Type
                        });
            }
        }
        
        private IEnumerable<LinkDto> CreateLinksForEvent(Guid eventId, string fields)
        {
            var lstLink = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                lstLink.Add(new LinkDto(Url.Link("GetEvent", new { eventId }), "self", "GET"));
            }
            else
            {
                lstLink.Add(new LinkDto(Url.Link("GetEvent", new { eventId, fields }), "self", "GET"));
            }

            lstLink.Add(new LinkDto(Url.Link("DeleteEvent", new {eventId}), "delete_event", "DELETE"));
            lstLink.Add(new LinkDto(Url.Link("GetGuests", new { eventId }), "event_guests", "GET"));
            lstLink.Add(new LinkDto(Url.Link("AddGuest", new { eventId }), "add_guest_to_event", "POST"));
            
            return lstLink;
        }

        private IEnumerable<LinkDto> CreateLinksForEvents(EventResourceParameters eventResourceParameters, bool hasNext, bool hasPrevious)
        {
            var lstLink = new List<LinkDto>();

            lstLink.Add(new LinkDto(GenerateEventResourceUri(eventResourceParameters, ResourceUriType.Current), "self", "GET"));

            if (hasNext)
            {
                lstLink.Add(new LinkDto(GenerateEventResourceUri(eventResourceParameters, ResourceUriType.NextPage), "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                lstLink.Add(new LinkDto(GenerateEventResourceUri(eventResourceParameters, ResourceUriType.PreviousPage), "previousPage", "GET"));
            }

            return lstLink;
        }
    }
}
