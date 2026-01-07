using Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.DataContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<BankPaymentRequest> BankPaymentRequests { get; set; }
        public DbSet<PaymentInitializationRequest> PaymentInitializationRequests { get; set; }
        public DbSet<Merchant> Merchants { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
