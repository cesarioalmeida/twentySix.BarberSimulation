using System;
using System.Linq;
using twentySix.BarberSimulation.Core.Enums;
using twentySix.BarberSimulation.Core.Events;
using twentySix.BarberSimulation.Core.Providers;

namespace twentySix.BarberSimulation.Core.Models
{
    public class Barber
    {
        private static int id;

        public Barber()
        {
            Id = ++id;

            EventBusProvider.Instance.EventBus.Subscribe<TimeChangedEvent>(OnTimeChanged);
        }

        public int Id { get; }

        public WorkingStatus Status { get; private set; }

        public double ServiceStart { get; private set; }

        public Customer ServingCustomer { get; private set; }

        public void StartWithCustomer(double time, Customer customer)
        {
            Status = WorkingStatus.Busy;
            ServiceStart = time;
            ServingCustomer = customer;
        }

        public void Free()
        {
            Status = WorkingStatus.Idle;
            ServiceStart = 0d;
            ServingCustomer = null;
        }

        private void OnTimeChanged(TimeChangedEvent obj)
        {
            if (Status == WorkingStatus.Idle && CustomerQueueProvider.Instance.Queue.Any())
            {
                StartWithCustomer(obj.Time, CustomerQueueProvider.Instance.Queue.Dequeue());
                Console.WriteLine($"{obj.Time:f2}: Barber ({Id}) starting to serve customer ({ServingCustomer.Id})");
            }

            if (Status == WorkingStatus.Busy
                && obj.Time - ServiceStart > ServingCustomer.ServiceTime)
            {
                EventBusProvider.Instance.EventBus.Publish(CustomerLeftEvent.Raise(obj.Time, ServingCustomer));

                Free();
            }
        }
    }
}