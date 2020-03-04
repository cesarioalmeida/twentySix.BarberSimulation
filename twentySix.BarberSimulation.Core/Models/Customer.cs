namespace twentySix.BarberSimulation.Core.Models
{
    public class Customer
    {
        private static int id;

        public Customer()
        {
            this.Id = ++id;
        }

        public int Id { get; }

        public double ArrivalTime { get; set; }

        public double ServiceTime { get; set; }
    }
}