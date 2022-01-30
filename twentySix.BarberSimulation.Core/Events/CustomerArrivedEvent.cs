namespace twentySix.BarberSimulation.Core.Events
{
    using Redbus.Events;

    public class CustomerArrivedEvent : EventBase
    {
        protected CustomerArrivedEvent(double time) => ArrivalTime = time;
        
        public double ArrivalTime { get; }

        public static CustomerArrivedEvent Raise(double time) => new(time);
    }
}