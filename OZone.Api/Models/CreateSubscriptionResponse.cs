namespace OZone.Api.Models;

public class CreateSubscriptionResponse
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
}

public class CreateSubscriptionRequest
{
    public Guid EventId { get; set; }
    public string Email { get; set; }= default!;
    public string Name { get; set; } = default!;
}