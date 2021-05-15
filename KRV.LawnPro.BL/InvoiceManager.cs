using KRV.LawnPro.BL.Models;
using KRV.LawnPro.PL;
using KRV.LawnPro.Reporting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KRV.LawnPro.BL
{

    public enum InvoiceStatus
    {
        Draft = 1,   // The invoice is still open and in progress
        Issued = 2,  // The invoice has been finalized and sent to the client
        Paid = 3,    // The client has paid the invoice
        Void = 4     // The invoice has been voided
    }


    public static class InvoiceManager
    {
        public async static Task<List<Invoice>> Load()
        {
            try
            {
                List<Invoice> results = new List<Invoice>();

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        var invoices = (from i in dc.tblInvoices
                                    join c in dc.tblCustomers on i.CustomerId equals c.Id
                                    select new Invoice
                                    {
                                        Id = i.Id,
                                        CustomerId = i.CustomerId,
                                        CustomerFirstName = c.FirstName,
                                        CustomerLastName = c.LastName,
                                        CustomerStreetAddress = c.StreetAddress,
                                        CustomerCity = c.City,
                                        CustomerState = c.State,
                                        CustomerZip = c.ZipCode,
                                        CustomerEmail = c.Email,
                                        EmployeeFullName = i.EmployeeName,
                                        PropertySqFt = c.PropertySqFt,
                                        ServiceDate = i.ServiceDate,
                                        ServiceType = i.ServiceName,
                                        ServiceRate = i.ServiceRate,
                                        Status = i.Status,
                                        CustomerFullName = c.FirstName + " " + c.LastName
                                    }).ToList();

                        invoices.ForEach(a => results.Add(new Invoice
                        {
                            Id = a.Id,
                            CustomerId = a.CustomerId,
                            CustomerStreetAddress = a.CustomerStreetAddress,
                            CustomerCity = a.CustomerCity,
                            CustomerState = a.CustomerState,
                            CustomerZip = a.CustomerZip,
                            FullAddress = a.CustomerStreetAddress + ", " + a.CustomerCity + ", " + a.CustomerState + " " + a.CustomerZip,
                            ServiceType = a.ServiceType,
                            CustomerEmail = a.CustomerEmail,
                            EmployeeFullName = a.EmployeeFullName,
                            CustomerFirstName = a.CustomerFirstName,
                            CustomerLastName = a.CustomerLastName,
                            CustomerFullName = a.CustomerFirstName + " " + a.CustomerLastName,
                            PropertySqFt = a.PropertySqFt,
                            ServiceDate = a.ServiceDate,
                            Status = a.Status,
                            ServiceRate = a.ServiceRate
                        }));
                    }
                });

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async static Task<Invoice> LoadById(Guid id)
        {
            try
            {
                Invoice invoice = new Invoice();

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        invoice = (from i in dc.tblInvoices
                                   join c in dc.tblCustomers on i.CustomerId equals c.Id
                                   where i.Id == id
                                   select new Invoice
                                   {
                                       Id = i.Id,
                                       CustomerId = i.CustomerId,
                                       CustomerFirstName = c.FirstName,
                                       CustomerLastName = c.LastName,
                                       CustomerStreetAddress = c.StreetAddress,
                                       CustomerCity = c.City,
                                       CustomerState = c.State,
                                       CustomerZip = c.ZipCode,
                                       CustomerEmail = c.Email,
                                       EmployeeFullName = i.EmployeeName,
                                       PropertySqFt = c.PropertySqFt,
                                       ServiceDate = i.ServiceDate,
                                       ServiceType = i.ServiceName,
                                       ServiceRate = i.ServiceRate,
                                       Status = i.Status
                                   }).FirstOrDefault();

                        if (invoice == null)
                        {
                            throw new Exception("Invoice not found");
                        }
                    }
                });

                return invoice;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async static Task<List<Invoice>> LoadByCustomerId(Guid id)
        {
            try
            {
                List<Invoice> invoices = new List<Invoice>();

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        invoices = (from i in dc.tblInvoices
                                    join c in dc.tblCustomers on i.CustomerId equals c.Id
                                    where c.Id == id
                                    select new Invoice
                                    {
                                        Id = i.Id,
                                        CustomerId = i.CustomerId,
                                        CustomerFirstName = c.FirstName,
                                        CustomerLastName = c.LastName,
                                        CustomerStreetAddress = c.StreetAddress,
                                        CustomerCity = c.City,
                                        CustomerState = c.State,
                                        CustomerZip = c.ZipCode,
                                        CustomerEmail = c.Email,
                                        EmployeeFullName = i.EmployeeName,
                                        PropertySqFt = c.PropertySqFt,
                                        ServiceDate = i.ServiceDate,
                                        ServiceType = i.ServiceName,
                                        ServiceRate = i.ServiceRate,
                                        Status = i.Status
                                    }).ToList();
                    }
                });

                return invoices;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async static Task<bool> Insert(Invoice invoice, bool rollback = false)
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

                        tblInvoice newrow = new tblInvoice();

                        newrow.Id = Guid.NewGuid();
                        newrow.CustomerId = invoice.CustomerId;
                        newrow.EmployeeName = invoice.EmployeeFullName;
                        newrow.ServiceDate = invoice.ServiceDate;
                        newrow.ServiceName = invoice.ServiceType;
                        newrow.ServiceRate = invoice.ServiceRate;
                        newrow.Status = invoice.Status;

                        invoice.Id = newrow.Id;

                        dc.tblInvoices.Add(newrow);

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

        public async static Task<int> Update(Invoice invoice, bool rollback = false)
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

                        tblInvoice updateRow = dc.tblInvoices.FirstOrDefault(s => s.Id == invoice.Id);

                        if (updateRow != null)
                        {
                            updateRow.EmployeeName = invoice.EmployeeFullName;
                            updateRow.ServiceDate = invoice.ServiceDate;
                            updateRow.ServiceName = invoice.ServiceType;
                            updateRow.ServiceRate = invoice.ServiceRate;
                            updateRow.Status = invoice.Status;

                            dc.tblInvoices.Update(updateRow);

                            results = dc.SaveChanges();

                            if (rollback) transaction.Rollback();
                        }
                        else
                        {
                            throw new Exception("Invoice not found");
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
                        bool inuse = dc.tblInvoices.Any(e => e.Id == id && e.Status != "Paid");

                        if (inuse && rollback == false)
                        {
                            throw new Exception("This Invoice has not been paid and cannot be deleted");
                        }
                        else
                        {
                            IDbContextTransaction transaction = null;
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            tblInvoice deleteRow = dc.tblInvoices.FirstOrDefault(s => s.Id == id);

                            if (deleteRow != null)
                            {
                                dc.tblInvoices.Remove(deleteRow);
                                results = dc.SaveChanges();
                            }
                            else
                            {
                                throw new Exception("Invoice not found");
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

        public static void EmailInvoice(string emailAddress, Invoice invoice)
        {
            try
            {
                // Generate the invoice
                string filename = "Invoice-" + invoice.CustomerLastName + "-" + invoice.ServiceDate.ToString("MM-dd-yyyy");
                CreateInvoicePDF(filename, invoice.Id);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("lawnproservicesllc@gmail.com");
                mail.To.Add("vicchiollo@yahoo.com");
                mail.Subject = "Lawn Pro Service - " + invoice.ServiceType + " on " + invoice.ServiceDate.ToShortDateString();
                mail.Body = "Your invoice for todays service is attached.  Thank you!";

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(Path.GetTempPath() + filename + ".pdf");
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("lawnproservicesllc@gmail.com", "lawn_pro_1");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void CreateInvoicePDF(string filename, Guid invoiceId)
        {
            try
            {
                var task = InvoiceManager.LoadById(invoiceId);
                Invoice invoice = task.Result;
                task.Wait();


                PDF.Export(filename, invoice);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void InvoicePDF(string filename, Invoice invoice)
        {
            try
            {
                PDF.Export(filename, invoice);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static void ExportExcel(List<Invoice> invoices)
        {
            try
            {
                string[,] data = new string[invoices.Count + 1, 6];
                int counter = 0;

                data[counter, 0] = "Customer";
                data[counter, 1] = "Service";
                data[counter, 2] = "Rate";
                data[counter, 3] = "Date";
                data[counter, 4] = "Employee";
                data[counter, 5] = "Status";

                counter++;

                foreach (Invoice a in invoices)
                {

                    data[counter, 0] = a.CustomerFullName;
                    data[counter, 1] = a.ServiceType;
                    data[counter, 2] = a.ServiceRate.ToString();
                    data[counter, 3] = a.ServiceDate.ToString();
                    data[counter, 4] = a.EmployeeFullName;
                    data[counter, 5] = a.Status;

                    counter++;

                }
                string filename = "Invoices" + "-" + DateTime.Now.ToString("MM-dd-yyyy");
                Excel.Export(filename, data);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static async Task<decimal> GetBalance(Guid customerId)
        {
            try
            {
                decimal balance = 0;

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        var parameters = new SqlParameter
                        {
                            ParameterName = "customerId",
                            SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                            Value = customerId
                        };

                        var results = dc.Set<spGetCustomerBalanceResult>().FromSqlRaw("exec spGetCustomerBalance @customerId", parameters);

                        foreach (var r in results)
                        {
                            balance = r.Balance;
                        }
                    }
                });

                return balance;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
