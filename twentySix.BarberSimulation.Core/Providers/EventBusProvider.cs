namespace twentySix.BarberSimulation.Core.Providers
{
    using Redbus;
    using Redbus.Interfaces;

    public class EventBusProvider
    {
        private static EventBusProvider instance;

        private EventBusProvider()
        {
        }

        public static EventBusProvider Instance => instance ?? (instance = new EventBusProvider());

        public IEventBus EventBus { get; } = new EventBus();
    }
}