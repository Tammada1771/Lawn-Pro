using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KRV.LawnPro.Mobile.Views;
using KRV.LawnPro.Mobile.Models;

namespace KRV.LawnPro.Mobile
{
    public partial class App : Application
    {
        // Create a session variable for the application
        public static User SessionUser = null;
        public static Employee SessionEmployee = null;
        public static DateTime SessionAppointmentFilter = new DateTime();

        public static string ApiBaseUri = "https://7d9ea230a4b3.ngrok.io";

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
