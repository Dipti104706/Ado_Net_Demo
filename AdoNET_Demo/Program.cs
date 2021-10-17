using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNET_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Creating a object for employeerepository
            EmployeeRepository repository = new EmployeeRepository();
            //Console.WriteLine("Id Name BasicPay Startdate Gender Department PhoneNumber Address Deduction Tax IncomeTax NetPay \n");
            //repository.GetAllEmployee();
            //For adding employee
            //EmployeeModel model = new EmployeeModel();
            //model.EmployeeName = "Sushant";
            //model.BasicPay = 900000;
            //model.StartDate = DateTime.Today;
            //model.Gender = "M";
            //model.PhoneNumber = 45698741256;
            //model.Address = "Pune";
            //model.Department = "Sales";
            //model.TaxablePay = 3500;
            //repository.AddEmployee(model);
            //repository.InsertIntoTwoTablesWithTransactions();
            //repository.UpdateSalary();
            repository.DeleteData();
            Console.ReadLine();
        }
    }
}
