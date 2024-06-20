using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models;

public partial class Customer
{
    public Customer()
    {
        Orders = new HashSet<Order>();
    }

    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string Email { get; set; } = null!;

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}

