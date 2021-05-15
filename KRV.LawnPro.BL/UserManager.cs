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
    public static class UserManager
    {
        private static string GetHash(string userpass)
        {
            using (var hasher = new System.Security.Cryptography.SHA1Managed())
            {
                var hashbytes = System.Text.Encoding.UTF8.GetBytes(userpass);
                return Convert.ToBase64String(hasher.ComputeHash(hashbytes));
            }
        }

        public static async Task<bool> Seed()
        {
            List<User> users = new List<User>();
            await Task.Run(() =>
            {
                using (LawnProEntities dc = new LawnProEntities())
                {
                    dc.tblUsers
                        .ToList()
                        .ForEach(u => users
                        .Add(new User
                        {
                            Id = u.Id,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            UserName = u.UserName,
                            UserPass = u.UserPass
                        }));

                    foreach (User user in users)
                    {
                        if (user.UserPass.Length != 28)
                        {
                            tblUser upDateRow = dc.tblUsers.FirstOrDefault(r => r.Id == user.Id);

                            if (upDateRow != null)
                            {
                                upDateRow.UserPass = GetHash(user.UserPass.Trim());
                            }

                            dc.tblUsers.Update(upDateRow);

                            // Commit the changes and get the number of rows affected
                            dc.SaveChanges();
                        }

                    }
                }
            });

            return true;
        }

        public async static Task<User> Login(User user)
        {
            try {

                await Task.Run(() =>
                {
                    if (!string.IsNullOrEmpty(user.UserName))
                    {
                        if (!string.IsNullOrEmpty(user.UserPass))
                        {
                            using (LawnProEntities dc = new LawnProEntities())
                            {
                                tblUser userrow = dc.tblUsers.FirstOrDefault(u => u.UserName == user.UserName);

                                if (userrow != null)
                                {
                                    // check the password
                                    if (userrow.UserPass == GetHash(user.UserPass))
                                    {
                                        // Login was successfull
                                        user.Id = userrow.Id;
                                        user.FirstName = userrow.FirstName;
                                        user.LastName = userrow.LastName;
                                        user.UserName = userrow.UserName;
                                        user.UserPass = userrow.UserPass;
                                    }
                                    else
                                    {
                                        throw new Exception("Invalid credentials.");
                                    }
                                }
                                else
                                {
                                    throw new Exception("User not found.");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Password is required.");
                        }
                    }
                    else
                    {
                        throw new Exception("Username is required.");
                    }
                });

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<List<User>> Load()
        {
            try
            {
                List<User> users = new List<User>();

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        dc.tblUsers
                            .ToList()
                            .ForEach(u => users
                            .Add(new User
                            {
                                Id = u.Id,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                UserName = u.UserName,
                                UserPass = u.UserPass
                            }));
                    }
                });

                return users;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<User> LoadById(Guid id)
        {
            try
            {
                User user = new User();

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        user = (from u in dc.tblUsers
                                where u.Id == id
                                select new User
                                {
                                    Id = u.Id,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    UserName = u.UserName,
                                    UserPass = u.UserPass
                                }).FirstOrDefault();
                    }
                });

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<bool> Insert(User user, bool rollback = false)
        {
            try
            {
                int result = 0;

                if(user.Id.Equals(user.Id.ToString()))
                {
                    user.Id = (Guid)user.Id;
                }

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        // Check if username already exists - do not allow ....
                        bool inuse = dc.tblUsers.Any(u => u.UserName.Trim().ToUpper() == user.UserName.Trim().ToUpper());

                        if (inuse)
                        {
                            throw new Exception("This User Name already exists.");
                        }
                        else
                        {
                            IDbContextTransaction transaction = null;
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            tblUser newUser = new tblUser();

                            newUser.Id = Guid.NewGuid();
                            newUser.FirstName = user.FirstName.Trim();
                            newUser.LastName = user.LastName.Trim();
                            newUser.UserName = user.UserName.Trim();
                            newUser.UserPass = GetHash(user.UserPass.Trim());

                            user.Id = newUser.Id;

                            dc.tblUsers.Add(newUser);

                            result = dc.SaveChanges();

                            if (rollback) transaction.Rollback();
                        }
                    }
                });

                return result == 1;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async static Task<int> Update(User user, bool nameonly = false, bool rollback = false)
        {
            try
            {
                int results = 0;

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        // Check if username already exists - do not allow ....
                        bool inuse = dc.tblUsers.Any(u => u.UserName.Trim().ToUpper() == user.UserName.Trim().ToUpper() && u.Id != user.Id);

                        if (inuse)
                        {
                            throw new Exception("This User Name already exists.");
                        }
                        else
                        {
                            IDbContextTransaction transaction = null;
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            tblUser upDateRow = dc.tblUsers.FirstOrDefault(r => r.Id == user.Id);

                            if (upDateRow != null)
                            {
                                upDateRow.FirstName = user.FirstName.Trim();
                                upDateRow.LastName = user.LastName.Trim();

                                if (!nameonly)
                                {
                                    upDateRow.UserName = user.UserName.Trim();
                                    upDateRow.UserPass = GetHash(user.UserPass.Trim());
                                }

                                dc.tblUsers.Update(upDateRow);

                                // Commit the changes and get the number of rows affected
                                results = dc.SaveChanges();

                                if (rollback) transaction.Rollback();
                            }
                            else
                            {
                                throw new Exception("User was not found.");
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


        public async static Task<int> Delete(Guid id, bool rollback = false)
        {
            try
            {
                int results = 0;

                await Task.Run(() =>
                {
                    using (LawnProEntities dc = new LawnProEntities())
                    {
                        // Check if user is associated with an exisiting customers or employees = do not allow delete ....
                        bool inuse = dc.tblEmployees.Any(e => e.UserId == id);

                        if (!inuse)
                        {
                            inuse = dc.tblCustomers.Any(c => c.UserId == id);
                        }

                        if (inuse)
                        {
                            throw new Exception("This user is associated with an existing Customers or Employee and therefore cannot be deleted.");
                        }
                        else
                        {
                            IDbContextTransaction transaction = null;
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            tblUser deleteRow = dc.tblUsers.FirstOrDefault(r => r.Id == id);

                            if (deleteRow != null)
                            {
                                dc.tblUsers.Remove(deleteRow);

                                // Commit the changes and get the number of rows affected
                                results = dc.SaveChanges();

                                if (rollback) transaction.Rollback();
                            }
                            else
                            {
                                throw new Exception("User was not found.");
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

        public static void ExportExcel(List<User> users)
        {
            try
            {
                string[,] data = new string[users.Count + 1, 3];
                int counter = 0;

                data[counter, 0] = "First Name";
                data[counter, 1] = "Last Name";
                data[counter, 2] = "Username";

                counter++;

                foreach (User u in users)
                {

                    data[counter, 0] = u.FirstName;
                    data[counter, 1] = u.LastName;
                    data[counter, 2] = u.UserName;

                    counter++;

                }
                string filename = "Users" + "-" + DateTime.Now.ToString("MM-dd-yyyy");
                Excel.Export(filename, data);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
