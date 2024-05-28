using System;
using System.Collections.Generic;

namespace FarmersApp.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime ProductionDate  { get; set; } = DateTime.Now;
        public int CategoryId { get; set; }
        public int Farmer { get; set; }
   
        public virtual Category? Category { get; set; } = null!;
        public virtual Farmer? FarmerNavigation { get; set; } = null!;
    }
}
