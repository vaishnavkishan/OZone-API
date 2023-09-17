using System.Text;
using OZone.Api.Domain;
using OZone.Api.Domain.Models;
using OZone.Api.Integrations;

namespace OZone.Api.Services;

public interface IEventNotificationService
{
    Task SendSubscriptionNotifications(Event createEvent, string to);
    Task SendEventNotifications(Event createEvent, string to);
}

public class EventNotificationService : IEventNotificationService
{
    private readonly ILogger<EventNotificationService> _logger;
    private readonly IEmailSender _emailSender;

    public EventNotificationService(ILogger<EventNotificationService> logger, IEmailSender emailSender)
    {
        _logger = logger;
        _emailSender = emailSender;
    }

    public async Task SendSubscriptionNotifications(Event createEvent, string to)
    {
        try
        {
            var subject = $"Subscription to '{createEvent.Name}' event is successful";
            await _emailSender.Send(to, subject, CreateEventTemplate(createEvent));
        }
        catch (ApplicationException ex)
        {
            _logger.LogError(ex, "Could not send email notification!");
        }
    }

    public async Task SendEventNotifications(Event createEvent, string to)
    {
        try
        {
            var subject = $"A new event '{createEvent.Name}' is registered";
            await _emailSender.Send(to, subject, CreateEventTemplate(createEvent));
        }
        catch (ApplicationException ex)
        {
            _logger.LogError(ex, "Could not send email notification!");
        }
    }
    
    public string CreateEventTemplate(Event createEvent)
    {
        StringBuilder body = new StringBuilder();
        
        body.AppendLine("<h3>Event details:</h3>");
        body.AppendLine($"<br/><span><b>Name</b>:{createEvent.Name}</span>");
        body.AppendLine($"<br/><span><b>Date</b>:{createEvent.Date}</span>");
        body.AppendLine($"<br/><span><b>Speakers</b>:{createEvent.Speakers}</span>");
        body.AppendLine($"<br/><span><b>Mode</b>:{createEvent.Mode.ToString()}</span>");
        body.AppendLine($"<br/><span><b>Mode Details</b>:{createEvent.ModelDetails}</span>");
        body.AppendLine($"<br/><span><b>Topics</b>:{createEvent.Topic}</span>");
        body.AppendLine($"<br/><span><b>Details</b>:{createEvent.Details}</span>");
        body.AppendLine($"<br/><span><b>Person of contact</b>:{createEvent.PersonOfContact}</span>");
        
        body.AppendLine("<h4>More details:</h4>");
        body.AppendLine($"<br/><span></hr>Rules:{createEvent.Rules}</span>");
        body.AppendLine($"<br/><span></hr>Deadline:{createEvent.Deadline}</span>");
        body.AppendLine($"<br/><span></hr>Community:{createEvent.Community}</span>");
        body.AppendLine($"<br/><span></hr>Capacity:{createEvent.Capacity}</span>");
        body.AppendLine($"<br/><span></hr>Type:{createEvent.Type.ToString()}</span>");
        body.AppendLine($"<br/><span></hr>Tags:{createEvent.Tags}</span>");

        return body.ToString();
    }
}