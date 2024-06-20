using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models;

[Index("CategoryId", Name = "IX_Products_CategoryId")]
public partial class Product
{
    public Product()
    {
        OrderDetails = new HashSet<OrderDetail>();
    }

    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    [Column(TypeName = "decimal(6, 2)")]
    public decimal SalePrice { get; set; }
    public int? CategoryId { get; set; }
    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal ImportPrice { get; set; }

    public int Quantity { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category? Category { get; set; }
    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}
