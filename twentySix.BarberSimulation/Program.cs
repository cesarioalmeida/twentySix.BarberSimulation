namespace twentySix.BarberSimulation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;

    using Redbus;
    using Redbus.Interfaces;

    using twentySix.BarberSimulation.Core.Enums;
    using twentySix.BarberSimulation.Core.Events;
    using twentySix.BarberSimulation.Core.Framework;
    using twentySix.BarberSimulation.Core.Models;

    public class Program
    {
        private const double PriceOfCut = 15d;

        private static readonly IEventBus EventBus = new EventBus();

        private static readonly double DeltaTime = 0.1d;

        private static readonly List<Barber> Barbers = new List<Barber>();

        private static readonly ExtendedQueue<Customer> CustomerQueue = new ExtendedQueue<Customer>();

        private static readonly Random RandomGenerator = new Random();

        private static readonly List<double> TotalTime = new List<double>();

        private static double earnings;

        public static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("--- Barber Simulations ---");

            Console.WriteLine("(Press ESC to stop)\n");

            Initialize();

            var time = 8d;

            var simulationTimer = new Timer { AutoReset = true, Interval = 100 };

            simulationTimer.Elapsed += (sender, eventArgs) =>
                {
                    time += DeltaTime;
                    EventBus.Publish(TimeChangedEvent.Raise(time));

                    Update(time);

                    if (time > 19)
                    {
                        simulationTimer.Stop();
                        Console.WriteLine("No more business today.");
                        Console.WriteLine($"Total earnings: {earnings}");
                        Console.WriteLine($"Average time spent: {TotalTime.Average()}");
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
                EventBus.Publish(CustomerArrivedEvent.Raise(time));
            }

            // barbers
            foreach (var barber in Barbers)
            {
                if (barber.Status == WorkingStatus.Idle && CustomerQueue.Any())
                {
                    barber.StartWithCustomer(time, CustomerQueue.Dequeue());
                    Console.WriteLine($"{time:f2}: Barber ({barber.Id}) starting to serve customer ({barber.ServingCustomer.Id})");
                }

                if (barber.Status == WorkingStatus.Busy
                    && time - barber.ServiceStart > barber.ServingCustomer.ServiceTime)
                {
                    TotalTime.Add(time - barber.ServingCustomer.ArrivalTime);

                    EventBus.Publish(CustomerLeftEvent.Raise(time, barber.ServingCustomer));

                    barber.Free();
                }
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
            EventBus.Subscribe<TimeChangedEvent>(OnTimeChanged);
            EventBus.Subscribe<CustomerArrivedEvent>(OnCustomerArrived);
            EventBus.Subscribe<CustomerLeftEvent>(OnCustomerLeft);
        }

        private static void OnTimeChanged(TimeChangedEvent obj)
        {
            CustomerQueue
                .Where(x => x.ToleranceTime < obj.Time - x.ArrivalTime)
                .ToList()
                .ForEach(x =>
                    {
                        CustomerQueue.Remove(x);
                        Console.WriteLine($"Customer ({x.Id}) unhappy. Leaving.");
                    });
        }

        private static void OnCustomerLeft(CustomerLeftEvent obj)
        {
            earnings += PriceOfCut;
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

            CustomerQueue.Enqueue(newCustomer);

            Console.WriteLine($"{obj.ArrivalTime:f2}: New customer ({newCustomer.Id}) arrived at {newCustomer.ArrivalTime}. {CustomerQueue.Count} customers waiting.");
        }
    }
}