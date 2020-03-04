namespace twentySix.BarberSimulation.Core.Models
{
    using twentySix.BarberSimulation.Core.Enums;

    public class Barber
    {
        private static int id;

        public Barber()
        {
            this.Id = ++id;
        }

        public int Id { get; }

        public WorkingStatus Status { get; private set; }

        public double ServiceStart { get; private set; }

        public Customer ServingCustomer { get; private set; }

        public void StartWithCustomer(double time, Customer customer)
        {
            this.Status = WorkingStatus.Busy;
            this.ServiceStart = time;
            this.ServingCustomer = customer;
        }

        public void Free()
        {
            this.Status = WorkingStatus.Idle;
            this.ServiceStart = 0d;
            this.ServingCustomer = null;
        }
    }
}