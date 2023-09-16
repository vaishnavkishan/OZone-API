using Microsoft.AspNetCore.Mvc;
using OZone.Api.Domain;
using OZone.Api.Domain.Models;
using OZone.Api.Integrations;
using OZone.Api.Models;
using OZone.Api.Services;

namespace OZone.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly IEventService _eventService;
    private readonly IOpenAiIntegration _openAi;

    public EventsController(ILogger<EventsController> logger, IEventService eventService, IOpenAiIntegration openAi)
    {
        _logger = logger;
        _eventService = eventService;
        _openAi = openAi;
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

    [HttpPost("suggest/topic")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Suggest(TopicSuggestionRequest req)
    {
        var prompt = @$"We are orgnaising an event for people from {req.Community} community, 
        please list some of the interesting topics on which the event can be organised for this community. 
        Output should be the numbered list with very brief description of the topic";

        var aiSuggestion = await _openAi.GetAiSuggestion(prompt);

        return Ok(new SuggestResponse { Suggestion = aiSuggestion });
    }

    [HttpPost("improve")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> ImproveText(TextSuggestionRequest req)
    {
        var prompt = @$"I am writing a description of an event for people from '{req.Community}' community.
        The name of the event is '{req.Name}'.
        The current description is '{req.Description}'.
        Please suggest better way of writing it, also suggest around 10 topics that can be covered.
        Output should be concise and clear and in below format: \
            improved_event_name:START'put suggested event name here'END!,improved_description:START'put suggested description here'END!,suggested_topics:START'put suggested topics to be covered here'END!";

        var suggestion = await _openAi.GetAiSuggestion(prompt);
//         var suggestion = @"
//
// improved_event_name: STARTUnlock the Power of Unit Testing in C#END!, 
// improved_description: STARTThis event will provide a comprehensive overview of the fundamentals and advanced functions of unit testing in C#. Participants will come away with a better understanding of its vital role in software development.END!,
// suggested_topics: STARTOverview of Unit Testing; Setting Up a Testing Environment; Writing Useful Tests; Customizing Tests; Mocking and Fakes; Debugging Tests; Performance Testing; Automated Testing; Overview of Unit Testing FrameworksEND!";
//         
        ImproveEventResponse res = new();
        try
        {
            var improved_event_name = suggestion.Split("improved_event_name")[1];
            improved_event_name = improved_event_name.Split("improved_description")[0];
            improved_event_name = RemoveTokens(improved_event_name);

            var improved_description = suggestion.Split("improved_description")[1];
            improved_description = improved_description.Split("suggested_topics")[0];
            improved_description = RemoveTokens(improved_description);

            var suggested_topics = suggestion.Split("suggested_topics")[1];
            suggested_topics = RemoveTokens(suggested_topics);

            suggestion = RemoveTokens(suggestion);

            res = new()
            {
                Name = improved_event_name,
                Description = improved_description,
                Topic = suggested_topics,
                FullResponse = suggestion
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing the suggestion.");
            res = new ImproveEventResponse { FullResponse = suggestion };
        }

        return Ok(res);
    }

    private string RemoveTokens(string input)
    {
        try
        {
            string[] tokens =
            {
                ": START", "START", "END!,", "END!", "improved_event_name", "improved_description", "suggested_topics"
            };
            foreach (var token in tokens)
            {
                if (input.Contains(token))
                    input = input.Replace(token, null);
            }

            return input;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while remove tokens from suggestion.");
            return input;
        }
    }
}