using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<BankPaymentRequest> BankPaymentRequests { get; set; }
        public DbSet<BankPaymentResponse> BankPaymentResponses { get; set; }
        public DbSet<PaymentCard> PaymentCards { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<PaymentFinalization> PaymentFinalizations { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
