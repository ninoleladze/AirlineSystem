using AirlineSystem.AirlineSystem.Domain.Entities;
using AirlineSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineSystem.AirlineSystem.Infrastructure.Persistence
{
    internal class AirlineDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightManifest> FlightManifests { get; set; }
        public DbSet<FlightAssignment> FlightAssignments { get; set; }
        public DbSet<CrewMember> CrewMembers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<TicketPromotion> TicketPromotions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectModels;Initial Catalog=AirlineSystem_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        }
        public AirlineDbContext() { }
        public AirlineDbContext(DbContextOptions<AirlineDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite PKs (EF can't guess)
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<TicketPromotion>().HasKey(tp => new { tp.TicketId, tp.PromotionId });

            // Fix multiple cascade paths (SQL Server error)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User).WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CrewMember>()
                .HasOne(c => c.User).WithOne()
                .HasForeignKey<CrewMember>(c => c.UserId).OnDelete(DeleteBehavior.NoAction);

            // Owned value objects (EF doesn't know these are embedded)
            modelBuilder.Entity<Flight>().OwnsOne(f => f.DepartureCoordinate, c => {
                c.Property(co => co.Latitude).HasColumnName("DepLat");
                c.Property(co => co.Longitude).HasColumnName("DepLng");
            });
            modelBuilder.Entity<Flight>().OwnsOne(f => f.ArrivalCoordinate, c => {
                c.Property(co => co.Latitude).HasColumnName("ArrLat");
                c.Property(co => co.Longitude).HasColumnName("ArrLng");
            });

            // Unique indexes
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Flight>().HasIndex(f => f.FlightNumber).IsUnique();
            modelBuilder.Entity<Aircraft>().HasIndex(a => a.TailNumber).IsUnique();
            modelBuilder.Entity<CrewMember>().HasIndex(c => c.LicenseNumber).IsUnique();
            modelBuilder.Entity<Promotion>().HasIndex(pr => pr.Code).IsUnique();
        }
    }
}
