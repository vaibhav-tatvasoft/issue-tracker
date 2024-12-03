using ChattingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApplication.DataAccess
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Group>()
                .HasOne(g => g.createdByUser)
                .WithMany()
                .HasForeignKey(g => g.createdBy);

            modelBuilder.Entity<Message>()
                .HasOne(g => g.fromUser)
                .WithMany(g => g.receivedMessages)
                .HasForeignKey(g => g.from);

            modelBuilder.Entity<Message>()
                .HasOne(g => g.toUser)
                .WithMany(g => g.sentMessages)
                .HasForeignKey(g => g.to);
        }
    }
}
