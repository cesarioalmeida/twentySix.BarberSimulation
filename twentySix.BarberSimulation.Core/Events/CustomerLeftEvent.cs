namespace twentySix.BarberSimulation.Core.Events
{
    using Redbus.Events;

    using twentySix.BarberSimulation.Core.Models;

    public class CustomerLeftEvent : EventBase
    {
        protected CustomerLeftEvent(double time, Customer customer)
        {
            this.DepartureTime = time;
            this.Customer = customer;
        }

        public double DepartureTime { get; }

        public Customer Customer { get; }

        public static CustomerLeftEvent Raise(double time, Customer customer)
        {
            return new CustomerLeftEvent(time, customer);
        }
    }
}