using KRV.LawnPro.BL.Models;
using KRV.LawnPro.PL;
using KRV.LawnPro.Reporting;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KRV.LawnPro.BL
{
    public enum AppointmentStatus
    {
        Unscheduled = 1,
        Scheduled = 2,   
        InProgress = 3,  
        Completed = 4,    
        Canceled = 5    
    }

    public static class AppointmentManager
    {
        public async static Task<List<Appointment>> Load()
        {
            try
            {
                List<Appointment> results = new List<Appointment>();
                
                await Task.Run(() =>
                {

                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        var appointments = (from a in dc.tblAppointments
                                            join c in dc.tblCustomers on a.CustomerId equals c.Id
                                                join e in dc.tblEmployees on a.EmployeeId equals e.Id into es
                                                from e in es.DefaultIfEmpty()
                                            join st in dc.tblServiceTypes on a.ServiceId equals st.Id
                                            orderby a.StartDateTime
                                            descending
                                            select new
                                            {
                                                a.Id,
                                                a.CustomerId,
                                                a.EmployeeId,
                                                a.StartDateTime,
                                                a.EndDateTime,
                                                a.ServiceId,
                                                a.Status,
                                                CustomerFirstName = c.FirstName,
                                                CustomerLastName = c.LastName,
                                                EmployeeFirstName = e.FirstName,
                                                EmployeeLastName = e.LastName,
                                                StreetAddress = c.StreetAddress,
                                                City = c.City,
                                                State = c.State,
                                                ZipCode = c.ZipCode,
                                                Email = c.Email,
                                                Phone = c.Phone,
                                                PropertySqFeet = c.PropertySqFt,
                                                ServiceType = st.Description,
                                                ServiceRate = st.CostPerSqFt
                                            }).ToList();

                            appointments.ForEach(a => results.Add(new Appointment
                            {
                                Id = a.Id,
                                CustomerId = a.CustomerId,
                                EmployeeId = a.EmployeeId,
                                StartDateTime = a.StartDateTime,
                                EndDateTime = a.EndDateTime,
                                ServiceId = a.ServiceId,
                                Status = a.Status,
                                CustomerFirstName = a.CustomerFirstName,
                                CustomerLastName = a.CustomerLastName,
                                EmployeeFirstName = a.EmployeeFirstName,
                                EmployeeLastName = a.EmployeeLastName,
                                StreetAddress = a.StreetAddress,
                                City = a.City,
                                State = a.State,
                                ZipCode = a.ZipCode,
                                Email = a.Email,
                                Phone = a.Phone,
                                PropertySqFeet = a.PropertySqFeet,
                                ServiceType = a.ServiceType,
                                ServiceRate = a.ServiceRate
                            })); 
                    }
                });

                return results.OrderBy(a=>a.StartDateTime).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<Appointment> LoadById(Guid id)
        {
            Appointment result = new Appointment();
            
            try
            {
                if (id != Guid.Empty)
                {
                    await Task.Run(() =>
                    {
                               using (LawnProEntities dc = new LawnProEntities())
                               {
                                   var appointment = (from a in dc.tblAppointments
                                                      join c in dc.tblCustomers on a.CustomerId equals c.Id
                                                      join e in dc.tblEmployees on a.EmployeeId equals e.Id into es
                                                      from e in es.DefaultIfEmpty()
                                                      join st in dc.tblServiceTypes on a.ServiceId equals st.Id
                                                      where a.Id == id
                                                      select new
                                                      {
                                                          a.Id,
                                                          a.CustomerId,
                                                          a.EmployeeId,
                                                          a.StartDateTime,
                                                          a.EndDateTime,
                                                          a.ServiceId,
                                                          a.Status,
                                                          CustomerFirstName = c.FirstName,
                                                          CustomerLastName = c.LastName,
                                                          EmployeeFirstName = e.FirstName,
                                                          EmployeeLastName = e.LastName,
                                                          StreetAddress = c.StreetAddress,
                                                          City = c.City,
                                                          State = c.State,
                                                          ZipCode = c.ZipCode,
                                                          Email = c.Email,
                                                          Phone = c.Phone,
                                                          PropertySqFeet = c.PropertySqFt,
                                                          ServiceType = st.Description,
                                                          ServiceRate = st.CostPerSqFt
                                                      }).FirstOrDefault();

                                   if (appointment != null)
                                   {
                                       result = new Appointment
                                       {
                                           Id = appointment.Id,
                                           CustomerId = appointment.CustomerId,
                                           EmployeeId = appointment.EmployeeId,
                                           StartDateTime = appointment.StartDateTime,
                                           EndDateTime = appointment.EndDateTime,
                                           ServiceId = appointment.ServiceId,
                                           Status = appointment.Status,
                                           CustomerFirstName = appointment.CustomerFirstName,
                                           CustomerLastName = appointment.CustomerLastName,
                                           EmployeeFirstName = appointment.EmployeeFirstName,
                                           EmployeeLastName = appointment.EmployeeLastName,
                                           StreetAddress = appointment.StreetAddress,
                                           City = appointment.City,
                                           State = appointment.State,
                                           ZipCode = appointment.ZipCode,
                                           Email = appointment.Email,
                                           Phone = appointment.Phone,
                                           PropertySqFeet = appointment.PropertySqFeet,
                                           ServiceType = appointment.ServiceType,
                                           ServiceRate = appointment.ServiceRate
                                       };

                                   }
                                   else
                                   {
                                       throw new Exception("Appointment was not found");
                                   }

                               }
                    });

                    return result;
                    
                }
                else
                {
                    throw new Exception("Please provide an id");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async static Task<List<Appointment>> LoadByCustomerId(Guid id)
        {
            List<Appointment> results = new List<Appointment>();

            try
            {
                if (id != Guid.Empty)
                {
                    await Task.Run(() =>
                    {
                        using (LawnProEntities dc = new LawnProEntities())
                        {
                            var appointments = (from a in dc.tblAppointments
                                                join c in dc.tblCustomers on a.CustomerId equals c.Id
                                                join e in dc.tblEmployees on a.EmployeeId equals e.Id into es
                                                from e in es.DefaultIfEmpty()
                                                join st in dc.tblServiceTypes on a.ServiceId equals st.Id
                                                where a.CustomerId == id
                                                orderby a.StartDateTime
                                                descending
                                                select new
                                                {
                                                    a.Id,
                                                    a.CustomerId,
                                                    a.EmployeeId,
                                                    a.StartDateTime,
                                                    a.EndDateTime,
                                                    a.ServiceId,
                                                    a.Status,
                                                    CustomerFirstName = c.FirstName,
                                                    CustomerLastName = c.LastName,
                                                    EmployeeFirstName = e.FirstName,
                                                    EmployeeLastName = e.LastName,
                                                    StreetAddress = c.StreetAddress,
                                                    City = c.City,
                                                    State = c.State,
                                                    ZipCode = c.ZipCode,
                                                    Email = c.Email,
                                                    Phone = c.Phone,
                                                    PropertySqFeet = c.PropertySqFt,
                                                    ServiceType = st.Description,
                                                    ServiceRate = st.CostPerSqFt
                                                }).OrderBy(a=>a.StartDateTime).ToList();

                                appointments.ForEach(a => results.Add(new Appointment
                                {
                                    Id = a.Id,
                                    CustomerId = a.CustomerId,
                                    EmployeeId = a.EmployeeId,
                                    StartDateTime = a.StartDateTime,
                                    EndDateTime = a.EndDateTime,
                                    ServiceId = a.ServiceId,
                                    Status = a.Status,
                                    CustomerFirstName = a.CustomerFirstName,
                                    CustomerLastName = a.CustomerLastName,
                                    EmployeeFirstName = a.EmployeeFirstName,
                                    EmployeeLastName = a.EmployeeLastName,
                                    StreetAddress = a.StreetAddress,
                                    City = a.City,
                                    State = a.State,
                                    ZipCode = a.ZipCode,
                                    Email = a.Email,
                                    Phone = a.Phone,
                                    PropertySqFeet = a.PropertySqFeet,
                                    ServiceType = a.ServiceType,
                                    ServiceRate = a.ServiceRate
                                }));

                        }
                    });

                    return results.OrderBy(a => a.StartDateTime).ToList();

                }
                else
                {
                    throw new Exception("Please provide an id");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async static Task<List<Appointment>> LoadByEmployeeId(Guid id)
        {
            List<Appointment> results = new List<Appointment>();

            try
            {
                if (id != Guid.Empty)
                {
                    await Task.Run(() =>
                    {
                        using (LawnProEntities dc = new LawnProEntities())
                        {
                            var appointments = (from a in dc.tblAppointments
                                                join c in dc.tblCustomers on a.CustomerId equals c.Id
                                                join e in dc.tblEmployees on a.EmployeeId equals e.Id
                                                join st in dc.tblServiceTypes on a.ServiceId equals st.Id
                                                where a.EmployeeId == id
                                                orderby a.StartDateTime
                                                descending
                                                select new
                                                {
                                                    a.Id,
                                                    a.CustomerId,
                                                    a.EmployeeId,
                                                    a.StartDateTime,
                                                    a.EndDateTime,
                                                    a.ServiceId,
                                                    a.Status,
                                                    CustomerFirstName = c.FirstName,
                                                    CustomerLastName = c.LastName,
                                                    EmployeeFirstName = e.FirstName,
                                                    EmployeeLastName = e.LastName,
                                                    StreetAddress = c.StreetAddress,
                                                    City = c.City,
                                                    State = c.State,
                                                    ZipCode = c.ZipCode,
                                                    Email = c.Email,
                                                    Phone = c.Phone,
                                                    PropertySqFeet = c.PropertySqFt,
                                                    ServiceType = st.Description,
                                                    ServiceRate = st.CostPerSqFt
                                                }).ToList();

                            appointments.ForEach(a => results.Add(new Appointment
                            {
                                Id = a.Id,
                                CustomerId = a.CustomerId,
                                EmployeeId = a.EmployeeId,
                                StartDateTime = a.StartDateTime,
                                EndDateTime = a.EndDateTime,
                                ServiceId = a.ServiceId,
                                Status = a.Status,
                                CustomerFirstName = a.CustomerFirstName,
                                CustomerLastName = a.CustomerLastName,
                                EmployeeFirstName = a.EmployeeFirstName,
                                EmployeeLastName = a.EmployeeLastName,
                                StreetAddress = a.StreetAddress,
                                City = a.City,
                                State = a.State,
                                ZipCode = a.ZipCode,
                                Email = a.Email,
                                Phone = a.Phone,
                                PropertySqFeet = a.PropertySqFeet,
                                ServiceType = a.ServiceType,
                                ServiceRate = a.ServiceRate
                            }));

                        }
                    });

                    return results.OrderBy(a => a.StartDateTime).ToList();

                }
                else
                {
                    throw new Exception("Please provide an id");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<List<Appointment>> LoadByStatus(string status)
        {
            try
            {
                List<Appointment> appointments = new List<Appointment>();

                await Task.Run(() =>
                {

                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        dc.tblAppointments
                            .Where(a => a.Status == status)
                            .ToList()
                            .ForEach(a => appointments
                            .Add(new Appointment
                            {
                                Id = a.Id,
                                CustomerId = a.CustomerId,
                                EmployeeId = a.EmployeeId,
                                StartDateTime = a.StartDateTime,
                                EndDateTime = a.EndDateTime,
                                ServiceId = a.ServiceId,
                                Status = a.Status
                            }));
                    }
                });

                return appointments.OrderBy(a => a.StartDateTime).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async static Task<bool> Insert(Appointment appointment, bool rollback = false)
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

                        tblAppointment newRow = new tblAppointment();

                        newRow.Id = Guid.NewGuid();
                        newRow.CustomerId = appointment.CustomerId;
                        newRow.EmployeeId = appointment.EmployeeId;
                        newRow.StartDateTime = appointment.StartDateTime;
                        newRow.EndDateTime = appointment.EndDateTime;
                        newRow.ServiceId = appointment.ServiceId;
                        newRow.Status = appointment.Status;

                        // Backfill the id on the input parameter appointment
                        appointment.Id = newRow.Id;

                        // Insert the row
                        dc.tblAppointments.Add(newRow);

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


        public async static Task<int> Update(Appointment appointment, bool rollback = false)
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

                        tblAppointment updateRow = dc.tblAppointments.FirstOrDefault(r => r.Id == appointment.Id);

                        if (updateRow != null)
                        {
                            updateRow.CustomerId = appointment.CustomerId;
                            updateRow.EmployeeId = appointment.EmployeeId;
                            updateRow.StartDateTime = appointment.StartDateTime;
                            updateRow.EndDateTime = appointment.EndDateTime;
                            updateRow.ServiceId = appointment.ServiceId;
                            updateRow.Status = appointment.Status;

                            dc.tblAppointments.Update(updateRow);

                            // Commit the changes and get the number of rows affected
                            results = dc.SaveChanges();

                            if (appointment.Status == AppointmentStatus.Completed.ToString())
                            {
                                GenerateInvoice(appointment);
                            }

                            if (rollback) transaction.Rollback();
                        }
                        else
                        {
                            throw new Exception("Appointment was not found.");
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
                        // Check if appointment is in progress or completed ....
                        bool inuse = dc.tblAppointments.Any(a => a.Id == id && a.Status == "InProgress" || a.Status == "Complete" );

                        if (inuse && rollback == false)
                        {
                            throw new Exception("This appointment is currently in progress or completed and therefore cannot be deleted.");
                        }
                        else
                        {
                            IDbContextTransaction transaction = null;
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            tblAppointment deleteRow = dc.tblAppointments.FirstOrDefault(r => r.Id == id);

                            if (deleteRow != null)
                            {
                                dc.tblAppointments.Remove(deleteRow);
                                results = dc.SaveChanges();

                                if (rollback) transaction.Rollback();
                            }
                            else
                            {
                                throw new Exception("Appointment was not found.");
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

        private static async void GenerateInvoice(Appointment appointment)
        {
            try
            {
                Invoice invoice = new Invoice
                {
                    CustomerId = appointment.CustomerId,
                    EmployeeFullName = appointment.EmployeeFullName,
                    ServiceDate = DateTime.Now,
                    ServiceType = appointment.ServiceType,
                    ServiceRate = appointment.ServiceRate,
                    Status = InvoiceStatus.Issued.ToString()
                };

                await InvoiceManager.Insert(invoice);

                InvoiceManager.EmailInvoice(appointment.Email, await InvoiceManager.LoadById(invoice.Id));

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void ExportExcel(List<Appointment> appointments)
        {
            try
            {
                string[,] data = new string[appointments.Count + 1, 6];
                int counter = 0;

                data[counter, 0] = "Customer";
                data[counter, 1] = "Employee";
                data[counter, 2] = "Start Time";
                data[counter, 3] = "End Times";
                data[counter, 4] = "Service";
                data[counter, 5] = "Status";

                counter++;

                foreach(Appointment a in appointments)
                {

                        data[counter, 0] = a.CustomerFullName;
                        data[counter, 1] = a.EmployeeFullName;
                        data[counter, 2] = a.StartDateTime.ToString();
                        data[counter, 3] = a.EndDateTime.ToString();
                        data[counter, 4] = a.ServiceType;
                        data[counter, 5] = a.Status;

                        counter++;

                }
                string filename = "Appointments" + "-" + DateTime.Now.ToString("MM-dd-yyyy");

                Excel.Export(filename, data);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }



    }
}
