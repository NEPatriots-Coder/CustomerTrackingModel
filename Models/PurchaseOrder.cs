using System;
using System.Collections.Generic;

namespace CRM.Desktop.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public required string PONumber { get; set; }
        public required string SupplierName { get; set; }
        public required string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<POLineItem> LineItems { get; set; } = new();

        public PurchaseOrder()
        {
            PONumber = string.Empty;
            SupplierName = string.Empty;
            Status = "Draft";
            OrderDate = DateTime.Now;
        }
    }
}