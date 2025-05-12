using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class MyDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<User_details> User_details { get; set; }
        public DbSet<Appointments> Appointments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-AADOHV3\SQLEXPRESS;Database=FinalDB_UserDatas;Integrated Security=True;Encrypt=False;");
        }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and foreign keys

            // Logs - Users (One to Many)
            modelBuilder.Entity<Logs>()
                .HasOne(log => log.Users)
                .WithMany(user => user.Logs)
                .HasForeignKey(log => log.user_id)
                .IsRequired();

            // Logs - InvolvedUser (One to Many)
            modelBuilder.Entity<Logs>()
                .HasOne(log => log.InvolvedUser)
                .WithMany()
                .HasForeignKey(log => log.involved_user);

            // User_details - Users (One to One)
            modelBuilder.Entity<User_details>()
                .HasOne(ud => ud.Users)
                .WithOne(u => u.User_details)
                .HasForeignKey<User_details>(ud => ud.user_id)
                .IsRequired();

            // Appointments - Doctor (One to Many)
            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.doctor_id)
                .IsRequired();

            // Appointments - User (One to Many)
            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.user_id)
                .IsRequired();

            // Explicitly setting the primary key for Appointments
            modelBuilder.Entity<Appointments>()
                .HasKey(a => a.appointment_id);  // This defines the primary key

            modelBuilder.Entity<Logs>()
                .HasKey(l => l.log_id);

            modelBuilder.Entity<Appointments>()
                .HasKey(a => a.appointment_id);

            modelBuilder.Entity<Users>()
                .HasKey(u => u.user_id);
        }



    }
}
