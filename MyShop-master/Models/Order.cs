using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models;

[Index("CustomerId", Name = "IX_Orders_CustomerId")]
public partial class Order
{
    public Order()
    {
        OrderDetails = new HashSet<OrderDetail>();
    }

    [Key]
    public int Id { get; set; }
    public DateTime OrderPlaced { get; set; }
    public DateTime? OrderFulfilled { get; set; }
    public int? CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual Customer? Customer { get; set; } = null;
    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    public decimal TotalPayment
    {
        get
        {
            decimal total = 0;
            foreach (var item in OrderDetails)
            {
                if(item.SalePrice != null)
                {
                    total += (decimal)(item.SalePrice * item.Quantity);
                }
            }

            return total;
        }
    }

    public override string ToString()
    {
        return $"Id: {Id}, OrderPlaced: {OrderPlaced}, OrderFulfilled: {OrderFulfilled}, CustomerId: {CustomerId}";
    }

    public Order Clone()
    {
        return new Order()
        {
            OrderPlaced = OrderPlaced,
            OrderFulfilled = OrderFulfilled,
            CustomerId = CustomerId,
        };
    }

    //public DateTimeOffset OrderPlacedDateTimeOffset
    //{
    //    get
    //    {
    //        return new DateTimeOffset(OrderPlaced);
    //    }
    //    set
    //    {
    //        OrderPlaced = value.DateTime;
    //    }
    //}
}
