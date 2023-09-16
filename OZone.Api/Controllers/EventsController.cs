using Microsoft.AspNetCore.Mvc;
using OZone.Api.Domain;
using OZone.Api.Domain.Models;

namespace OZone.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly EventContext _db;

    public EventsController(ILogger<EventsController> logger, EventContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet]
    public IEnumerable<Event> Get(string? kind)
    {
        if (kind == "upcoming")
            return _db.Events.Where(x => x.Date.CompareTo(DateTime.UtcNow) > 0).ToList();
        
        if (kind == "past")
            return _db.Events.Where(x => x.Date.CompareTo(DateTime.UtcNow) < 0).ToList();
        
        return _db.Events.ToList();
    }

    /// <summary>
    /// Create an event
    /// </summary>
    /// <param name="createEvent">Event details</param>
    /// <returns>Newly created event</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public Event Create(Event createEvent)
    {
        var eventT = _db.Events.Add(createEvent);
        _db.SaveChanges();
        return _db.Events.Where(x => x.Id == eventT.Entity.Id).FirstOrDefault();
    }
}