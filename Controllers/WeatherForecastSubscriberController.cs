using Microsoft.AspNetCore.Mvc;

namespace ToDoApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastSubscriberController : ControllerBase
    {
        public static async Task Main(string[] args)
        {
            var subscriber = new RabbitMQSubscriber();
            await subscriber.SubscribeToQueueAsync("weatherQueue");
        }
    }
}
