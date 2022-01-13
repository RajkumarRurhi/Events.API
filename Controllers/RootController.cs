using Events.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Events.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class RootController : ControllerBase
    {
        [HttpGet(Name ="GetRoot")]
        public IActionResult GetRoot()
        {
            var lstLink = new List<LinkDto>();

            lstLink.Add(new LinkDto(Url.Link("GetRoot", new { }), "self", "GET"));
            lstLink.Add(new LinkDto(Url.Link("GetEvents", new { }), "events", "GET"));
            lstLink.Add(new LinkDto(Url.Link("CreateEvent", new { }), "create_event", "POST"));

            return Ok(lstLink);
        }
    }
}
