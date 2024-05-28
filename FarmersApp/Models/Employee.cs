using System;
using System.Collections.Generic;

namespace FarmersApp.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Farmers = new HashSet<Farmer>();
        }

        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual ICollection<Farmer> Farmers { get; set; }
    }
}
