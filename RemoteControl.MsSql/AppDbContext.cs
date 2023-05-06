using Microsoft.EntityFrameworkCore;
using RemoteControl.Business.Common;
using RemoteControl.Business.Models;

namespace RemoteControl.MsSql
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Session> Sessions { get; set; }


        public const string DefaultConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RemoteControl;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";

        public override int SaveChanges()
        {
            InitEntities();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            InitEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void InitEntities()
        {
            var rnd = new Random();
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entityEntry in entities)
            {
                var entity = entityEntry.Entity as BaseModel;
                var now = DateTime.UtcNow;
                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = now;

                    if (entity.Id == Guid.Empty)
                        entity.Id = Guid.NewGuid();
                }
                entity.UpdatedAt = now;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });


            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasIndex(e => e.Token).IsUnique();
            });
        }
    }
}
