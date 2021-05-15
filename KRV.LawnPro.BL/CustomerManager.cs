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
    public static class CustomerManager
    {
        public async static Task<List<Customer>> Load()
        {
            try
            {
                List<Customer> customers = new List<Customer>();
                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        dc.tblCustomers
                            .ToList()
                            .ForEach(r => customers
                            .Add(new Customer
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
                                PropertySqFeet = r.PropertySqFt,
                                UserId = r.UserId
                            }));
                    }
                });

                return customers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<Customer> LoadById(Guid id)
        {
            try
            {
                Customer customer = new Customer();

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        tblCustomer row = dc.tblCustomers.FirstOrDefault(r => r.Id == id);
                        if (row != null)
                        {
                            customer.Id = row.Id;
                            customer.FirstName = row.FirstName;
                            customer.LastName = row.LastName;
                            customer.StreetAddress = row.StreetAddress;
                            customer.City = row.City;
                            customer.State = row.State;
                            customer.ZipCode = row.ZipCode;
                            customer.Email = row.Email;
                            customer.Phone = row.Phone;
                            customer.PropertySqFeet = row.PropertySqFt;
                            customer.UserId = row.UserId;
                        }
                        else
                        {
                            throw new Exception("Customer not found");
                        }
                    }
                });

                return customer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<Customer> LoadByUserId(Guid userId)
        {
            try
            {
                Customer customer = new Customer();

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        tblCustomer row = dc.tblCustomers.FirstOrDefault(r => r.UserId == userId);
                        if (row != null)
                        {
                            customer.Id = row.Id;
                            customer.FirstName = row.FirstName;
                            customer.LastName = row.LastName;
                            customer.StreetAddress = row.StreetAddress;
                            customer.City = row.City;
                            customer.State = row.State;
                            customer.ZipCode = row.ZipCode;
                            customer.Email = row.Email;
                            customer.Phone = row.Phone;
                            customer.PropertySqFeet = row.PropertySqFt;
                            customer.UserId = row.UserId;
                        }
                        else
                        {
                            throw new Exception("Customer not found");
                        }
                    }
                });

                return customer;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async static Task<bool> Insert(Customer customer, bool rollback = false)
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

                        tblCustomer newRow = new tblCustomer();
                        // Teranary operator
                        newRow.Id = Guid.NewGuid();
                        newRow.FirstName = customer.FirstName;
                        newRow.LastName = customer.LastName;
                        newRow.StreetAddress = customer.StreetAddress;
                        newRow.City = customer.City;
                        newRow.State = customer.State;
                        newRow.ZipCode = customer.ZipCode;
                        newRow.Email = customer.Email;
                        newRow.Phone = customer.Phone;
                        newRow.PropertySqFt = customer.PropertySqFeet;
                        newRow.UserId = customer.UserId;

                        // Backfill the id on the input parameter customer
                        customer.Id = newRow.Id;

                        // Insert the row
                        dc.tblCustomers.Add(newRow);

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


        public async static Task<int> Update(Customer customer, bool rollback = false)
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

                        tblCustomer upDateRow = dc.tblCustomers.FirstOrDefault(r => r.Id == customer.Id);

                        if (upDateRow != null)
                        {
                            upDateRow.FirstName = customer.FirstName;
                            upDateRow.LastName = customer.LastName;
                            upDateRow.StreetAddress = customer.StreetAddress;
                            upDateRow.City = customer.City;
                            upDateRow.State = customer.State;
                            upDateRow.ZipCode = customer.ZipCode;
                            upDateRow.Email = customer.Email;
                            upDateRow.Phone = customer.Phone;
                            upDateRow.PropertySqFt = customer.PropertySqFeet;
                            upDateRow.UserId = customer.UserId;

                            dc.tblCustomers.Update(upDateRow);

                            // Commit the changes and get the number of rows affected
                            results = dc.SaveChanges();

                            if (rollback) transaction.Rollback();
                        }
                        else
                        {
                            throw new Exception("Customer was not found.");
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
                        // Check if user is associated with an exisiting appointment or invoice = do not allow delete ....
                        bool inuse = dc.tblAppointments.Any(e => e.CustomerId == id);

                        if (!inuse)
                        {
                            inuse = dc.tblInvoices.Any(c => c.CustomerId == id);
                        }

                        if (inuse && rollback == false)
                        {
                            throw new Exception("This customer is associated with an existing appointment or invoice and therefore cannot be deleted.");
                        }
                        else
                        {
                            IDbContextTransaction transaction = null;
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            tblCustomer deleteRow = dc.tblCustomers.FirstOrDefault(r => r.Id == id);

                            if (deleteRow != null)
                            {
                                dc.tblCustomers.Remove(deleteRow);
                                results = dc.SaveChanges();

                                if (rollback) transaction.Rollback();
                            }
                            else
                            {
                                throw new Exception("Customer was not found.");
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

        public static void ExportExcel(List<Customer> customers)
        {
            try
            {
                string[,] data = new string[customers.Count + 1, 9];
                int counter = 0;

                data[counter, 0] = "First Name";
                data[counter, 1] = "Last Name";
                data[counter, 2] = "Street Address";
                data[counter, 3] = "City";
                data[counter, 4] = "State";
                data[counter, 5] = "ZipCode";
                data[counter, 6] = "Email";
                data[counter, 7] = "Phone";
                data[counter, 8] = "Property Size";

                counter++;

                foreach (Customer c in customers)
                {

                    data[counter, 0] = c.FirstName;
                    data[counter, 1] = c.LastName;
                    data[counter, 2] = c.StreetAddress;
                    data[counter, 3] = c.City;
                    data[counter, 4] = c.State;
                    data[counter, 5] = c.ZipCode;
                    data[counter, 6] = c.Email;
                    data[counter, 7] = c.Phone;
                    data[counter, 8] = c.PropertySqFeet.ToString();


                    counter++;

                }
                string filename = "Customers" + "-" + DateTime.Now.ToString("MM-dd-yyyy");
                Excel.Export(filename, data);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


    }
}
