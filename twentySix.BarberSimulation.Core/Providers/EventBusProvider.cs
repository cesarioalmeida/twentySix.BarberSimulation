namespace twentySix.BarberSimulation.Core.Providers
{
    using Redbus;
    using Redbus.Interfaces;

    public class EventBusProvider
    {
        private static EventBusProvider _instance;

        private EventBusProvider()
        {
        }

        public static EventBusProvider Instance => _instance ??= new EventBusProvider();

        public IEventBus EventBus { get; } = new EventBus();
    }
}