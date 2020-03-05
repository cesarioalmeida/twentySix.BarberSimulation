namespace twentySix.BarberSimulation.Core.Models
{
    using System;
    using System.Linq;

    using twentySix.BarberSimulation.Core.Enums;
    using twentySix.BarberSimulation.Core.Events;
    using twentySix.BarberSimulation.Core.Providers;

    public class Barber
    {
        private static int id;

        public Barber()
        {
            this.Id = ++id;

            EventBusProvider.Instance.EventBus.Subscribe<TimeChangedEvent>(OnTimeChanged);
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

        private void OnTimeChanged(TimeChangedEvent obj)
        {
            if (this.Status == WorkingStatus.Idle && CustomerQueueProvider.Instance.Queue.Any())
            {
                this.StartWithCustomer(obj.Time, CustomerQueueProvider.Instance.Queue.Dequeue());
                Console.WriteLine($"{obj.Time:f2}: Barber ({this.Id}) starting to serve customer ({this.ServingCustomer.Id})");
            }

            if (this.Status == WorkingStatus.Busy
                && obj.Time - this.ServiceStart > this.ServingCustomer.ServiceTime)
            {
                EventBusProvider.Instance.EventBus.Publish(CustomerLeftEvent.Raise(obj.Time, this.ServingCustomer));

                this.Free();
            }
        }
    }
}