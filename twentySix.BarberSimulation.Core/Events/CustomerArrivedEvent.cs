namespace twentySix.BarberSimulation.Core.Events
{
    using Redbus.Events;

    public class CustomerArrivedEvent : EventBase
    {
        protected CustomerArrivedEvent(double time)
        {
            this.ArrivalTime = time;
        }

        public double ArrivalTime { get; }

        public static CustomerArrivedEvent Raise(double time)
        {
            return new CustomerArrivedEvent(time);
        }
    }
}