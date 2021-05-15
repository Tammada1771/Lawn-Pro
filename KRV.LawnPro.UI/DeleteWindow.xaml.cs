using KRV.LawnPro.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KRV.LawnPro.UI
{
    /// <summary>
    /// Interaction logic for DeleteWindow.xaml
    /// </summary>
    public partial class DeleteWindow : Window
    {
        Guid dataToDelete;
        Guid userIdToDelete;
        object valueToDelete;
        LawnProMainWindow _owner;

        public DeleteWindow(Customer customer, object value, LawnProMainWindow owner)
        {

            InitializeComponent();
            dataToDelete = customer.Id;
            userIdToDelete = customer.UserId;
            valueToDelete = value;
            _owner = owner;
            lblItem.Content = value + "?";
            lblData.Content = customer.FullName + ", " + "\n" + customer.FullAddress;
        }
        public DeleteWindow(Employee employee, object value, LawnProMainWindow owner)
        {

            InitializeComponent();
            dataToDelete = employee.Id;
            userIdToDelete = employee.UserId;
            valueToDelete = value;
            _owner = owner;
            lblItem.Content = value + "?";
            lblData.Content = employee.FullName + ", " + "\n" + employee.FullAddress;
        }
        public DeleteWindow(Appointment appointment, object value, LawnProMainWindow owner)
        {
            InitializeComponent();
            dataToDelete = appointment.Id;
            valueToDelete = value;
            _owner = owner;
            lblItem.Content = value + "?";
            lblData.Content = "For " + appointment.CustomerFullName + " on " + "\n" + appointment.FullAddress + " at " + appointment.StartDateTime;
        }
        public DeleteWindow(ServiceType serviceType, object value, LawnProMainWindow owner)
        {
            InitializeComponent();
            dataToDelete = serviceType.Id;
            valueToDelete = value;
            _owner = owner;
            lblItem.Content = value + "?";
            lblData.Content = serviceType.Description;
        }
        public DeleteWindow(Invoice invoice, object value, LawnProMainWindow owner)
        {
            InitializeComponent();
            dataToDelete = invoice.Id;
            valueToDelete = value;
            _owner = owner;
            lblItem.Content = value + "?";
            lblData.Content = "For " + invoice.CustomerFullName + " From " + "\n" + invoice.ServiceDate + ", status is " + invoice.Status;
        }
        public DeleteWindow(User user, object value, LawnProMainWindow owner)
        {
            InitializeComponent();
            dataToDelete = user.Id;
            valueToDelete = value;
            _owner = owner;
            lblItem.Content = value + "?";
            lblData.Content = user.FullName + "With the Username of " + "\n" + user.UserName;
        }

        private static HttpClient InitializeClient()
        {
            HttpClient client = new HttpClient();
            // Will need to update address as needed, start API with UI
            client.BaseAddress = new Uri("https://localhost:44375/");
            return client;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpClient client = InitializeClient();
                HttpResponseMessage response = new HttpResponseMessage();
                string result = "";


                if ("Customer" == valueToDelete.ToString() || "Employee" == valueToDelete.ToString())
                {
                    response = client.DeleteAsync(valueToDelete.ToString() + "/" + dataToDelete).Result;
                    result = response.Content.ReadAsStringAsync().Result;

                    if(result == "1")
                    {
                        response = client.DeleteAsync("User/" + userIdToDelete).Result;
                        result = response.Content.ReadAsStringAsync().Result;

                        if (result == "1")
                        {
                            _owner.RefreshDataGrid();
                            _owner.ChangeStatus("Deleted " + valueToDelete + " and User Successfully.");
                            this.Close();
                        }
                    }
                }
                else
                {
                    response = client.DeleteAsync(valueToDelete.ToString() + "/" + dataToDelete).Result;
                    result = response.Content.ReadAsStringAsync().Result;

                    if (result == "1")
                    {
                        _owner.RefreshDataGrid();
                        _owner.ChangeStatus("Deleted " + valueToDelete + " Successfully.");
                        this.Close();
                    } 
                }
            }
            catch (Exception ex)
            {
                _owner.ChangeStatus(ex.Message);
                this.Close();
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
