namespace twentySix.BarberSimulation.Core.Providers
{
    using Framework;
    using Models;

    public class CustomerQueueProvider
    {
        private static CustomerQueueProvider _instance;

        private CustomerQueueProvider()
        {
        }

        public static CustomerQueueProvider Instance => _instance ??= new CustomerQueueProvider();

        public ExtendedQueue<Customer> Queue { get; } = new();
    }
}