using DataTrack.Model;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Interface;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataTrack.DataBase;

public class DataBaseContextSeed
{
    private DatabaseContext _context;

    public DataBaseContextSeed(DatabaseContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (_context.Users.Any()) return;
        
        #region Users
        User admin1 = new User
        {
            FirstName = "Milos",
            LastName = "Cuturic",
            Email = "milos@email.com",
            Admin = true,
            Password = "$2a$12$EPDF.vrZaeJxAaHz9PK87uJTiaGTBY.sx7MouMG6At7Vcvgidu3wu"
        };
        
        User admin2 = new User
        {
            FirstName = "Luka",
            LastName = "Djordjevic",
            Email = "luka@email.com",
            Admin = true,
            Password = "$2a$12$EPDF.vrZaeJxAaHz9PK87uJTiaGTBY.sx7MouMG6At7Vcvgidu3wu"
        };
        
        User admin3 = new User
        {
            FirstName = "Marko",
            LastName = "Janosevic",
            Email = "marko@email.com",
            Admin = true,
            Password = "$2a$12$EPDF.vrZaeJxAaHz9PK87uJTiaGTBY.sx7MouMG6At7Vcvgidu3wu"
        };

        _context.Users.Add(admin1);
        _context.Users.Add(admin2);
        _context.Users.Add(admin3);
        
        #endregion

        #region Devices
        Device device1 = new Device
        {
            Name = "Analog 1",
            IOAddress = new Guid().ToString(),
            LowerBound = 0,
            Value = 50,
            UpperBound = 100,
            IsDigital = false,
            Driver = "RTU"
        };
        
        Device device2 = new Device
        {
            Name = "Analog 2",
            IOAddress = new Guid().ToString(),
            LowerBound = 0,
            Value = 50,
            UpperBound = 100,
            IsDigital = false,
            Driver = "SIMULATION"
        };
        
        Device device3 = new Device
        {
            Name = "Digital 1",
            IOAddress = new Guid().ToString(),
            LowerBound = 0,
            Value = 0,
            UpperBound = 1,
            IsDigital = true,
            Driver = "RTU"
        };

        _context.Devices.Add(device1);
        _context.Devices.Add(device2);
        _context.Devices.Add(device3);
        
        #endregion

        _context.SaveChanges();
    }
}