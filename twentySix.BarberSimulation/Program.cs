namespace twentySix.BarberSimulation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;

    using Redbus;
    using Redbus.Events;
    using Redbus.Interfaces;

    using twentySix.BarberSimulation.Core.Enums;
    using twentySix.BarberSimulation.Core.Events;
    using twentySix.BarberSimulation.Core.Models;

    public class Program
    {
        private static readonly IEventBus EventBus = new EventBus();

        private static readonly double DeltaTime = 0.1d;

        private static readonly List<Barber> Barbers = new List<Barber>();

        private static readonly Queue<Customer> CustomerQueue = new Queue<Customer>();

        private static readonly Random RandomGenerator = new Random();

        public static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("--- Barber Simulations ---");

            Console.WriteLine("(Press ESC to stop)\n");

            Initialize();

            var time = 0d;

            var simulationTimer = new Timer { AutoReset = true, Interval = 1000 };

            simulationTimer.Elapsed += (sender, eventArgs) =>
                {
                    time += DeltaTime;
                    EventBus.Publish(new PayloadEvent<double>(time));

                    Update(time);
                };

            simulationTimer.Start();

            Console.ReadKey();
        }

        private static void Update(double time)
        {
            // customers
            if (RandomGenerator.NextDouble() > 0.7)
            {
                EventBus.Publish(CustomerArrivedEvent.Raise(time));
            }

            // barbers
            foreach (var barber in Barbers)
            {
                if (barber.Status == WorkingStatus.Idle && CustomerQueue.Any())
                {
                    barber.StartWithCustomer(time, CustomerQueue.Dequeue());
                    Console.WriteLine($"Barber {barber.Id} starting to serve customer {barber.ServingCustomer.Id}");
                }

                if (barber.Status == WorkingStatus.Busy
                    && time - barber.ServiceStart > barber.ServingCustomer.ServiceTime)
                {
                    barber.Free();
                }
            }
        }

        private static void Initialize()
        {
            Barbers.Add(new Barber());
            Barbers.ForEach(x => Console.WriteLine($"Added Barber {x.Id}"));

            Console.WriteLine("Open for business");

            SubscribeEvents();
        }

        private static void SubscribeEvents()
        {
            EventBus.Subscribe<CustomerArrivedEvent>(OnCustomerArrived);
        }

        private static void OnCustomerArrived(CustomerArrivedEvent obj)
        {
            var newCustomer = new Customer
            {
                ArrivalTime = obj.ArrivalTime,
                ServiceTime = RandomGenerator.NextDouble() * 5d
            };
            CustomerQueue.Enqueue(newCustomer);

            Console.WriteLine($"New customer ({newCustomer.Id}) arrived at {newCustomer.ArrivalTime}. {CustomerQueue.Count} customers waiting.");
        }
    }
}