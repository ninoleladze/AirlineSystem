# AirlineSystem
AirlineSystem - Flight Management Console App  C# console application with EF Core &amp; Clean Architecture. Manage flights, crew, bookings, payments, and user wallets with role-based access.  Tech: .NET 8 | EF Core | SQL Server | Authentication, Flight scheduling, Ticket booking, Wallet system, Promo codes, Admin panel, Logging
AirlineSystem - Complete Flight Management Console Application

A full-stack C# console application built with EF Core, Clean Architecture, and MSSQL. 
Manage flights, crew, bookings, payments, promotions, and user wallets with role-based access control.

Features:
- User authentication with role-based permissions (Admin/User/Crew)
- Flight scheduling with aircraft conflict detection
- Ticket booking with seat selection and real-time availability
- User wallet system with balance tracking
- Payment processing and refund management
- Promo code system with automatic discount application
- Crew assignment with 10-hour rest period enforcement
- Flight manifest auto-generation on departure
- Search and filter capabilities (flights, crew, tickets)
- Admin panel (revenue reports, role management, maintenance tracking)
- File-based logging system
- Comprehensive xUnit test suite

Tech Stack:
- .NET 8 / C#
- Entity Framework Core (Code-First)
- SQL Server LocalDB
- Clean Architecture (Domain/Application/Infrastructure/UI layers)

Database includes: Users, Roles, Flights, Aircraft, Crew, Tickets, Payments, Promotions, Maintenance Records
All relationships configured: One-to-One, One-to-Many, Many-to-Many with payload join entities
