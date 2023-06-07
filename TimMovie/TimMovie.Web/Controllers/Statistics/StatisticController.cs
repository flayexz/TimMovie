using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TimMovie.Core.Events;
using TimMovie.Infrastructure.Settings;

namespace TimMovie.Web.Controllers.Statistics;


public class StatisticController : Controller
{
    private readonly IProducer<Null, RegisterFilmTrafficEvent> _producer;
    private readonly KafkaSettings _settings;

    public StatisticController(IProducer<Null, RegisterFilmTrafficEvent> producer, IOptions<KafkaSettings> settings)
    {
        _producer = producer;
        _settings = settings.Value;
    }

    [HttpPost]
    public async Task<IActionResult> IncreaseFilmTrafficAsync([FromQuery] Guid filmId)
    {
        await _producer.ProduceAsync(_settings.Topic, new Message<Null, RegisterFilmTrafficEvent>
        {
            Value = new RegisterFilmTrafficEvent
            {
                Id = filmId
            }
        });

        return Ok();
    }
}