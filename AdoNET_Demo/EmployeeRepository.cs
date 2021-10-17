using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNET_Demo
{
    class EmployeeRepository
    {
        //Connecting to Database
        public static string connectionString = @"Data Source = (localdb)\ProjectsV13;Initial Catalog = payroleserviceADO; Integrated Security = True";
        //passing the string to sqlconnection to make connection 
        SqlConnection sqlconnection = new SqlConnection(connectionString);
        //GetAllEmployee method
        public void GetAllEmployee()
        {
            try
            {
                //Creating object for employeemodel and access the fields
                EmployeeModel employeeModel = new EmployeeModel();
                //Retrieve query
                string query = @"select * from employee_payroll";
                SqlCommand sqlCommand = new SqlCommand(query, sqlconnection);
                //Open the connection
                this.sqlconnection.Open();
                //ExecuteReader: Returns data as rows.
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employeeModel.EmployeeId = Convert.ToInt32(reader["id"] == DBNull.Value ? default : reader["id"]);
                        employeeModel.EmployeeName = reader["name"] == DBNull.Value ? default : reader["name"].ToString();
                        employeeModel.BasicPay = Convert.ToDouble(reader["Base_Pay"] == DBNull.Value ? default : reader["Base_Pay"]);
                        employeeModel.StartDate = (DateTime)(reader["startDate"] == DBNull.Value ? default(DateTime) : reader["startDate"]);
                        employeeModel.Gender = reader["gender"] == DBNull.Value ? default : reader["gender"].ToString();
                        employeeModel.Department = reader["department"] == DBNull.Value ? default : reader["department"].ToString();
                        employeeModel.PhoneNumber = Convert.ToDouble(reader["phone"] == DBNull.Value ? default : reader["phone"]);
                        employeeModel.Address = reader["address"] == DBNull.Value ? default : reader["address"].ToString();
                        employeeModel.Deductions = Convert.ToDouble(reader["Deductions"] == DBNull.Value ? default : reader["Deductions"]);
                        employeeModel.TaxablePay = Convert.ToDouble(reader["TaxablePay"] == DBNull.Value ? default : reader["TaxablePay"]);
                        employeeModel.IncomeTax = Convert.ToDouble(reader["IncomeTax"] == DBNull.Value ? default : reader["IncomeTax"]);
                        employeeModel.NetPay = Convert.ToDouble(reader["NetPay"] == DBNull.Value ? default : reader["NetPay"]);
                        Console.WriteLine("{0} {1} {2}  {3} {4} {5}  {6}  {7} {8} {9} {10} {11}", employeeModel.EmployeeId, employeeModel.EmployeeName, employeeModel.BasicPay, employeeModel.StartDate, employeeModel.Gender, employeeModel.Department, employeeModel.PhoneNumber, employeeModel.Address, employeeModel.Deductions, employeeModel.TaxablePay, employeeModel.IncomeTax, employeeModel.NetPay);
                        Console.WriteLine("\n");
                    }
                }
                else
                {
                    Console.WriteLine("No data found");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                this.sqlconnection.Close();
            }
        }

        // Update the salary data 
        public void UpdateSalary()
        {
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                sqlconnection.Open();
                string query = @"update employee_payroll set Base_pay=3000000 where name='Sunil'";
                SqlCommand command = new SqlCommand(query, sqlconnection);

                int result = command.ExecuteNonQuery();
                if (result != 0)
                {
                    Console.WriteLine("Salary Updated Successfully ");
                }
                else
                {
                    Console.WriteLine("Unsuccessfull");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlconnection.Close();

            }
        }

        //Delete data from table
        public void DeleteData()
        {
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                sqlconnection.Open();
                string query = @"Delete from employee_payRoll where Name='Sunil'";
                SqlCommand command = new SqlCommand(query, sqlconnection);

                int result = command.ExecuteNonQuery();
                if (result != 0)
                {
                    Console.WriteLine("Name deleted Successfully ");
                }
                else
                {
                    Console.WriteLine("Unsuccessfull");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlconnection.Close();

            }
        }
        //Adding new employee details to the database
        public void AddEmployee(EmployeeModel model)
        {
            try
            {
                using (this.sqlconnection)
                {
                    //we can also use update query to add employee, but using stored procedure is to write once and reuse concept
                    //query will compile every time, but SP compile one time till u modify anything
                    SqlCommand command = new SqlCommand("dbo.SpAddEmployeeDetails", this.sqlconnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@name", model.EmployeeName);
                    command.Parameters.AddWithValue("@Base_Pay", model.BasicPay);
                    command.Parameters.AddWithValue("@start", model.StartDate);
                    command.Parameters.AddWithValue("@gender", model.Gender);
                    command.Parameters.AddWithValue("@phone_number", model.PhoneNumber);
                    command.Parameters.AddWithValue("@address", model.Address);
                    command.Parameters.AddWithValue("@department", model.Department);
                    command.Parameters.AddWithValue("@Taxable_pay", model.TaxablePay);
                    sqlconnection.Open();
                    //ExecuteNonQuery return how many row affected by executing query
                    var result = command.ExecuteNonQuery();
                    if (result != 0)
                    {
                        Console.WriteLine("Successfully inserted the records");
                    }
                    else
                    {
                        Console.WriteLine("Insertion of result is unsuccessfull");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlconnection.Close();
            }
        }
        //Inserting in to two table without using transaction query
        public void InsertIntoTwoTables()
        {
            using (sqlconnection)
            {
                sqlconnection.Open();

                //Enlist a command in the current transaction
                SqlCommand command = sqlconnection.CreateCommand();
                try
                {
                    //Execute two separate commands 
                    command.CommandText = "Insert into EmployeeTest(name) values('Savita')";
                    command.ExecuteScalar();
                    Console.WriteLine("Inserted in to Employee table is successful");
                    command.CommandText = "Insert into Salary(Salary,EmpId) values ('4200','1)";
                    command.ExecuteNonQuery();
                    Console.WriteLine("Inserted in to Salary table is successful");
                    //Insert wrong colomn number will throw exception, but the first table is updated in this without transaction query case
                    //command.CommandText = "Insert into Salary(Salary,EmpId2) values ('4200','1)";
                }
                catch (Exception ex)
                {
                    //Handle the exception if the transaction fails to commit
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //Inserting in to two table using transaction query
        public void InsertIntoTwoTablesWithTransactions()
        {
            using (sqlconnection)
            {
                sqlconnection.Open();

                // Start a local transaction.
                SqlTransaction sqlTran = sqlconnection.BeginTransaction();

                // Enlist a command in the current transaction.
                SqlCommand command = sqlconnection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    // Execute two separate commands.
                    command.CommandText =
                      "insert into EmployeeTest(name) values('Pinki')";
                    command.ExecuteScalar();
                    Console.WriteLine("Inserted into Employee table successfully.");
                    command.CommandText =
                      "insert into Salary(Salary,EmpId) values('7000','7')";
                    command.ExecuteNonQuery();
                    Console.WriteLine("Inserted into  table successfully.");
                    // Commit the transaction.
                    sqlTran.Commit();
                    Console.WriteLine("Both records were written to database.");
                }
                catch (Exception ex)
                {
                    // Handle the exception if the transaction fails to commit.
                    Console.WriteLine(ex.Message);

                    try
                    {
                        // Attempt to roll back the transaction if any of the action fails 
                        sqlTran.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        // Throws an InvalidOperationException if the connection back on the server.
                        Console.WriteLine(exRollback.Message);
                    }
                }
            }
        }
    }
}
