using DataTrack.Model;
using Microsoft.EntityFrameworkCore;

namespace DataTrack.DataBase;

public class DatabaseContext : DbContext
{
    
    public DbSet<Alarm> Alarm { get; set; }
    public DbSet<AnalogInput> AnalogInput { get; set; }
    public DbSet<AnalogOutput> AnalogOutput { get; set; }
    public DbSet<DigitalInput> DigitalInput { get; set; }
    public DbSet<DigitalOutput> DigitalOutput { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Device> Devices { get; set; }
   
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseMySQL(
    //         "server=localhost;port=3306;user=root;password=root123;database=datatrackdb");
    // }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // modelBuilder.Entity<User>()
        //     .HasIndex(c => new { c.Email })
        //     .IsUnique(true);
        //
        // modelBuilder.Entity<User>()
        //     .HasMany(e => e.AnalogInputs)
        //     .WithMany(e => e.Users)
        //     .UsingEntity("UsersToAnalogInputsJoinTable");
        //
        // modelBuilder.Entity<User>()
        //     .HasMany(e => e.DigitalInputs)
        //     .WithMany(e => e.Users)
        //     .UsingEntity("UsersToDigitalInputsJoinTable");
    }

}