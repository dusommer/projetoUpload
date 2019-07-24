using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Upload.Domain.Entities;

namespace Upload.Infra.Persistence
{
    public class UploadContext : DbContext
    {
        public UploadContext() : base("UPLOADConnectionString")
        {

        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<File> Files { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("varchar"));



            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .HasMany(c => c.Files)
                .WithRequired(e => e.User)
                .HasForeignKey(c => c.IdUser);

            modelBuilder.Entity<File>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<File>()
                .Property(e => e.Size)
                .IsRequired();

            modelBuilder.Entity<File>()
                .Property(e => e.CreatedDate)
                .IsRequired();


            base.OnModelCreating(modelBuilder);
        }

    }
}
