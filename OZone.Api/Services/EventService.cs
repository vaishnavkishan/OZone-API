using Microsoft.EntityFrameworkCore;
using OZone.Api.Domain;
using OZone.Api.Domain.Models;

namespace OZone.Api.Services;

public interface IEventService
{
    Task<IEnumerable<Event>> Get(string? kind);
    Task<Event> GetById(Guid id);
    Task<Event> Create(Event createEvent);
}

public class EventService : IEventService
{
    private readonly ILogger<EventService> _logger;
    private readonly EventContext _db;
    private readonly IEmailSender _emailSender;

    public EventService(ILogger<EventService> logger, EventContext db, IEmailSender emailSender)
    {
        _logger = logger;
        _db = db;
        _emailSender = emailSender;
    }

    public async Task<IEnumerable<Event>> Get(string? kind)
    {
        if (kind == "upcoming")
            return await _db.Events.Where(x => x.Date.CompareTo(DateTime.UtcNow) > 0).ToListAsync();

        if (kind == "past")
            return await _db.Events.Where(x => x.Date.CompareTo(DateTime.UtcNow) < 0).ToListAsync();

        return _db.Events.ToList();
    }

    public async Task<Event?> GetById(Guid id)
    {
        return await _db.Events.FindAsync(id);
    }

    public async Task<Event> Create(Event createEvent)
    {
        var eventT = _db.Events.Add(createEvent);
        _db.SaveChanges();

        await SendNotifications(createEvent);
        return eventT.Entity;
    }

    private async Task SendNotifications(Event createEvent)
    {
        string body = "New event details:"; //TODO: Add event details
        string subject = $"A new event '{createEvent.Name}' is registered.";

        try
        {
            await _emailSender.Send(createEvent.PersonOfContact, subject, body);
        }
        catch (ApplicationException ex)
        {
            _logger.LogError(ex, "Could not send email notification!");
        }
    }
}