namespace twentySix.BarberSimulation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;

    using Core.Events;
    using Core.Models;
    using Core.Providers;

    public class Program
    {
        private const double PriceOfCut = 15d;

        private const double MaxHrs = 20d;

        private static readonly double DeltaTime = 0.1d;

        private static readonly List<Barber> Barbers = new();

        private static readonly Random RandomGenerator = new();

        private static readonly List<double> TotalTime = new();

        private static double earnings;

        public static void Main()
        {
            Console.Clear();
            Console.WriteLine("--- Barber Simulations ---");

            Console.WriteLine("(Press ESC to stop)\n");

            Initialize();

            var time = 0d;

            var simulationTimer = new Timer { AutoReset = true, Interval = 50 };

            simulationTimer.Elapsed += (_, _) =>
                {
                    time += DeltaTime;
                    EventBusProvider.Instance.EventBus.Publish(TimeChangedEvent.Raise(time));

                    Update(time);

                    if (time > MaxHrs)
                    {
                        simulationTimer.Stop();
                        Console.WriteLine("No more business today.");
                        Console.WriteLine($"Total earnings: {earnings}");
                        Console.WriteLine($"Average time spent: {TotalTime.Average()}");
                        Console.WriteLine($"Average earning/hr: {earnings / MaxHrs:f2}");
                    }
                };

            simulationTimer.Start();

            Console.ReadKey();
        }

        private static void Update(double time)
        {
            // customers
            if (RandomGenerator.NextDouble() > 0.8)
            {
                EventBusProvider.Instance.EventBus.Publish(CustomerArrivedEvent.Raise(time));
            }
        }

        private static void Initialize()
        {
            Barbers.Add(new Barber());
            Barbers.Add(new Barber());
            Console.WriteLine($"Added {Barbers.Count} barbers");

            Console.WriteLine("Opened for business");

            SubscribeEvents();
        }

        private static void SubscribeEvents()
        {
            EventBusProvider.Instance.EventBus.Subscribe<TimeChangedEvent>(OnTimeChanged);
            EventBusProvider.Instance.EventBus.Subscribe<CustomerArrivedEvent>(OnCustomerArrived);
            EventBusProvider.Instance.EventBus.Subscribe<CustomerLeftEvent>(OnCustomerLeft);
        }

        private static void OnTimeChanged(TimeChangedEvent obj)
        {
            CustomerQueueProvider.Instance.Queue
                .Where(x => x.ToleranceTime < obj.Time - x.ArrivalTime)
                .ToList()
                .ForEach(x =>
                    {
                        CustomerQueueProvider.Instance.Queue.Remove(x);
                        Console.WriteLine($"{obj.Time:f2}: Customer ({x.Id}) unhappy. Leaving.");
                    });
        }

        private static void OnCustomerLeft(CustomerLeftEvent obj)
        {
            earnings += PriceOfCut;
            TotalTime.Add(obj.DepartureTime - obj.Customer.ArrivalTime);
        }

        private static void OnCustomerArrived(CustomerArrivedEvent obj)
        {
            var serviceTime = RandomGenerator.NextDouble() * 4d;
            var newCustomer = new Customer
            {
                ArrivalTime = obj.ArrivalTime,
                ServiceTime = serviceTime,
                ToleranceTime = serviceTime + (RandomGenerator.NextDouble() * 3d)
            };

            CustomerQueueProvider.Instance.Queue.Enqueue(newCustomer);

            Console.WriteLine($"{obj.ArrivalTime:f2}: New customer ({newCustomer.Id}). {CustomerQueueProvider.Instance.Queue.Count} customers waiting.");
        }
    }
}