namespace SuperLibrary.RabbitMQ.Abstractions;

public interface IRabbitMqPublisher<TMessage> where TMessage : class
{
    public Task PublishAsync(TMessage message, CancellationToken cancellationToken);
}