using KRV.LawnPro.BL.Models;
using KRV.LawnPro.PL;
using KRV.LawnPro.Reporting;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRV.LawnPro.BL
{
    public static class EmployeeManager
    {
        public async static Task<List<Employee>> Load()
        {
            try
            {
                List<Employee> employees = new List<Employee>();

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        dc.tblEmployees
                            .ToList()
                            .ForEach(r => employees
                            .Add(new Employee
                            {
                                Id = r.Id,
                                FirstName = r.FirstName,
                                LastName = r.LastName,
                                StreetAddress = r.StreetAddress,
                                City = r.City,
                                State = r.State,
                                ZipCode = r.ZipCode,
                                Email = r.Email,
                                Phone = r.Phone,
                                UserId = r.UserId
                            }));
                    }
                });

                return employees;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<Employee> LoadById(Guid id)
        {
            try
            {
                Employee employee = new Employee();

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        tblEmployee row = dc.tblEmployees.FirstOrDefault(r => r.Id == id);
                        if (row != null)
                        {
                            employee.Id = row.Id;
                            employee.FirstName = row.FirstName;
                            employee.LastName = row.LastName;
                            employee.StreetAddress = row.StreetAddress;
                            employee.City = row.City;
                            employee.State = row.State;
                            employee.ZipCode = row.ZipCode;
                            employee.Email = row.Email;
                            employee.Phone = row.Phone;
                            employee.UserId = row.UserId;
                        }
                        else
                        {
                            throw new Exception("Employee not found");
                        }
                    }
                });

                return employee;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<Employee> LoadByUserId(Guid userId)
        {
            try
            {
                Employee employee = new Employee();

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        tblEmployee row = dc.tblEmployees.FirstOrDefault(r => r.UserId == userId);
                        if (row != null)
                        {
                            employee.Id = row.Id;
                            employee.FirstName = row.FirstName;
                            employee.LastName = row.LastName;
                            employee.StreetAddress = row.StreetAddress;
                            employee.City = row.City;
                            employee.State = row.State;
                            employee.ZipCode = row.ZipCode;
                            employee.Email = row.Email;
                            employee.Phone = row.Phone;
                            employee.UserId = row.UserId;
                        }
                        else
                        {
                            throw new Exception("Employee not found");
                        }
                    }
                });

                return employee;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async static Task<bool> Insert(Employee employee, bool rollback = false)
        {
            try
            {
                int result = 0;

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        IDbContextTransaction transaction = null;
                        if (rollback) transaction = dc.Database.BeginTransaction();

                        tblEmployee newRow = new tblEmployee();
                        // Teranary operator
                        newRow.Id = Guid.NewGuid();
                        newRow.FirstName = employee.FirstName;
                        newRow.LastName = employee.LastName;
                        newRow.StreetAddress = employee.StreetAddress;
                        newRow.City = employee.City;
                        newRow.State = employee.State;
                        newRow.ZipCode = employee.ZipCode;
                        newRow.Email = employee.Email;
                        newRow.Phone = employee.Phone;
                        newRow.UserId = employee.UserId;

                        // Backfill the id on the input parameter employee
                        employee.Id = newRow.Id;

                        // Insert the row
                        dc.tblEmployees.Add(newRow);

                        // Commit the changes and get the number of rows affected
                        result = dc.SaveChanges();

                        if (rollback) transaction.Rollback();
                    }
                });

                return result == 1;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async static Task<int> Update(Employee employee, bool rollback = false)
        {
            try
            {
                int results = 0;

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        IDbContextTransaction transaction = null;
                        if (rollback) transaction = dc.Database.BeginTransaction();

                        tblEmployee upDateRow = dc.tblEmployees.FirstOrDefault(r => r.Id == employee.Id);

                        if (upDateRow != null)
                        {
                            upDateRow.FirstName = employee.FirstName;
                            upDateRow.LastName = employee.LastName;
                            upDateRow.StreetAddress = employee.StreetAddress;
                            upDateRow.City = employee.City;
                            upDateRow.State = employee.State;
                            upDateRow.ZipCode = employee.ZipCode;
                            upDateRow.Email = employee.Email;
                            upDateRow.Phone = employee.Phone;
                            upDateRow.UserId = employee.UserId;

                            dc.tblEmployees.Update(upDateRow);

                            // Commit the changes and get the number of rows affected
                            results = dc.SaveChanges();

                            if (rollback) transaction.Rollback();
                        }
                        else
                        {
                            throw new Exception("Employee was not found.");
                        }
                    }
                });

                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<int> Delete(Guid id, bool rollback = false)
        {
            try
            {
                int results = 0;

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        // Check if employee is associated with an exisiting appointment = do not allow delete ....
                        bool inuse = dc.tblAppointments.Any(e => e.EmployeeId == id);

                        if (inuse && rollback == false)
                        {
                            throw new Exception("This employee is associated with an existing appointment or invoice and therefore cannot be deleted.");
                        }
                        else
                        {
                            IDbContextTransaction transaction = null;
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            tblEmployee deleteRow = dc.tblEmployees.FirstOrDefault(r => r.Id == id);

                            if (deleteRow != null)
                            {
                                dc.tblEmployees.Remove(deleteRow);
                                results = dc.SaveChanges();

                                if (rollback) transaction.Rollback();
                            }
                            else
                            {
                                throw new Exception("Employee was not found.");
                            }
                        }
                    }
                });

                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void ExportExcel(List<Employee> employees)
        {
            try
            {
                string[,] data = new string[employees.Count + 1, 10];
                int counter = 0;

                data[counter, 0] = "First Name";
                data[counter, 1] = "Last Name";
                data[counter, 2] = "Street Address";
                data[counter, 3] = "City";
                data[counter, 4] = "State";
                data[counter, 5] = "ZipCode";
                data[counter, 6] = "Email";
                data[counter, 7] = "Phone";
                data[counter, 8] = "User";

                counter++;

                foreach (Employee c in employees)
                {

                    data[counter, 0] = c.FirstName;
                    data[counter, 1] = c.LastName;
                    data[counter, 2] = c.StreetAddress;
                    data[counter, 3] = c.City;
                    data[counter, 4] = c.State;
                    data[counter, 5] = c.ZipCode;
                    data[counter, 6] = c.Email;
                    data[counter, 7] = c.Phone;
                    data[counter, 8] = CustomerManager.LoadByUserId(c.UserId).ToString();


                    counter++;

                }
                string filename = "Employees" + "-" + DateTime.Now.ToString("MM-dd-yyyy");
                Excel.Export(filename, data);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
