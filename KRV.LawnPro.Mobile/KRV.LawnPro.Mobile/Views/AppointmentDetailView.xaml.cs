using KRV.LawnPro.Mobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KRV.LawnPro.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentDetailView : ContentPage
    {
        Appointment _appointment;

        public AppointmentDetailView(Appointment appointment)
        {
            InitializeComponent();
            Title = "Appointment Details";
            _appointment = appointment;
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
                txtCustomerName.Text = _appointment.CustomerFullName;
                btnAddress.Text = _appointment.FullAddress;
                btnEmail.Text = _appointment.Email;
                btnPhone.Text = _appointment.Phone;
                txtServiceType.Text = _appointment.ServiceType;
                txtDuration.Text = _appointment.AppointmentLength.ToString();
                txtStatus.Text = _appointment.StatusDetail;

                switch (_appointment.Status)
                {
                    case "Scheduled":
                        btnUpdateStatus.Text = "Mark In Progress";
                        btnUpdateStatus.IsEnabled = true;
                        txtStatus.Text = "SCHEDULED";
                        txtStatus.TextColor = Color.Red;
                        break;
                    case "InProgress":
                        btnUpdateStatus.Text = "Mark Completed";
                        btnUpdateStatus.IsEnabled = true;
                        txtStatus.Text = "IN PROGRESS";
                        txtStatus.TextColor = Color.Yellow;
                        break;
                    case "Completed":
                        btnUpdateStatus.Text = "COMPLETED";
                        btnUpdateStatus.IsEnabled = false;
                        txtStatus.TextColor = Color.Black;
                        txtStatus.FontAttributes = FontAttributes.Bold;
                        txtStatus.Text = "COMPLETED";
                        break;
                    case "Canceled":
                        btnUpdateStatus.Text = "CANCELED";
                        btnUpdateStatus.IsEnabled = false;
                        txtStatus.TextColor = Color.Black;
                        txtStatus.Text = "CANCELED";
                        break;
                    default:
                        break;
                }
            }
        }

        private async void btnUpdateStatus_Clicked(object sender, EventArgs e)
        {
            try
            {
                // update the appointmet status
                switch (_appointment.Status)
                {
                    case "Scheduled":
                        _appointment.Status = "InProgress";
                        _appointment.StartDateTime = DateTime.Now;
                        txtStatus.Text = "IN PROGRESS";
                        txtStatus.TextColor = Color.Yellow;
                        btnUpdateStatus.Text = "Mark Completed";
                        btnUpdateStatus.IsEnabled = true;
                        break;
                    case "InProgress":
                        _appointment.Status = "Completed";
                        _appointment.EndDateTime = DateTime.Now;
                        txtStatus.TextColor = Color.Black;
                        txtStatus.FontAttributes = FontAttributes.Bold;
                        txtStatus.Text = "COMPLETED";
                        btnUpdateStatus.IsEnabled = false;

                        string message = "Thank you for choosing Lawn Pro Services LLC.  We would like you to know that we have completed your service today.";
                        string recipient = _appointment.Phone;

                        // Send an SMS to customer
                        await SendSms(message, recipient);

                        break;
                    default:
                        return;
                }

                var client = new HttpClient();
                string serializedObject = JsonConvert.SerializeObject(_appointment);
                var content = new StringContent(serializedObject);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                HttpResponseMessage updateResponse = await client.PutAsync(new Uri(App.ApiBaseUri + "/appointment/"), content);
                string updateResult = updateResponse.Content.ReadAsStringAsync().Result;

                if (updateResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await DisplayAlert("Error", "Sorry, an error occured updating the appointment status.", "Ok");
                }
            }
            catch (Exception)
            { 
                await DisplayAlert("Error", "Sorry, an error occured.", "Ok");
            }
            finally
            {
                btnUpdateStatus.IsEnabled = true;
            }
        }

        private async void btnNavigate_Clicked(object sender, EventArgs e)
        {
            btnAddress.IsEnabled = false;
            try
            {
                var locations = await Geocoding.GetLocationsAsync(_appointment.FullAddress);
                var location = locations?.FirstOrDefault();

                if (location == null) return;

                var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
                await Map.OpenAsync(location, options);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Error", "Sorry, this feature not supported on device.", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Sorry, an error occured.", "Ok");
            }
            finally
            {
                btnAddress.IsEnabled = true;
            }
        }

        private async void btnEmail_Clicked(object sender, EventArgs e)
        {
            btnEmail.IsEnabled = false;
            try
            {
                var message = new EmailMessage
                {
                    Subject = "LawnPro Service",
                    To = new List<string> { _appointment.Email }
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                await DisplayAlert("Error", "Sorry, this feature not supported on device.", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Sorry, an error occured.", "Ok");
            }
            finally
            {
                btnEmail.IsEnabled = true;
            }

        }

        private void btnPhone_Clicked(object sender, EventArgs e)
        {
            btnPhone.IsEnabled = false;
            try
            {
                PhoneDialer.Open(_appointment.Phone);
            }
            catch (ArgumentNullException anEx)
            {
                DisplayAlert("Error", "Invalid phone number.", "Ok");
            }
            catch (FeatureNotSupportedException ex)
            {
                DisplayAlert("Error", "Sorry, this feature not supported on device.", "Ok");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Sorry, an error occured.", "Ok");
            }
            finally
            {
                btnPhone.IsEnabled = true;
            }
        }

        private async Task SendSms(string messageText, string recipient)
        {
            try
            {
                var message = new SmsMessage(messageText, new[] { recipient });
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
    }
}
