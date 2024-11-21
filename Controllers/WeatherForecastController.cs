using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly RabbitMQConnection _rabbitMQConnection;
    private readonly RabbitMQSubscriber _rabbitMQSubscriber;

    public WeatherForecastController()
    {
        _rabbitMQConnection = new RabbitMQConnection();
        _rabbitMQSubscriber = new RabbitMQSubscriber(); // Subscriber instance
    }

    [HttpPost("publish")]
    public async Task<IActionResult> PublishMessage()
    {
        var message = "Test weather message";
        await _rabbitMQConnection.PublishMessageAsync("weatherQueue", message);
        return Ok("Message published to RabbitMQ.");
    }

    [HttpGet("subscribe")]
    public async Task<IActionResult> SubscribeToQueue()
    {
        // Note: This will block the endpoint to wait for messages; consider using background services for long-running tasks.
        _ = Task.Run(() => _rabbitMQSubscriber.SubscribeToQueueAsync("weatherQueue")); // Run subscriber as a separate task
        return Ok("Subscriber started. Listening for messages...");
    }

    [HttpPost("a")]
    public async Task<IActionResult> aaa(dto dt)
    {
        return Ok("Subscriber started. Listening for messages...");
    }
}
public class dto
{
    public int Id { get; set; }
    public string? Name { get; set; }
   // public IFormFile? File { get; set; }
}
