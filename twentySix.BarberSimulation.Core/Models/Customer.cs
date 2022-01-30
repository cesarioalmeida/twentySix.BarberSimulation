namespace twentySix.BarberSimulation.Core.Models
{
    public class Customer
    {
        private static int _id;

        public Customer() => Id = ++_id;

        public int Id { get; }

        public double ArrivalTime { get; set; }

        public double ServiceTime { get; set; }

        public double ToleranceTime { get; set; }
    }
}