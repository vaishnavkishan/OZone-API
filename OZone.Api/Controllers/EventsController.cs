using Microsoft.AspNetCore.Mvc;
using OZone.Api.Domain;
using OZone.Api.Domain.Models;
using OZone.Api.Services;

namespace OZone.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly IEventService _eventService;

    public EventsController(ILogger<EventsController> logger, IEventService eventService)
    {
        _logger = logger;
        _eventService = eventService;
    }

    /// <summary>
    /// Get events by kind
    /// </summary>
    /// <param name="kind">Kind of events to return. 'upcoming', 'past'.</param>
    /// <returns>Returns filtered events if kind is provided. Otherwise returns all events.</returns>
    [HttpGet]
    public async Task<IActionResult> Get(string? kind)
    {
        return Ok(await _eventService.Get(kind));
    }
    
    /// <summary>
    /// Get event by id
    /// </summary>
    /// <param name="id">Unique Guid of the event</param>
    /// <returns>Event details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _eventService.GetById(id));
    }

    /// <summary>
    /// Create an event
    /// </summary>
    /// <param name="createEvent">Event details</param>
    /// <returns>Newly created event</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(Event createEvent)
    {
        return Ok(await _eventService.Create(createEvent));
    }
}