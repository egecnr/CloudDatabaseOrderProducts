using ExtraFunction.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace ExtraFunction.DAL
{
    public class DatabaseContext : DbContext
    {


        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Review> ProductReviews { get; set; } = null!;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseCosmos("https://sawa-db.documents.azure.com:443/",
                "gggcb28Z24nJAmpz4SRwQRNT9Xyd0wn1riSKAUkvVyaBf4WRALsyx4kgl6POPmi8Ka7JHZfTx06uWD3DHzoqTw==",
                "sawa-db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>().ToContainer("C_Products").HasPartitionKey(c => c.Id);
            modelBuilder.Entity<Review>().ToContainer("C_Reviews").HasPartitionKey(c => c.ProductId);
        }
    }
}
