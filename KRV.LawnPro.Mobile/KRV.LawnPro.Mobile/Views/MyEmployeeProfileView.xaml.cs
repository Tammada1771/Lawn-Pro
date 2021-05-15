using KRV.LawnPro.Mobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KRV.LawnPro.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyEmployeeProfileView : ContentPage
    {
        Employee employee;

        public MyEmployeeProfileView()
        {
            InitializeComponent();
            Title = "My Profile";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (App.SessionUser == null)
            {
                // Session key is empty - User is not logged in
                // Display the login view in a navigatable modal 
                Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
            }
            else
            {
                employee = App.SessionEmployee;
                LoadEmployee();
            }
        }


        private async void LoadEmployee()
        {
            try
            {
                txtFirstName.Text = employee.FirstName;
                txtLastName.Text = employee.LastName;
                txtAddress.Text = employee.StreetAddress;
                txtCity.Text = employee.City;
                txtState.Text = employee.State;
                txtZipCode.Text = employee.ZipCode;
                txtEmail.Text = employee.Email;
                txtPhone.Text = employee.Phone;
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "An error occurred loading your profile.", "Ok");
            }
        }


        private void SaveProfile_Clicked(object sender, EventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                employee.FirstName = txtFirstName.Text;
                employee.LastName = txtLastName.Text;
                employee.StreetAddress = txtAddress.Text;
                employee.City = txtCity.Text;
                employee.State = txtState.Text;
                employee.ZipCode = txtZipCode.Text;
                employee.Email = txtEmail.Text;
                employee.Phone = txtPhone.Text;

                var client = new HttpClient();
                string serializedObject = JsonConvert.SerializeObject(employee);
                var content = new StringContent(serializedObject);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                HttpResponseMessage updateEmployeeResponse = client.PutAsync(new Uri(App.ApiBaseUri + "/employee/"), content).Result;
                string updateEmployeeResult = updateEmployeeResponse.Content.ReadAsStringAsync().Result;

                if (updateEmployeeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // update the user first name and last name
                    User user = App.SessionUser;

                    user.FirstName = txtFirstName.Text;
                    user.LastName = txtLastName.Text;
                    user.UserPass2 = user.UserPass;

                    client = new HttpClient();
                    serializedObject = JsonConvert.SerializeObject(user);
                    content = new StringContent(serializedObject);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    bool nameonly = true;
                    HttpResponseMessage updateUserResponse = client.PutAsync(new Uri(App.ApiBaseUri + "/user/" + nameonly + "/"), content).Result;
                    string updateUserResult = updateUserResponse.Content.ReadAsStringAsync().Result;

                    if (updateUserResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        App.SessionUser = user;
                        App.SessionEmployee = employee;
                        DisplayAlert("Success", "Your employee profile has been saved.", "Ok");
                    }
                }
                else
                {
                    throw new Exception(updateEmployeeResult);
                }
                btnSave.IsEnabled = true;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
                btnSave.IsEnabled = true;
            }
        }
    }
}