using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<IActionResult> Subscribe(CreateSubscriptionRequest req)
    {
        var user =await _db.Users.Where(x => x.Email == req.Email).FirstOrDefaultAsync();

        if (user == null)
        {
           var entity= await _db.Users.AddAsync(new User { Name = req.Name, Email = req.Email });
           user = entity.Entity;
        }

        if (await _db.Subscriptions.AnyAsync(x => x.UserId == user.Id && x.EventId == req.EventId))
            return BadRequest("Already subscribed to the event!");

        var sub = new Subscription
        {
            UserId = user.Id,
            EventId = req.EventId
        };
        
        var eventT = await _db.Subscriptions.AddAsync(sub);
        _db.SaveChanges();

        return Ok(new CreateSubscriptionResponse
        {
            Id = eventT.Entity.Id,
            EventId = eventT.Entity.EventId,
            UserId = eventT.Entity.UserId
        });
    }
}