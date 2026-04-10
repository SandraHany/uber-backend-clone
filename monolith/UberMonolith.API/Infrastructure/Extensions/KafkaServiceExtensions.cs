using Confluent.Kafka;

namespace UberMonolith.API;

public static class KafkaServiceExtensions
{
    public static IServiceCollection AddKafkaProducer(this IServiceCollection services , IConfiguration config)
    {
        var kafkaConfig = config.GetSection("Kafka");
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = kafkaConfig["BootstrapServers"],
            Acks = Acks.All
        };
        services.AddSingleton(producerConfig);
        return services;
    }
    public static IServiceCollection AddKafkaConsumer(this IServiceCollection services, IConfiguration config)
    {
        var kafkaConfig = config.GetSection("Kafka");
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaConfig["BootstrapServers"],
            GroupId = kafkaConfig["GroupId"],
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        services.AddSingleton(consumerConfig);
        return services;
    }
}
