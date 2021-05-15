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
    public static class ServiceTypeManager
    {
        public async static Task<List<ServiceType>> Load()
        {
            try
            {
                List<ServiceType> serviceTypes = new List<ServiceType>();

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        dc.tblServiceTypes
                            .ToList()
                            .ForEach(s => serviceTypes
                            .Add(new ServiceType
                            {
                                Id = s.Id,
                                Description = s.Description,
                                CostPerSQFT = s.CostPerSqFt
                            }));
                    }
                });

                return serviceTypes.OrderBy(s => s.Description).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async static Task<ServiceType> LoadById(Guid id)
        {
            try
            {
                ServiceType serviceType = new ServiceType();

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        tblServiceType row = dc.tblServiceTypes.FirstOrDefault(s => s.Id == id);

                        if(row != null)
                        {
                            serviceType.Id = row.Id;
                            serviceType.Description = row.Description;
                            serviceType.CostPerSQFT = row.CostPerSqFt;
                        }
                        else
                        {
                            throw new Exception("Service type not found");
                        }
                    }
                });

                return serviceType;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async static Task<ServiceType> LoadByDescription(string description)
        {
            try
            {
                ServiceType serviceType = new ServiceType();

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        tblServiceType tblServicetype = dc.tblServiceTypes.FirstOrDefault(s => s.Description == description);

                        if(tblServicetype != null)
                        {
                            serviceType.Id = tblServicetype.Id;
                            serviceType.Description = tblServicetype.Description;
                            serviceType.CostPerSQFT = tblServicetype.CostPerSqFt;
                        }
                        else
                        {
                            throw new Exception("Service type not found");
                        }
                    }
                });

                return serviceType;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async static Task<bool> Insert(ServiceType serviceType, bool rollback = false)
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

                        tblServiceType newrow = new tblServiceType();

                        newrow.Id = Guid.NewGuid();
                        newrow.Description = serviceType.Description;
                        newrow.CostPerSqFt = serviceType.CostPerSQFT;

                        serviceType.Id = newrow.Id;

                        dc.tblServiceTypes.Add(newrow);

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

        public async static Task<int> Update(ServiceType serviceType, bool rollback = false)
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

                        tblServiceType updateRow = dc.tblServiceTypes.FirstOrDefault(s => s.Id == serviceType.Id);

                        if(updateRow != null)
                        {
                            updateRow.Description = serviceType.Description;
                            updateRow.CostPerSqFt = serviceType.CostPerSQFT;

                            dc.tblServiceTypes.Update(updateRow);

                            results = dc.SaveChanges();

                            if (rollback) transaction.Rollback();
                        }
                        else
                        {
                            throw new Exception("Service type not found");
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
                        bool inuse = dc.tblAppointments.Any(e => e.ServiceId == id);

                        if(inuse && rollback == false)
                        {
                            throw new Exception("This service type is associated with an existing appointment and cannot be deleted");
                        }
                        else
                        {
                            IDbContextTransaction transaction = null;
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            tblServiceType deleteRow = dc.tblServiceTypes.FirstOrDefault(s => s.Id == id);

                            if(deleteRow != null)
                            {
                                dc.tblServiceTypes.Remove(deleteRow);
                                results = dc.SaveChanges();
                            }
                            else
                            {
                                throw new Exception("Service type not found");
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

        public static void ExportExcel(List<ServiceType> serviceTypes)
        {
            try
            {
                string[,] data = new string[serviceTypes.Count + 1, 2];
                int counter = 0;

                data[counter, 0] = "Description";
                data[counter, 1] = "Rate";


                counter++;

                foreach (ServiceType st in serviceTypes)
                {

                    data[counter, 0] = st.Description;
                    data[counter, 1] = st.CostPerSQFT.ToString();

                    counter++;

                }
                string filename = "ServiceTypes" + "-" + DateTime.Now.ToString("MM-dd-yyyy");
                Excel.Export(filename, data);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
 
}
