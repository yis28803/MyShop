using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models;

[Index("OrderId", Name = "IX_OrderDetails_OrderId")]
[Index("ProductId", Name = "IX_OrderDetails_ProductId")]
public partial class OrderDetail
{
    [Key]
    public int Id { get; set; }
    public int? Quantity { get; set; }
    public int? ProductId { get; set; }
    public int? OrderId { get; set; }
    public decimal? ImportPrice { get; set; }
    public decimal? SalePrice { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order? Order { get; set; } = null;


    [ForeignKey("ProductId")]
    [InverseProperty("OrderDetails")]
    public virtual Product Product { get; set; } = null!;

    public OrderDetail Clone()
    {
        var result = new OrderDetail
        {
            Quantity = Quantity,
            ProductId = ProductId,
            //result.Product = Product;    //why cannot insert to database if have this line??
            OrderId = OrderId,
            ImportPrice = ImportPrice,
            SalePrice = SalePrice
        };
        return result;
    }

    public override string ToString()
    {
        var v = $"Id: {Id}, Quantity: {Quantity}, ProductId: {ProductId}, OrderId: {OrderId}, ImportPrice: {ImportPrice}, SalePrice: {SalePrice}";
        return v;
    }
}

