﻿using DataTrack.Model.Utils;

namespace DataTrack.Model;

public class User : IBaseEntity
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; } 
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public bool Admin { get; set; }
    
    public User? RegisteredBy { get; set; }

    public User()
    {
    }

    public User(Guid id, string firstName, string lastName, string email, string password, 
        bool adming, User? registeredBy)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Admin = adming;
        RegisteredBy = registeredBy;
    }
}