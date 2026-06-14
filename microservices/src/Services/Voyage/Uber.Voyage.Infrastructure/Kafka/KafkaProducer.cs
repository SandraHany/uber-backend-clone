using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Uber.Voyage.Application.Abstractions;

namespace Uber.Voyage.Infrastructure.Kafka;

internal sealed class KafkaProducer : IKafkaProducer

{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;

    public KafkaProducer(IConfiguration configuration, ILogger<KafkaProducer> logger)
    {
        _logger = logger;

        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            Acks = Acks.All,
            MessageSendMaxRetries = 3,
            RetryBackoffMs = 1000
            
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishAsync<T>(string topic, string key, T message, CancellationToken ct) where T : class
    {
        var payload = JsonSerializer.Serialize(message);
        var result = await _producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = payload },ct);
        _logger.LogInformation(
         "[KafkaProducer] Published {Type} → {Topic} [partition={P} offset={O}]",
         typeof(T).Name, topic, result.Partition.Value, result.Offset.Value);
    }
    public void Dispose() => _producer?.Dispose();
}


