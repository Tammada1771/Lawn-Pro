using KRV.LawnPro.BL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using Xceed.Wpf.Toolkit;

namespace KRV.LawnPro.UI
{
    public enum AppointmentStatus
    {
        Unscheduled = 1,
        Scheduled = 2,
        InProgress = 3,
        Completed = 4,
        Canceled = 5
    }

    /// <summary>
    /// Interaction logic for MaintainAppointments.xaml
    /// </summary>
    public partial class MaintainAppointments : Window
    {
        Appointment appointment;
        Guid appointmentId;
        Guid? employeeId;
        Guid serviceId;
        Guid customerId;
        LawnProMainWindow _owner;

        public MaintainAppointments(LawnProMainWindow owner)
        {
            InitializeComponent();
            _owner = owner;
            MoveButtons(btnAdd, 0);
            LoadScreen();
            cboCustomer.Focus();

            lblTitle.Content = "Appointment";
            this.Title = "Maintain Appointment";
        }

        public MaintainAppointments(LawnProMainWindow owner, Appointment appointment)
        {
            InitializeComponent();
            _owner = owner;
            MoveButtons(btnUpdate, 0);
            LoadScreen();
            FillTextBoxes(appointment);

            if (appointment.EmployeeId.HasValue)
            {
                employeeId = (Guid)appointment.EmployeeId;   
            }
            serviceId = appointment.ServiceId;
            customerId = appointment.CustomerId;
            appointmentId = appointment.Id;

            lblTitle.Content = "Appointment";
            this.Title = "Maintain Appointment";
        }

        public MaintainAppointments(LawnProMainWindow owner, Appointment appointment, bool magnify)
        {
            InitializeComponent();
            _owner = owner;
            btnAdd.Visibility = Visibility.Hidden;
            LoadScreen();
            FillTextBoxes(appointment);
            JustMagnify();

            lblTitle.Content = "Appointment";
            this.Title = "Inspect Appointment";
        }

