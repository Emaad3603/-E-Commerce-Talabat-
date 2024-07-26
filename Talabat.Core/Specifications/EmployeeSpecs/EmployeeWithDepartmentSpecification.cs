using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.EmployeeSpecs
{
    public class EmployeeWithDepartmentSpecification :BaseSpecifications<Employee>

    {

        public EmployeeWithDepartmentSpecification() :base()
        { 
            Includes.Add(E=>E.Department);
        }
        public EmployeeWithDepartmentSpecification(int Id) :base(Employee=>Employee.Id ==Id)
        {

            Includes.Add(E => E.Department);
        }
    }
}
