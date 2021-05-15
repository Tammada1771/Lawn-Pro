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
    public partial class MyUserProfileView : ContentPage
    {
        User user;

        public MyUserProfileView()
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
                user = App.SessionUser;
                LoadUser();
            }
        }


        private async void LoadUser()
        {
            try
            {
                txtFirstName.Text = user.FirstName;
                txtLastName.Text = user.LastName;
                txtUserName.Text = user.UserName;
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

                if (txtUserPass.Text == txtUserPass2.Text)
                {
                    user.FirstName = txtFirstName.Text;
                    user.LastName = txtLastName.Text;
                    user.UserName = txtUserName.Text;
                    user.UserPass = txtUserPass.Text;
                    user.UserPass2 = txtUserPass2.Text;

                    var client = new HttpClient();
                    string serializedObject = JsonConvert.SerializeObject(user);
                    var content = new StringContent(serializedObject);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    HttpResponseMessage updateUserResponse = client.PutAsync(new Uri(App.ApiBaseUri + "/user/"), content).Result;
                    string updateUserResult = updateUserResponse.Content.ReadAsStringAsync().Result;

                    if (updateUserResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Update the User in tblUser
                        User user = App.SessionUser;
                        DisplayAlert("Success", "Your user profile has been saved.", "Ok");
                    }
                    else
                    {
                        throw new Exception(updateUserResult);
                    }
                    btnSave.IsEnabled = true;

                }
                else
                {
                    throw new Exception("Passwords do not match.");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
                btnSave.IsEnabled = true;
            }
        }
    }
}