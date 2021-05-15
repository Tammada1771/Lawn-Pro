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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {

            InitializeComponent();
            Title = "Login";
        }

        // Add an event to notify when Login completes successfully
        public event EventHandler<EventArgs> LoginSuccessful;


        private async void Login_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                btnLogin.IsEnabled = false;

                // Vaildate the user entries
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    await DisplayAlert("Error", "Please enter a Username", "Ok");
                    btnLogin.IsEnabled = true;
                    return;
                }
                else if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    await DisplayAlert("Error", "Please enter a Password", "Ok");
                    btnLogin.IsEnabled = true;
                    return;
                }

                User user = new User
                {
                    FirstName = "unknown",
                    LastName = "unknown",
                    UserName = txtUsername.Text,
                    UserPass = txtPassword.Text,
                    UserPass2 = txtPassword.Text
                };

                var client = new HttpClient();
                string serializedObject = JsonConvert.SerializeObject(user);
                var content = new StringContent(serializedObject);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage userResponse = await client.PostAsync(new Uri(App.ApiBaseUri + "/user/login/"), content);
                string userResult = userResponse.Content.ReadAsStringAsync().Result;

                // check for a vaild user
                if (userResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    user = JsonConvert.DeserializeObject<User>(userResult);
                    
                    // Load the employee into session
                    HttpResponseMessage employeeResponse = await client.GetAsync(new Uri(App.ApiBaseUri + "/employee/byUserId/" + user.Id));
                    string employeeResult = employeeResponse.Content.ReadAsStringAsync().Result;

                    if (employeeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Employee employee = JsonConvert.DeserializeObject<Employee>(employeeResult);

                        // Save the user  and employee in session
                        App.SessionUser = user;
                        App.SessionEmployee = employee;

                        // Close the modal
                        LoginSuccessful?.Invoke(this, EventArgs.Empty);
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", employeeResult, "Ok");
                        btnLogin.IsEnabled = true;
                        return;
                    }
                }
                else
                {
                    await DisplayAlert("Error", userResult, "Ok");
                    btnLogin.IsEnabled = true;
                    return;
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Sorry, an error occurred trying to log in.", "Ok");
                btnLogin.IsEnabled = true;
            }
        }




    }
}