using AirlineSystem.AirlineSystem.Domain.Entities;
using AirlineSystem.AirlineSystem.Domain.Enums;
using AirlineSystem.AirlineSystem.Domain.ValueObjects;
using AirlineSystem.AirlineSystem.Infrastructure.Persistence;


namespace AirlineSystem.Infrastructure.Seed
{
    internal class DataSeeder
    {
        private AirlineDbContext DC = new AirlineDbContext();

        public void Seed()
        {
            if (DC.Aircrafts.Any()) return;

           
            var a1 = new Aircraft
            {
                Model = "Boeing 737",
                TailNumber = "TC-AAA",
                Capacity = 180,
                Manufacturer = "Boeing",
                YearManufactured = 2018
            };
            var a2 = new Aircraft
            {
                Model = "Airbus A320",
                TailNumber = "TC-BBB",
                Capacity = 150,
                Manufacturer = "Airbus",
                YearManufactured = 2020
            };
            var a3 = new Aircraft
            {
                Model = "Boeing 777",
                TailNumber = "TC-CCC",
                Capacity = 396,
                Manufacturer = "Boeing",
                YearManufactured = 2015
            };
            DC.Aircrafts.AddRange(a1, a2, a3);

            var f1 = new Flight
            {
                FlightNumber = "TK-001",
                DepartureAirport = "Istanbul (IST)",
                ArrivalAirport = "London (LHR)",
                DepartureTime = DateTime.Now.AddDays(1),
                ArrivalTime = DateTime.Now.AddDays(1).AddHours(4),
                Status = FlightStatus.Scheduled,
                AvailableSeats = 180,
                Aircraft = a1,
                BasePrice = 350m,
                PriceCurrency = "USD",
                DepartureCoordinate = new Coordinate(41.27, 28.75),
                ArrivalCoordinate = new Coordinate(51.47, -0.46)
            };

            var f2 = new Flight
            {
                FlightNumber = "TK-002",
                DepartureAirport = "Istanbul (IST)",
                ArrivalAirport = "New York (JFK)",
                DepartureTime = DateTime.Now.AddDays(2),
                ArrivalTime = DateTime.Now.AddDays(2).AddHours(11),
                Status = FlightStatus.Scheduled,
                AvailableSeats = 396,
                Aircraft = a3,
                BasePrice = 850m,
                PriceCurrency = "USD",
                DepartureCoordinate = new Coordinate(41.27, 28.75),
                ArrivalCoordinate = new Coordinate(40.63, -73.77)
            };

            var f3 = new Flight
            {
                FlightNumber = "TK-003",
                DepartureAirport = "Istanbul (IST)",
                ArrivalAirport = "Paris (CDG)",
                DepartureTime = DateTime.Now.AddDays(1).AddHours(6),
                ArrivalTime = DateTime.Now.AddDays(1).AddHours(9.5),
                Status = FlightStatus.Scheduled,
                AvailableSeats = 150,
                Aircraft = a2,
                BasePrice = 320m,
                PriceCurrency = "USD",
                DepartureCoordinate = new Coordinate(41.27, 28.75),
                ArrivalCoordinate = new Coordinate(49.00, 2.54)
            };

            var f4 = new Flight
            {
                FlightNumber = "TK-004",
                DepartureAirport = "London (LHR)",
                ArrivalAirport = "Dubai (DXB)",
                DepartureTime = DateTime.Now.AddDays(3),
                ArrivalTime = DateTime.Now.AddDays(3).AddHours(7),
                Status = FlightStatus.Scheduled,
                AvailableSeats = 180,
                Aircraft = a1,
                BasePrice = 650m,
                PriceCurrency = "USD",
                DepartureCoordinate = new Coordinate(51.47, -0.46),
                ArrivalCoordinate = new Coordinate(25.25, 55.36)
            };

            var f5 = new Flight
            {
                FlightNumber = "TK-005",
                DepartureAirport = "Istanbul (IST)",
                ArrivalAirport = "Tokyo (NRT)",
                DepartureTime = DateTime.Now.AddDays(4),
                ArrivalTime = DateTime.Now.AddDays(4).AddHours(12),
                Status = FlightStatus.Scheduled,
                AvailableSeats = 396,
                Aircraft = a3,
                BasePrice = 1200m,
                PriceCurrency = "USD",
                DepartureCoordinate = new Coordinate(41.27, 28.75),
                ArrivalCoordinate = new Coordinate(35.76, 140.38)
            };

            DC.Flights.AddRange(f1, f2, f3, f4, f5);
            DC.SaveChanges();

            if (!DC.Promotions.Any())
            {
                DC.Promotions.Add(new Promotion
                {
                    Code = "WELCOME10",
                    Description = "10% off your first ticket",
                    DiscountPercent = 10,
                    ExpiryDate = DateTime.Now.AddYears(1),
                    IsActive = true,
                    MaxUses = 100
                });
                DC.SaveChanges();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Flight data seeded successfully (5 flights).");
            Console.ResetColor();
        }
    }
}