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
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Title = "Main Menu";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (App.SessionUser == null)
            {
                // Session key is empty - User is not logged in
                // Display the login view in a navigatable modal - check if login page already is pushed
                if (Navigation.ModalStack.Count == 0 ||
                    Navigation.ModalStack.Last().GetType() != typeof(LoginPage))
                {
                    var loginPage = new LoginPage();
                    // Subscribe to the event when login completes
                    loginPage.LoginSuccessful += LoginPage_LoginSuccessful;
 
                    await Navigation.PushModalAsync(loginPage);
                }
            }
            else
            {
                lblFullName.Text = App.SessionUser.FullName;
            }
        }

        private void LoginPage_LoginSuccessful(object sender, EventArgs e)
        {
            // Unsubscribe to the event to prevent memory leak
            (sender as LoginPage).LoginSuccessful -= LoginPage_LoginSuccessful;

            lblFullName.Text = App.SessionUser.FullName;
        }


        private async void Logout_Clicked(System.Object sender, System.EventArgs e)
        {

            // Clear the session key
            App.SessionUser = null;
            App.SessionEmployee = null;
            // Display the login view in a navigatable modal
            await Navigation.PushModalAsync(new LoginPage());
        }

        private void Appointments_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AppointmentsView());
        }

        private void MyEmployeeProfile_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MyEmployeeProfileView());
        }

        private void MyUserProfile_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MyUserProfileView());
        }

    }
}