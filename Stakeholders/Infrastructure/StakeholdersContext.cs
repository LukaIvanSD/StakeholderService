using Microsoft.EntityFrameworkCore;
using System;
using Stakeholders.Core.Domain;

namespace Stakeholders.Infrastructure
{
    public class StakeholdersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }

        public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("stakeholders");


            ConfigureStakeholder(modelBuilder);
        }

        private static void ConfigureStakeholder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();
            modelBuilder.Entity<Person>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Person>(p => p.UserId);
        }
    }
}
