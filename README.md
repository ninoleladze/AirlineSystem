# AirlineSystem

 AirlineSystem is a full-featured console application for managing airline operations.
  Passengers can register, search flights, book tickets, make payments, and apply promo
  codes. Admins manage flights, aircraft, crew assignments, user roles, and view revenue
  reports. Crew scheduling enforces a 10-hour rest period between flight assignments.

  Built with .NET 8, Entity Framework Core (SQL Server LocalDB), and BCrypt password
  hashing. Structured with Clean Architecture across Domain, Application, Infrastructure,
  and Console UI layers.

  Features:
  - Flight booking with seat conflict detection
  - Wallet-based payment system with full refund support
  - Promo code discounts
  - Crew scheduling with rest-period enforcement
  - Forward-only flight status transitions
  - Role-based access control (Admin / User / Crew)
  - Audit timestamps on all entities
  - Admin panel with revenue reporting and user management
