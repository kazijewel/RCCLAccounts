using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Data.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Other properties of the employee entity

        // Navigation properties for relationships, if any
        public ICollection<Department> Departments { get; set; }
    }

    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Other properties of the department entity

        // Navigation properties for relationships, if any
        public ICollection<Employee> Employees { get; set; }
    }
}
