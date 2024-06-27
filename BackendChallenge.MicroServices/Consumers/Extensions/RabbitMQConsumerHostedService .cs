namespace BackendChallenge.MicroServices.Consumers.Extensions
{
    public class RabbitMQConsumerHostedService : IHostedService
    {
        private readonly RabbitMQConsumer _consumer;
        private Task _executingTask;
        private CancellationTokenSource _cts;

        public RabbitMQConsumerHostedService(RabbitMQConsumer consumer)
        {
            _consumer = consumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _executingTask = _consumer.StartConsumingAsync(_cts.Token);
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                return;
            }
            try
            {
                _cts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }
    }


}
