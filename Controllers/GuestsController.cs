using AutoMapper;
using Events.API.Data;
using Events.API.Entities;
using Events.API.Helpers;
using Events.API.Models;
using Events.API.ResourceParameters;
using Marvin.Cache.Headers;
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
    [Route("api/events/{eventId}/guests")]
    //[ResponseCache(CacheProfileName = "60SecondsCacheProfile")]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public)]
    [HttpCacheValidation(MustRevalidate = true)]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class GuestsController : ControllerBase
    {
        private readonly IEventRepo eventRepo;
        private readonly ILogger<GuestsController> logger;
        private readonly IMapper mapper;
        private readonly IPropertyMapper propertyMapper;
        private readonly IPropertyChecker propertyChecker;

        public GuestsController(IEventRepo eventRepo, ILogger<GuestsController> logger, IMapper mapper, IPropertyMapper propertyMapper, IPropertyChecker propertyChecker)
        {
            this.eventRepo = eventRepo;
            this.logger = logger;
            this.mapper = mapper;
            this.propertyMapper = propertyMapper;
            this.propertyChecker = propertyChecker;
        }

        [Produces("application/json",
            "application/vnd.raj.hateoas+json",
            "application/vnd.raj.guests.full+json",
            "application/vnd.raj.guests.full.hateoas+json",
            "application/vnd.raj.guests.friendly+json",
            "application/vnd.raj.guests.friendly.hateoas+json")]
        [HttpGet(Name ="GetGuests")]
        [HttpHead]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 300)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetGuests(Guid eventId, [FromQuery] GuestsResourceParameters guestsResourceParameters, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (eventId == Guid.Empty || !propertyChecker.HasProperties<GuestModel>(guestsResourceParameters.Fields) ||
                !MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            if (!await eventRepo.EventExists(eventId))
            {
                return NotFound();
            }

            var lstGuest = await eventRepo.GetGuestsAsync(eventId, guestsResourceParameters);

            var paginationMetadata = new
            {
                totalCount = lstGuest.TotalCount,
                pageSize = lstGuest.PageSize,
                currentPage = lstGuest.CurrentPage,
                totalPages = lstGuest.TotalPages
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
              .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            IEnumerable<LinkDto> links = new List<LinkDto>();
            if (includeLinks)
            {
                links = CreateLinksForGuests(guestsResourceParameters, lstGuest.HasNext, lstGuest.HasPrevious);
            }

            var primaryMediaType = includeLinks ? parsedMediaType.SubTypeWithoutSuffix.Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8)
                : parsedMediaType.SubTypeWithoutSuffix;

            if (primaryMediaType == "vnd.raj.guests.full")
            {
                var fullGuests = mapper.Map<IEnumerable<GuestFullDto>>(lstGuest).ShapeData(guestsResourceParameters.Fields);
                
                if (includeLinks)
                {
                    var fullGuestsWithLinks = fullGuests.Select(guest =>
                    {
                        var guestAsDict = guest as IDictionary<string, object>;
                        var guestLinks = CreateLinksForGuest(eventId, guestAsDict["Email"].ToString(), guestsResourceParameters.Fields);
                        guestAsDict.Add("links", guestLinks);
                        return guestAsDict;
                    });

                    var fullGuestsWithLinksToReturn = new
                    {
                        values = fullGuestsWithLinks,
                        links = links
                    };

                    return Ok(fullGuestsWithLinksToReturn);
                }

                return Ok(fullGuests);
                
            }

            var shapedGuests = mapper.Map<IEnumerable<GuestModel>>(lstGuest).ShapeData(guestsResourceParameters.Fields);
            
            if (includeLinks)
            {
                var shapedGuestsWithLinks = shapedGuests.Select(guest =>
                {
                    var guestAsDict = guest as IDictionary<string, object>;
                    var guestLinks = CreateLinksForGuest(eventId, guestAsDict["Email"].ToString(), guestsResourceParameters.Fields);
                    guestAsDict.Add("links", guestLinks);
                    return guestAsDict;
                });

                var shapedGuestsWithLinksToReturn = new
                {
                    values = shapedGuestsWithLinks,
                    links = links
                };
                return Ok(shapedGuestsWithLinksToReturn);
            }

            return Ok(shapedGuests);
        }

        //[ResponseCache(Duration = 120)]
        [Produces("application/json",
            "application/vnd.raj.hateoas+json",
            "application/vnd.raj.guest.full+json",
            "application/vnd.raj.guest.full.hateoas+json",
            "application/vnd.raj.guest.friendly+json",
            "application/vnd.raj.guest.friendly.hateoas+json")]
        [HttpGet("{guestEmail}", Name ="GetGuest")]
        [HttpHead("{guestEmail}")]
        public async Task<IActionResult> GetGuest(Guid eventId, string guestEmail, [FromQuery] string fields, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (eventId == Guid.Empty || !propertyChecker.HasProperties<GuestModel>(fields) ||
                !MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            var guest = await eventRepo.GetGuestAsync(eventId, guestEmail);
            if(guest == null)
            {
                return NotFound();
            }
            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
               .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

            IEnumerable<LinkDto> links = new List<LinkDto>();
            if (includeLinks)
            {
                links = CreateLinksForGuest(eventId, guestEmail, fields);
            }

            var primaryMediaType = includeLinks ? parsedMediaType.SubTypeWithoutSuffix.Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8)
                : parsedMediaType.SubTypeWithoutSuffix;

            if (primaryMediaType == "vnd.raj.guest.full")
            {
                var fullGuestToReturn = mapper.Map<GuestFullDto>(guest).ShapeData(fields) as IDictionary<string, object>;
                if (includeLinks)
                {
                    fullGuestToReturn.Add("links", links);
                }

                return Ok(fullGuestToReturn);
            }

            var friendlyGuestsToReturn = mapper.Map<GuestModel>(guest).ShapeData<GuestModel>(fields) as IDictionary<string, object>;
            if (includeLinks)
            {
                friendlyGuestsToReturn.Add("links", links);
            }

            return Ok(friendlyGuestsToReturn);
        }

        [HttpOptions]
        public IActionResult GetGuestsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,PUT,PATCH,DELETE,HEAD");
            return Ok();
        }

        [HttpPost(Name = "AddGuest")]
        public async Task<ActionResult<GuestModel>> CreateGuest(Guid eventId, GuestForAddingModel guestForAdding)
        {
            if (eventId == Guid.Empty)
            {
                return BadRequest();
            }

            if (!await eventRepo.EventExists(eventId))
            {
                return NotFound();
            }

            PersonEvent guest = await eventRepo.GetGuestAsync(eventId, guestForAdding.Email);
            if (guest != null) //Guest alreay exists in this event. No need to add it.
            {
                return BadRequest();
            }
            Person person = await GetOrCreatePerson(guestForAdding);

            PersonEvent guestToAdd = mapper.Map<PersonEvent>(guestForAdding);
            guestToAdd.EventId = eventId;
            guestToAdd.Person = person;

            eventRepo.Add(guestToAdd);
            await eventRepo.SaveChangesAsync();

            return CreatedAtRoute("GetGuest", new { guestEmail = guestToAdd.PersonEmail, eventId = guestToAdd.EventId }, mapper.Map<GuestModel>(guestToAdd));
        }

        private async Task<Person> GetOrCreatePerson(GuestForManuplation guest)
        {
            Person person = await eventRepo.GetPersonAsyncTracked(guest.Email);
            if (person == null) //If person doesn't exists. Add him/her.
            {
                person = mapper.Map<Person>(guest);
                eventRepo.Add(person);
            }

            return person;
        }

        [HttpPut("{guestEmail}")]
        public async Task<ActionResult<GuestModel>> UpdateGuest(Guid eventId, string guestEmail, GuestForUpdateModel guestForUpdate)
        {
            if (eventId == Guid.Empty)
            {
                return BadRequest();
            }

            if (!await eventRepo.EventExists(eventId))
            {
                return NotFound();
            }

            PersonEvent guest = await eventRepo.GetGuestAsync(eventId, guestEmail);

            //upsert
            if (guest == null)
            {
                Person person = await GetOrCreatePerson(guestForUpdate);

                PersonEvent guestToAdd = mapper.Map<PersonEvent>(guestForUpdate);
                guestToAdd.EventId = eventId;
                guestToAdd.Person = person;

                eventRepo.Add(guestToAdd);

                await eventRepo.SaveChangesAsync();

                return CreatedAtRoute("GetGuest", new { guestEmail = guestToAdd.PersonEmail, eventId = guestToAdd.EventId }, mapper.Map<GuestModel>(guestToAdd));
            }

            guest = mapper.Map<PersonEvent>(guestForUpdate);
            guest.EventId = eventId;

            await eventRepo.SaveChangesAsync();

            return Ok(mapper.Map<GuestModel>(guest));
        }

        [HttpPatch("{guestEmail}")]
        public async Task<ActionResult<GuestModel>> PartiallyUpdateGuest(Guid eventId, string guestEmail, JsonPatchDocument<GuestForUpdateModel> patchDocument)
        {
            if (eventId == Guid.Empty)
            {
                return BadRequest();
            }

            if (!await eventRepo.EventExists(eventId))
            {
                return NotFound();
            }

            PersonEvent guest = await eventRepo.GetGuestAsyncTracked(eventId, guestEmail);

            //upsert
            if(guest == null)
            {
                GuestForUpdateModel guestForUpdateModel = new GuestForUpdateModel() { Email = guestEmail };
                Person person = await GetOrCreatePerson(guestForUpdateModel);

                mapper.Map(person, guestForUpdateModel);

                patchDocument.ApplyTo(guestForUpdateModel);

                PersonEvent guestToAdd = new PersonEvent();
                guestToAdd.Person = person;
                mapper.Map(guestForUpdateModel, guestToAdd);
                guestToAdd.EventId = eventId;

                eventRepo.Add(guestToAdd);

                await eventRepo.SaveChangesAsync();

                return CreatedAtRoute("GetGuest", new { guestEmail = guestToAdd.PersonEmail, eventId = guestToAdd.EventId }, mapper.Map<GuestModel>(guestToAdd));
            }

            var guestToPatch = mapper.Map<GuestForUpdateModel>(guest);
            patchDocument.ApplyTo(guestToPatch, ModelState);

            if (!TryValidateModel(guestToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(guestToPatch, guest);

            await eventRepo.SaveChangesAsync();

            return Ok(mapper.Map<GuestModel>(guest));
        }

        [HttpDelete("{guestEmail}", Name = "RemoveGuest")]
        public async Task<IActionResult> RemoveGuest(Guid eventId, string guestEmail)
        {
            if (eventId == Guid.Empty)
            {
                return BadRequest();
            }

            if (!await eventRepo.EventExists(eventId))
            {
                return NotFound();
            }

            var guest = await eventRepo.GetGuestAsyncTracked(eventId, guestEmail);
            if(guest == null)
            {
                return NotFound();
            }

            eventRepo.Delete(guest);
            await eventRepo.SaveChangesAsync();

            return NoContent();
        }

        private string GenerateGuestsResourceUri(GuestsResourceParameters guestsResourceParameters, ResourceUriType resourceUriType)
        {
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetGuests",
                        new
                        {
                            fields = guestsResourceParameters.Fields,
                            orderBy = guestsResourceParameters.OrderBy,
                            pageNumber = guestsResourceParameters.PageNumber - 1,
                            pageSize = guestsResourceParameters.PageSize,
                            searchQuery = guestsResourceParameters.SearchQuery
                        });
                case ResourceUriType.NextPage:
                    return Url.Link("GetGuests",
                        new
                        {
                            fields = guestsResourceParameters.Fields,
                            orderBy = guestsResourceParameters.OrderBy,
                            pageNumber = guestsResourceParameters.PageNumber + 1,
                            pageSize = guestsResourceParameters.PageSize,
                            searchQuery = guestsResourceParameters.SearchQuery
                        });
                case ResourceUriType.Current:
                default:
                    return Url.Link("GetGuests",
                        new
                        {
                            fields = guestsResourceParameters.Fields,
                            orderBy = guestsResourceParameters.OrderBy,
                            pageNumber = guestsResourceParameters.PageNumber,
                            pageSize = guestsResourceParameters.PageSize,
                            searchQuery = guestsResourceParameters.SearchQuery
                        });
            }
        }

        private IEnumerable<LinkDto> CreateLinksForGuest(Guid eventId, string guestEmail, string fields)
        {
            var lstLink = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                lstLink.Add(new LinkDto(Url.Link("GetGuest", new { eventId, guestEmail }), "self", "GET"));
            }
            else
            {
                lstLink.Add(new LinkDto(Url.Link("GetGuest", new { eventId, guestEmail, fields }), "self", "GET"));
            }

            lstLink.Add(new LinkDto(Url.Link("RemoveGuest", new { eventId, guestEmail }), "remove_guest_from_event", "DELETE"));

            return lstLink;
        }

        private IEnumerable<LinkDto> CreateLinksForGuests(GuestsResourceParameters guestResourceParameters, bool hasNext, bool hasPrevious)
        {
            var lstLink = new List<LinkDto>();

            lstLink.Add(new LinkDto(GenerateGuestsResourceUri(guestResourceParameters, ResourceUriType.Current), "self", "GET"));

            if (hasNext)
            {
                lstLink.Add(new LinkDto(GenerateGuestsResourceUri(guestResourceParameters, ResourceUriType.NextPage), "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                lstLink.Add(new LinkDto(GenerateGuestsResourceUri(guestResourceParameters, ResourceUriType.PreviousPage), "previousPage", "GET"));
            }

            return lstLink;
        }
    }
}
