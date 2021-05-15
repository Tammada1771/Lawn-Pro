using KRV.LawnPro.Mobile.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class AppointmentsView : ContentPage
    {
        List<Appointment> appointments = new List<Appointment>();
        public AppointmentsView()
        {
            InitializeComponent();
            Title = "Appointments";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (App.SessionUser == null)
            {
                // Session key is empty - User is not logged in
                // Display the login view in a navigatable modal 
                await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
            }
            else
            {
                try
                {
                    if (appointments.Count == 0)
                    {
                        var client = new HttpClient();
                        HttpResponseMessage response = await client.GetAsync(new Uri(App.ApiBaseUri + "/appointment/byEmployeeId/" + App.SessionEmployee.Id));
                        string result = response.Content.ReadAsStringAsync().Result;

                        // will throw an exception if a list of appointments is not returned
                        appointments = JsonConvert.DeserializeObject<List<Appointment>>(result);
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert("Error", "An error occurred loading your appointments.", "Ok");
                }

                List<Appointment> openAppointments = new List<Appointment>();
                foreach (Appointment item in appointments)
                {
                    if (item.Status == "Scheduled" || item.Status == "InProgress")
                    {
                        openAppointments.Add(item);
                    }
                }

                if (App.SessionAppointmentFilter == DateTime.MinValue)
                {
                    lstAppointmentList.ItemsSource = openAppointments.OrderBy(a => a.StartDateTime);
                    btnClearFilter.IsEnabled = false;
                    txtFilterState.Text = "Showing All Active Appointments";
                }
                else
                {
                    var filteredAppointments = appointments.Where(a => a.StartDateTime.ToShortDateString() == App.SessionAppointmentFilter.ToShortDateString()).ToList().OrderBy(a => a.StartDateTime);
                    lstAppointmentList.ItemsSource = filteredAppointments;
                    btnClearFilter.IsEnabled = true;
                    txtFilterState.Text = filteredAppointments.Count().ToString() + " appointments for " + App.SessionAppointmentFilter.Date.ToShortDateString();
                }
            }
        }

        private void appointmentDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            // poulate the list with appointments for date selected 
            lstAppointmentList.ItemsSource = appointments.Where(a => a.StartDateTime.ToShortDateString() == e.NewDate.ToShortDateString() ).ToList().OrderBy(a => a.StartDateTime);
            App.SessionAppointmentFilter = e.NewDate;
        }

        private void lstAppointmentList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new AppointmentDetailView((Appointment)e.SelectedItem));
        }

        private void btnDateFilter_Clicked(object sender, EventArgs e)
        {
            if (App.SessionAppointmentFilter == DateTime.MinValue)
            {
                appointmentDatePicker.Date = DateTime.Now;
            }
            else
            {
                appointmentDatePicker.Date = App.SessionAppointmentFilter;
            }

            appointmentDatePicker.Focus();
        }

        private void btnClearFilter_Clicked(object sender, EventArgs e)
        {
            List<Appointment> openAppointments = new List<Appointment>();
            foreach (Appointment item in appointments)
            {
                if (item.Status == "Scheduled" || item.Status == "InProgress")
                {
                    openAppointments.Add(item);
                }
            }
            lstAppointmentList.ItemsSource = openAppointments.OrderBy(a => a.StartDateTime);
            appointmentDatePicker.Date = DateTime.Now;
            App.SessionAppointmentFilter = new DateTime();
            txtFilterState.Text = "Showing All Active Appointments";
            btnClearFilter.IsEnabled = false;
        }

        private void appointmentDatePicker_Unfocused(object sender, FocusEventArgs e)
        {
            if (appointmentDatePicker.Date == new DateTime(1900, 01, 01))
            {
                // do nothing - cancel was clicked
            }
            else
            {
                // poulate the list with appointments for date selected 
                var filteredAppointments = appointments.Where(a => a.StartDateTime.ToShortDateString() == appointmentDatePicker.Date.ToShortDateString()).ToList().OrderBy(a => a.StartDateTime);
                lstAppointmentList.ItemsSource = filteredAppointments;
                App.SessionAppointmentFilter = appointmentDatePicker.Date;
                btnClearFilter.IsEnabled = true;
                txtFilterState.Text = filteredAppointments.Count().ToString() + " appointments for " + appointmentDatePicker.Date.ToShortDateString();
            }
        }
    }
}