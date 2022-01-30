namespace twentySix.BarberSimulation.Core.Events
{
    using Redbus.Events;

    using Models;

    public class CustomerLeftEvent : EventBase
    {
        protected CustomerLeftEvent(double time, Customer customer)
        {
            DepartureTime = time;
            Customer = customer;
        }

        public double DepartureTime { get; }

        public Customer Customer { get; }

        public static CustomerLeftEvent Raise(double time, Customer customer) => new(time, customer);
    }
}