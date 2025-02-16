// Forms/PODetailForm.cs
using System;

namespace CRM.Desktop.Models
{
    public class POLineItem
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public required string PartNumber { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public POLineItem()
        {
            PartNumber = string.Empty;
            Quantity = 1;
            UnitPrice = 0.0m;
        }
    }
}


   