        private void MoveButtons(Button button, int margin)
        {
            if (button == btnAdd)
            {
                Thickness m = btnAdd.Margin;
                m.Left = margin;
                btnAdd.Margin = m;

                btnUpdate.Visibility = Visibility.Hidden;
                btnAdd.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if (button == btnUpdate)
            {
                Thickness m = btnUpdate.Margin;
                m.Right = margin;
                btnUpdate.Margin = m;

                btnAdd.Visibility = Visibility.Hidden;
                btnUpdate.HorizontalAlignment = HorizontalAlignment.Center;
            }

        }

        public class Combo
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private IEnumerable<Combo> MakeStatusComboBox()
        {
            var datasource = new List<Combo>();
            datasource.Add(new Combo { Name = "Canceled", Value = "Canceled" });
            datasource.Add(new Combo { Name = "Completed", Value = "Completed" });
            datasource.Add(new Combo { Name = "In Progress", Value = "InProgress" });
            datasource.Add(new Combo { Name = "Scheduled", Value = "Scheduled" });
            datasource.Add(new Combo { Name = "Completed", Value = "Completed" });
            IEnumerable<Combo> result = datasource.OrderBy(datasource => datasource.Name);
            return result;
        }


        private static HttpClient InitializeClient()
        {
            HttpClient client = new HttpClient();
            // Will need to update address as needed, start API with UI
            client.BaseAddress = new Uri("https://localhost:44375/");
            return client;
        }

        private void JustMagnify()
        {
            btnAdd.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnAdd.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Hidden;
            txtAddress.Focusable = false;
            cboStatus.Focusable = false;
            cboEmployee.Focusable = false;
            cboService.Focusable = false;
        }

        private void FillTextBoxes(Appointment appointment)
        {
            //populate the text boxes
            txtAddress.Text = appointment.FullAddress;
            cboStatus.SelectedValue = appointment.Status;
            cboEmployee.SelectedValue = appointment.EmployeeId;
            cboCustomer.SelectedValue = appointment.CustomerId;
            cboService.SelectedValue = appointment.ServiceId;

            //populate the date pickers
            dpStartTime.SelectedDate = appointment.StartDateTime;
        }

        private void LoadScreen()
        {
            HttpClient client = InitializeClient();
            HttpResponseMessage response;

            //set up date picker


            //populate the service type combobox
            response = client.GetAsync("ServiceType").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            var data = items.ToObject<List<ServiceType>>();
            List<ServiceType> serviceTypes = data;
            IEnumerable<ServiceType> serviceTypesResult = serviceTypes.OrderBy(serviceTypes => serviceTypes.Description);
            cboService.ItemsSource = serviceTypesResult;
            cboService.DisplayMemberPath = "Description";
            cboService.SelectedValuePath = "Id";

            //populate the employee combobox
            response = client.GetAsync("Employee").Result;
            result = response.Content.ReadAsStringAsync().Result;
            items = (JArray)JsonConvert.DeserializeObject(result);
            data = items.ToObject<List<Employee>>();
            List<Employee> employees = data;
            IEnumerable<Employee> employeesResult = employees.OrderBy(employees => employees.FullName);
            cboEmployee.ItemsSource = employeesResult;
            cboEmployee.DisplayMemberPath = "FullName";
            cboEmployee.SelectedValuePath = "Id";

            //populate the customer combobox
            response = client.GetAsync("Customer").Result;
            result = response.Content.ReadAsStringAsync().Result;
            items = (JArray)JsonConvert.DeserializeObject(result);
            data = items.ToObject<List<Customer>>();
            List<Customer> customers = data;
            IEnumerable<Customer> customerResult = customers.OrderBy(customers => customers.FullName);
            cboCustomer.ItemsSource = customerResult;
            cboCustomer.DisplayMemberPath = "FullName";
            cboCustomer.SelectedValuePath = "Id";

            //populate status combobox
            cboStatus.ItemsSource = MakeStatusComboBox();
            cboStatus.DisplayMemberPath = "Name";
            cboStatus.SelectedValuePath = "Value";
        }

        private bool CheckScreen()
        {
            bool isempty = false;

            dynamic[] info = new dynamic[5];

            info[0] = cboCustomer;
            info[1] = cboEmployee;
            info[2] = cboService;
            info[3] = cboStatus;
            info[4] = txtAddress;


            foreach (dynamic inf in info)
            {
                switch (inf)
                {
                    case TextBox:
                        if (string.IsNullOrEmpty(inf.Text))
                        {
                            isempty = true;
                        }
                        continue;
                    case ComboBox:
                        if (string.IsNullOrEmpty(inf.Text))
                        {
                            isempty = true;
                        }
                        continue;
                }
            }
            return isempty;
        }

        private int GetLength(Guid cusId)
        {
            HttpClient client = InitializeClient();
            HttpResponseMessage response;

            response = client.GetAsync("Customer/" + cusId).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            var data = items.ToObject<List<Customer>>();

            Customer cus = data;

            return (int)Math.Ceiling((decimal)(cus.PropertySqFeet / 10000)) * 30;
            
        }

        private void AddData(Appointment appointment)
        {
            appointment.CustomerId = (Guid)cboCustomer.SelectedValue;
            appointment.EmployeeId = (Guid)cboEmployee.SelectedValue;
            appointment.ServiceId = (Guid)cboService.SelectedValue;
            appointment.StartDateTime = (DateTime)dpStartTime.SelectedDate;
            appointment.EndDateTime = (DateTime)dpStartTime.SelectedDate;
            appointment.Status = (string)cboStatus.SelectedValue;

            HttpClient client = InitializeClient();
            string SerializedObject = JsonConvert.SerializeObject(appointment);
            var content = new StringContent(SerializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync("Appointment", content).Result;

            employeeId = null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(CheckScreen())
                {
                    lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatus.Content = "Please Fill out all the Information.";
                    return;
                }
                appointment = new Appointment();
                AddData(appointment);
                _owner.RefreshDataGrid();
                _owner.ChangeStatus("Appointment Successfully Added");
                appointment = null;
                this.Close();
            }
            catch (Exception ex)
            {
                lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                lblStatus.Content = ex.Message;
            }
        }

        private Appointment LoadAppointment(Guid id)
        {
            Appointment loadedAppointment = new Appointment();
            loadedAppointment.Id = id;
            loadedAppointment.ServiceId = (Guid)cboService.SelectedValue;
            loadedAppointment.CustomerId = customerId;
            loadedAppointment.EmployeeId = (Guid)cboEmployee.SelectedValue;
            loadedAppointment.StartDateTime = (DateTime)dpStartTime.SelectedDate;
            loadedAppointment.EndDateTime = (DateTime)dpStartTime.SelectedDate;
            loadedAppointment.Status = cboStatus.SelectedValue.ToString();

            return loadedAppointment;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpClient client = InitializeClient();
                appointment = LoadAppointment(appointmentId);
                string serializedObject = JsonConvert.SerializeObject(appointment);
                var content = new StringContent(serializedObject);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = client.PutAsync("Appointment/", content).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                if (result == "1")
                {
                    _owner.RefreshDataGrid();
                    _owner.ChangeStatus("Appointment Succesfully Updated");
                    this.Close();
                    appointment = null;
                }
                else
                {
                    _owner.RefreshDataGrid();
                    _owner.ChangeStatus("Appointment Failed Updating");
                    this.Close();
                    appointment = null;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                lblStatus.Content = ex.Message;
            }
        }

        private void cboEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cboEmployee.SelectedIndex != -1)
            {
                cboStatus.SelectedIndex = 4;
            }
        }

        private void cboCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HttpClient client = InitializeClient();
            HttpResponseMessage response;

            Customer cus = new Customer();
            response = client.GetAsync("Customer/byId/" + cboCustomer.SelectedValue).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = JsonConvert.DeserializeObject(result);
            var data = items.ToObject<Customer>();
            cus = data;

            txtAddress.Text = cus.FullAddress;
        }
    }
}
