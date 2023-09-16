using Microsoft.AspNetCore.Mvc;
using OZone.Api.Domain;
using OZone.Api.Domain.Models;
using OZone.Api.Models;

namespace OZone.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ILogger<SubscriptionsController> _logger;
    private readonly EventContext _db;

    public SubscriptionsController(ILogger<SubscriptionsController> logger, EventContext db)
    {
        _logger = logger;
        _db = db;
    }
    
    [HttpGet]
    public IEnumerable<Subscription> Get()
    {
        return _db.Subscriptions.ToList();
    }
    
    /// <summary>
    /// Subscribe a user to an event
    /// </summary>
    /// <param name="subscription">Subscription details</param>
    /// <returns>Created subscription</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public CreateSubscriptionResponse Subscribe(Subscription subscription)
    {
        var eventT = _db.Subscriptions.Add(subscription);
        _db.SaveChanges();

        return new CreateSubscriptionResponse
        {
            Id = eventT.Entity.Id,
            EventId = eventT.Entity.EventId,
            UserId = eventT.Entity.UserId
        };
    }
}