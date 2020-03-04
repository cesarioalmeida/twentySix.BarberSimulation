namespace twentySix.BarberSimulation.Core.Events
{
    using Redbus.Events;

    public class TimeChangedEvent : EventBase
    {
        protected TimeChangedEvent(double time)
        {
            this.Time = time;
        }

        public double Time { get; }

        public static TimeChangedEvent Raise(double time)
        {
            return new TimeChangedEvent(time);
        }
    }
}