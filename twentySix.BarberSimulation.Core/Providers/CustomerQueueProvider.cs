namespace twentySix.BarberSimulation.Core.Providers
{
    using twentySix.BarberSimulation.Core.Framework;
    using twentySix.BarberSimulation.Core.Models;

    public class CustomerQueueProvider
    {
        private static CustomerQueueProvider instance;

        private CustomerQueueProvider()
        {
        }

        public static CustomerQueueProvider Instance => instance ?? (instance = new CustomerQueueProvider());

        public ExtendedQueue<Customer> Queue { get; } = new ExtendedQueue<Customer>();
    }
}