using KRV.LawnPro.BL;
using KRV.LawnPro.BL.Models;
using KRV.LawnPro.Reporting;
using Microsoft.Extensions.Logging;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KRV.LawnPro.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LawnProMainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        //used to pass information to the maintain screens for editing
        Customer customer;
        List<Customer> customers;

        Employee employee;
        List<Employee> employees;

        Appointment appointment;
        List<Appointment> appointments;

        ServiceType serviceType;
        List<ServiceType> serviceTypes;

        Invoice invoice;
        List<Invoice> invoices;
        Guid invoiceId;

        User user;
        List<User> users;

        private readonly ILogger<LawnProMainWindow> logger;
        List<string> filteredValues;
        int appointmentInt = 0;

        public LawnProMainWindow()
        {
            InitializeComponent();
            //set up the combo box with the different objects, customers, employees...
            cboData.ItemsSource = MakeCombobox();
            cboData.DisplayMemberPath = "Name";
            cboData.SelectedValuePath = "Value";
            //turn off all buttons until object is selected
            TurnButtons();
        }

        private static HttpClient InitializeClient()
        {
            HttpClient client = new HttpClient();
            // Will need to update address as needed, start API with UI
            client.BaseAddress = new Uri("https://localhost:44375/");  
            return client;
        }

        private void TurnButtons()
        {
            //turning the buttons on if a combobox is selected, edit and delete turn on when an object is selected in the datagrid
            if(cboData.SelectedIndex > -1)
            {
                btnLoad.Visibility = Visibility.Visible;
                btnNew.Visibility = Visibility.Visible;
                btnEdit.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
                btnExport.Visibility = Visibility.Visible;
            }
            else
            {
                btnLoad.Visibility = Visibility.Hidden;
                btnNew.Visibility = Visibility.Hidden;
                btnEdit.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
                btnExport.Visibility = Visibility.Hidden;
            }
        }

        private void SetDataGrid(object selectedvalue)
        {
            //set up the ui of the data grid, hide unwanted information, style the border
            Style headerStyle = new Style();
            DataGridColumnHeader header = new DataGridColumnHeader();
            headerStyle.TargetType = header.GetType();

            headerStyle.Setters.Add(new Setter { Property = Control.BackgroundProperty, Value = Brushes.DarkGreen });
            headerStyle.Setters.Add(new Setter { Property = Control.ForegroundProperty, Value = Brushes.White});
            headerStyle.Setters.Add(new Setter { Property = Control.FontWeightProperty, Value = FontWeights.Bold });
            headerStyle.Setters.Add(new Setter { Property = Control.BorderThicknessProperty, Value = new Thickness(0.5) });
            headerStyle.Setters.Add(new Setter { Property = Control.BorderBrushProperty, Value = Brushes.White });
            headerStyle.Setters.Add(new Setter { Property = Control.HorizontalContentAlignmentProperty, Value = HorizontalAlignment.Center });

            this.grdData.RowBackground = Brushes.White;
            this.grdData.AlternatingRowBackground = Brushes.LightGreen;

            switch (selectedvalue)
            {
                case "Customer":
                    grdData.Columns[0].Visibility = Visibility.Hidden;
                    grdData.Columns[1].Visibility = Visibility.Hidden;
                    grdData.Columns[2].Visibility = Visibility.Hidden;
                    grdData.Columns[3].Visibility = Visibility.Hidden;
                    grdData.Columns[4].Visibility = Visibility.Hidden;
                    grdData.Columns[5].Visibility = Visibility.Hidden;
                    grdData.Columns[6].Visibility = Visibility.Hidden;
                    grdData.Columns[7].HeaderStyle = headerStyle;
                    grdData.Columns[8].HeaderStyle = headerStyle;
                    grdData.Columns[9].Header = "Property Size";
                    grdData.Columns[9].Width = Width * 0.25;
                    grdData.Columns[9].HeaderStyle = headerStyle;
                    grdData.Columns[10].Visibility = Visibility.Hidden;
                    grdData.Columns[11].Header = "Name";
                    grdData.Columns[11].Width = Width * 0.25;
                    grdData.Columns[11].HeaderStyle = headerStyle;
                    grdData.Columns[12].Header = "Address";
                    grdData.Columns[12].Width = Width * 0.25;
                    grdData.Columns[12].HeaderStyle = headerStyle;
                    break;
                case "Employee":
                    grdData.Columns[0].Visibility = Visibility.Hidden;
                    grdData.Columns[1].Visibility = Visibility.Hidden;
                    grdData.Columns[2].Visibility = Visibility.Hidden;
                    grdData.Columns[3].Visibility = Visibility.Hidden;
                    grdData.Columns[4].Visibility = Visibility.Hidden;
                    grdData.Columns[5].Visibility = Visibility.Hidden;
                    grdData.Columns[6].Visibility = Visibility.Hidden;
                    grdData.Columns[9].Visibility = Visibility.Hidden;
                    grdData.Columns[1].HeaderStyle = headerStyle;
                    grdData.Columns[2].HeaderStyle = headerStyle;
                    grdData.Columns[7].HeaderStyle = headerStyle;
                    grdData.Columns[7].Width = Width * 0.3;
                    grdData.Columns[8].HeaderStyle = headerStyle;
                    grdData.Columns[10].HeaderStyle = headerStyle;
                    grdData.Columns[10].Header = "Name";
                    grdData.Columns[10].Width = Width * 0.3;
                    grdData.Columns[11].HeaderStyle = headerStyle;
                    grdData.Columns[11].Header = "Address";
                    grdData.Columns[11].Width = Width * 0.3;
                    break;
                case "Appointment":
                    grdData.Columns[0].Visibility = Visibility.Hidden;
                    grdData.Columns[1].Visibility = Visibility.Hidden;
                    grdData.Columns[2].Visibility = Visibility.Hidden;
                    grdData.Columns[3].Header = "Start Date";
                    grdData.Columns[3].HeaderStyle = headerStyle;
                    grdData.Columns[4].Header = "End Date";
                    grdData.Columns[4].HeaderStyle = headerStyle;
                    grdData.Columns[5].Visibility = Visibility.Hidden;
                    grdData.Columns[6].HeaderStyle = headerStyle;
                    grdData.Columns[7].Header = "Service";
                    grdData.Columns[7].HeaderStyle = headerStyle;
                    grdData.Columns[8].Header = "Rate";
                    grdData.Columns[8].HeaderStyle = headerStyle;
                    grdData.Columns[9].Header = "Length";
                    grdData.Columns[9].HeaderStyle = headerStyle;
                    grdData.Columns[10].Visibility = Visibility.Hidden;
                    grdData.Columns[11].Visibility = Visibility.Hidden;
                    grdData.Columns[12].Visibility = Visibility.Hidden;
                    grdData.Columns[13].Visibility = Visibility.Hidden;
                    grdData.Columns[14].Visibility = Visibility.Hidden;
                    grdData.Columns[15].Visibility = Visibility.Hidden;
                    grdData.Columns[16].Visibility = Visibility.Hidden;
                    grdData.Columns[17].Visibility = Visibility.Hidden;
                    grdData.Columns[18].HeaderStyle = headerStyle;
                    grdData.Columns[19].HeaderStyle = headerStyle;
                    grdData.Columns[20].Header = "Property Size";
                    grdData.Columns[20].HeaderStyle = headerStyle;
                    grdData.Columns[21].Header = "Customer Name";
                    grdData.Columns[21].HeaderStyle = headerStyle;
                    grdData.Columns[22].Header = "Employee Name";
                    grdData.Columns[22].HeaderStyle = headerStyle;
                    grdData.Columns[23].Header = "Address";
                    grdData.Columns[23].HeaderStyle = headerStyle;
                    break;
                case "ServiceType":
                    grdData.Columns[0].Visibility = Visibility.Hidden;
                    grdData.Columns[1].HeaderStyle = headerStyle;
                    grdData.Columns[1].Width = Width * 0.5;
                    grdData.Columns[2].HeaderStyle = headerStyle;
                    grdData.Columns[2].Header = "Rate";
                    grdData.Columns[2].Width = Width * 0.5;
                    break;
                case "Invoice":
                    grdData.Columns[0].Visibility = Visibility.Hidden;
                    grdData.Columns[1].Visibility = Visibility.Hidden;
                    grdData.Columns[2].Visibility = Visibility.Hidden;
                    grdData.Columns[3].Visibility = Visibility.Hidden;
                    grdData.Columns[4].Visibility = Visibility.Hidden;
                    grdData.Columns[5].HeaderStyle = headerStyle;
                    grdData.Columns[5].Header = "Customer Name";
                    grdData.Columns[6].Visibility = Visibility.Hidden;
                    grdData.Columns[7].Visibility = Visibility.Hidden;
                    grdData.Columns[8].Visibility = Visibility.Hidden;
                    grdData.Columns[9].HeaderStyle = headerStyle;
                    grdData.Columns[9].Header = "Address";
                    grdData.Columns[10].HeaderStyle = headerStyle;
                    grdData.Columns[10].Header = "Email";
                    grdData.Columns[11].HeaderStyle = headerStyle;
                    grdData.Columns[11].Header = "Date";
                    grdData.Columns[12].HeaderStyle = headerStyle;
                    grdData.Columns[12].Header = "Service";
                    grdData.Columns[13].HeaderStyle = headerStyle;
                    grdData.Columns[13].Header = "Property Size";
                    grdData.Columns[14].HeaderStyle = headerStyle;
                    grdData.Columns[14].Header = "Rate";
                    grdData.Columns[15].HeaderStyle = headerStyle;
                    grdData.Columns[15].Header = "Employee Name";
                    grdData.Columns[16].HeaderStyle = headerStyle;
                    grdData.Columns[16].Header = "Status";
                    grdData.Columns[17].HeaderStyle = headerStyle;
                    grdData.Columns[17].Header = "Total";
                    break;
                case "User":
                    grdData.Columns[0].Visibility = Visibility.Hidden;
                    grdData.Columns[1].Visibility = Visibility.Hidden;
                    grdData.Columns[2].Visibility = Visibility.Hidden;
                    grdData.Columns[3].HeaderStyle = headerStyle;
                    grdData.Columns[3].Header = "Username";
                    grdData.Columns[3].Width = Width * 0.5;
                    grdData.Columns[4].Visibility = Visibility.Hidden;
                    grdData.Columns[5].Visibility = Visibility.Hidden;
                    grdData.Columns[6].Visibility = Visibility.Hidden;
                    grdData.Columns[7].HeaderStyle = headerStyle;
                    grdData.Columns[7].Header = "Name";
                    grdData.Columns[7].Width = Width * 0.5;
                    break;
            }
                
        }


        private void cboData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //checking if the value is null before i refresh the datagrid, needs a value to refresh
                if(cboData.SelectedValue == null)
                {
                    lblStatus.Foreground = new SolidColorBrush(Colors.White);
                    lblStatus.Content = "Welcome";
                    cboFilter.SelectedValue = -1;
                    return;
                }
                btnMagnify.Visibility = Visibility.Hidden;
                btnMagnify.IsEnabled = false;
                cboFilter.SelectedValue = 0;
                RefreshDataGrid();
            }
            catch (Exception ex)
            {
                lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                lblStatus.Content = ex.Message;
            }
                 

        }

        public class Combo
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public void RefreshDataGrid()
        {
            //Sets all the information in the datagrid depending on what object is selected in the combobox
            HttpClient client = InitializeClient();
            HttpResponseMessage response;

            //var classType = SetClass(cboData.SelectedValue);
            grdData.ItemsSource = null;
            TurnButtons();
            response = client.GetAsync(cboData.SelectedValue.ToString()).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);

            List<Combo> combos = new List<Combo>();

            switch (cboData.SelectedValue)
            {
                case "Customer":
                    lblStatus.Content = "";
                    var data = items.ToObject<List<Customer>>();
                    grdData.ItemsSource = data;
                    customers = data;
                    filteredValues = GetCustomerFilterObjects(customers);
                    cboFilter.IsEnabled = true;
                    combos = MakeFilterComboBox(filteredValues, 1);
                    combos.Add(new Combo() { Name = "Status: Consultation", Value = "Consultation" });
                    cboFilter.ItemsSource = combos;
                    SetCboFilterUp();
                    LoadScreen("Customer/s");
                    break;
                case "Employee":
                    lblStatus.Content = "";
                    data = items.ToObject<List<Employee>>();
                    grdData.ItemsSource = data;
                    employees = data;
                    filteredValues = GetEmployeeFilterObjects(employees);
                    cboFilter.IsEnabled = true;
                    cboFilter.ItemsSource = MakeFilterComboBox(filteredValues, 1);
                    SetCboFilterUp();
                    LoadScreen("Employee/s");
                    break;
                case "Appointment":
                    lblStatus.Content = "";
                    data = items.ToObject<List<Appointment>>();
                    grdData.ItemsSource = data;
                    appointments = data;
                    filteredValues = GetAppointmentFilterObjects(appointments);
                    cboFilter.IsEnabled = true;
                    combos = MakeFilterComboBox(filteredValues, 2);
                    if (combos.Any(combos => combos.Value == "Unscheduled") == false)
                    {
                        combos.Add(new Combo() { Name = "Status: Unscheduled", Value = "Unscheduled" }); 
                    }
                    cboFilter.ItemsSource = combos;
                    SetCboFilterUp();
                    SetDataGrid(cboData.SelectedValue);
                    int numberOfAppointments = DetermineAppointments();
                    lblStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                    lblStatus.Content = "Returned " + (grdData.Items.Count - 1) + " Appointments." + " There are " + numberOfAppointments + " unscheduled.";
                    break;
                case "ServiceType":
                    lblStatus.Content = "";
                    data = items.ToObject<List<ServiceType>>();
                    grdData.ItemsSource = data;
                    serviceTypes = data;
                    filteredValues = GetServiceTypeFilterObjects(serviceTypes);
                    cboFilter.IsEnabled = true;
                    cboFilter.ItemsSource = MakeFilterComboBox(filteredValues, 3);
                    SetCboFilterUp();
                    LoadScreen("Service Type/s");
                    break;
                case "Invoice":
                    lblStatus.Content = "";
                    data = items.ToObject<List<Invoice>>();
                    grdData.ItemsSource = data;
                    invoices = data;
                    filteredValues = GetInvoiceFilterObjects(invoices);
                    cboFilter.IsEnabled = true;
                    cboFilter.ItemsSource = MakeFilterComboBox(filteredValues, 1);
                    SetCboFilterUp();
                    LoadScreen("Invoice/s");
                    break;
                case "User":
                    lblStatus.Content = "";
                    data = items.ToObject<List<User>>();
                    grdData.ItemsSource = data;
                    users = data;
                    cboFilter.ItemsSource = null;
                    cboFilter.IsEnabled = false;
                    LoadScreen("User/s");
                    break;
            }
        }

        private List<Combo> MakeCombobox()
        {
            var datasource = new List<Combo>();
            datasource.Add(new Combo() { Name = "Users", Value = "User" });
            datasource.Add(new Combo() { Name = "Customers", Value = "Customer" });
            datasource.Add(new Combo() { Name = "Employees", Value = "Employee" });
            datasource.Add(new Combo() { Name = "Appointments", Value = "Appointment" });
            datasource.Add(new Combo() { Name = "Service Types", Value = "ServiceType" });
            datasource.Add(new Combo() { Name = "Invoices", Value = "Invoice" });

            return datasource;
        }

        private List<Combo> MakeFilterComboBox(List<string> valuesToFilter, int filtertype)
        {
            var datasource = new List<Combo>();
            datasource.Add(new Combo() { Name = "All", Value = "All" });

            foreach(string s in valuesToFilter)
            {
                bool result = datasource.Any(Combo => Combo.Value == s);
                if(result == false)
                {
                    switch(filtertype)
                    {
                        case 1:
                            datasource.Add(new Combo() { Name = "Zip Code: " + s, Value = s });
                            break;
                        case 2:
                            datasource.Add(new Combo() { Name = "Status: " + s, Value = s });
                            break;
                        case 3:
                            datasource.Add(new Combo() { Name = "Cost: " + s, Value = s });
                            break;
                    }
                }
                else
                {
                    continue;
                }
            }

            return datasource;
        }

        private int DetermineAppointments()
        {
            int result = 0;
            foreach(Appointment a in appointments)
            {
                bool unscheduled = a.Status.Equals("Unscheduled");
                if(unscheduled == true)
                {
                    result++;
                }
            }
            return result;
        }

        private List<string> GetCustomerFilterObjects(List<Customer> customersToFilter)
        {
            List<string> customerValues = new List<string>();

            foreach(Customer c in customersToFilter)
            {
                customerValues.Add(c.ZipCode);
            }

            return customerValues;
        }

        private List<string> GetEmployeeFilterObjects(List<Employee> employeesToFilter)
        {
            List<string> employeeValues = new List<string>();

            foreach (Employee e in employeesToFilter)
            {
                employeeValues.Add(e.ZipCode);
            }

            return employeeValues;
        }

        private List<string> GetAppointmentFilterObjects(List<Appointment> appointmentsToFilter)
        {
            List<string> appointmentValues = new List<string>();

            foreach (Appointment a in appointmentsToFilter)
            {
                appointmentValues.Add(a.Status);
            }

            return appointmentValues;
        }

        private List<string> GetServiceTypeFilterObjects(List<ServiceType> serviceTypesToFilter)
        {
            List<string> serviceTypeValues = new List<string>();

            foreach (ServiceType st in serviceTypesToFilter)
            {
                serviceTypeValues.Add(st.CostPerSQFT.ToString());
            }

            return serviceTypeValues;
        }

        private List<string> GetInvoiceFilterObjects(List<Invoice> invoicesToFilter)
        {
            List<string> invoiceValues = new List<string>();

            foreach (Invoice i in invoicesToFilter)
            {
                invoiceValues.Add(i.CustomerZip);
            }

            return invoiceValues;
        }

        private void SetCboFilterUp()
        {
            cboFilter.DisplayMemberPath = "Name";
            cboFilter.SelectedValuePath = "Value";
            cboFilter.SelectedIndex = 0;
        }

        private void LoadScreen(string classType)
        {
            if (classType == "Appointment/s" && appointmentInt == 1)
            {
                SetDataGrid(cboData.SelectedValue);
                int numberOfAppointments = DetermineAppointments();
                lblStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                lblStatus.Content = "Returned " + (grdData.Items.Count - 1) + " Appointments." + " There are " + numberOfAppointments + " unscheduled.";
                appointmentInt = 0;
                return;
            }

            SetDataGrid(cboData.SelectedValue);
            lblStatus.Foreground = new SolidColorBrush(Colors.Yellow);
            lblStatus.Content = "Returned " + (grdData.Items.Count - 1) + " " + classType;
        }

        public void ChangeStatus(string label)
        {
            //Used to change the lblstatus on the mainpage from the subsiquent pages
            lblStatus.Foreground = new SolidColorBrush(Colors.Yellow);
            lblStatus.Content = label;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {     

            try
            {
                switch(cboData.SelectedValue)
                {
                    case "Customer":
                        CustomerManager.ExportExcel(customers);
                        lblStatus.Content = "Successfully Exported Customers";
                        break;
                    case "Employee":
                        EmployeeManager.ExportExcel(employees);
                        lblStatus.Content = "Successfully Exported Employees";
                        break;
                    case "Appointment":
                        AppointmentManager.ExportExcel(appointments);
                        lblStatus.Content = "Successfully Exported Appointments";
                        break;
                    case "Service Type":
                        ServiceTypeManager.ExportExcel(serviceTypes);
                        lblStatus.Content = "Successfully Exported Service Types";
                        break;
                    case "Invoice":
                        if (grdData.SelectedValue == null)
                        {
                            //export the whole grid to excel
                            InvoiceManager.ExportExcel(invoices);
                            lblStatus.Content = "Successfully Exported Invoices";
                        }
                        else
                        {
                            //export a selected item to PDF
                            string invoicePDF = "Invoice" + "-" + invoice.ServiceDate.ToString("MM-dd-yyyy") + "-" + invoice.CustomerFullName;
                            InvoiceManager.InvoicePDF(invoicePDF, invoice);
                        }
                        break;
                    case "User":
                        UserManager.ExportExcel(users);
                        lblStatus.Content = "Successfully Exported Users";
                        break;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                lblStatus.Content = ex.Message;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            //clears the screen, back to the begining
            cboData.SelectedIndex = -1;
            cboFilter.SelectedIndex = -1;
            grdData.ItemsSource = null;
            lblStatus.Foreground = new SolidColorBrush(Colors.White);
            TurnButtons();
        }

        private void grdData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //turns on the edit and delete buttons for when the object is selected in the datagrid
            if(grdData.SelectedValue != null)
            {
                btnMagnify.Visibility = Visibility.Visible;
                btnMagnify.IsEnabled = true;
                lblStatus.Content = string.Empty;
                btnEdit.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Visible;
                btnNew.Visibility = Visibility.Hidden;
                btnExport.Visibility = Visibility.Visible;
                // sets up an enpty object to be sent to the edit pages
                switch(cboData.SelectedValue)
                {
                    case "Customer":
                        if (grdData.SelectedValue != null)
                        {
                            customer = (Customer)grdData.SelectedValue; 
                        }
                        break;
                    case "Employee":
                        if (grdData.SelectedValue != null)
                        {
                            employee = (Employee)grdData.SelectedValue; 
                        }
                        break;
                    case "Appointment":
                        if (grdData.SelectedValue != null)
                        {
                            appointment = (Appointment)grdData.SelectedValue; 
                        }
                        break;
                    case "ServiceType":
                        if (grdData.SelectedValue != null)
                        {
                            serviceType = (ServiceType)grdData.SelectedValue; 
                        }
                        break;
                    case "Invoice":
                        if (grdData.SelectedValue != null)
                        {
                            invoice = (Invoice)grdData.SelectedValue;
                            invoiceId = invoice.Id;
                        }
                        break;
                    case "User":
                        if (grdData.SelectedValue != null)
                        {
                            user = (User)grdData.SelectedValue; 
                        }
                        break;
                }
            }

        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            switch(cboData.SelectedValue)
            {
                case "Customer":
                    new MaintainCustomersEmployees(ScreenMode.Customer, this).ShowDialog();
                    break;
                case "Employee":
                    new MaintainCustomersEmployees(ScreenMode.Employee, this).ShowDialog();
                    break;
                case "Appointment":
                    new MaintainAppointments(this).ShowDialog();
                    break;
                case "ServiceType":
                    new MaintainServiceTypes(this).ShowDialog();
                    break;
                case "Invoice":
                    new MaintainInvoices(this).ShowDialog();
                    break;
                case "User":
                    new MaintainUsers(this).ShowDialog();
                    break;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            switch(cboData.SelectedValue)
            {
                case "Customer":
                    new MaintainCustomersEmployees(ScreenMode.Customer, this, customer).ShowDialog();
                    break;
                case "Employee":
                    new MaintainCustomersEmployees(ScreenMode.Employee, this, employee).ShowDialog();
                    break;
                case "Appointment":
                    if(appointment.Status == "InProgress" || appointment.Status == "Completed")
                    {
                        lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                        ChangeStatus("Cannot Edit an Appointment with status In Progress or Completed");
                        break;
                    }
                    else
                    {
                        new MaintainAppointments(this, appointment).ShowDialog();
                    }
                    break;
                case "ServiceType":
                    new MaintainServiceTypes(this, serviceType).ShowDialog();
                    break;
                case "Invoice":
                    new MaintainInvoices(this, invoice).ShowDialog();
                    break;
                case "User":
                    new MaintainUsers(this, user).ShowDialog();
                    break;
            }
        }

        private List<Appointment> LoadAppoinments()
        {
            List<Appointment> appts = new List<Appointment>();

            HttpClient client = InitializeClient();
            HttpResponseMessage response;

            response = client.GetAsync("Appointment").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            appts = items.ToObject<List<Appointment>>();

            return appts;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            List<Appointment> appts = new List<Appointment>();
            appts = LoadAppoinments();
            bool canDelete = true;

            switch(cboData.SelectedValue)
            {
                case "Customer":
                    foreach(Appointment a in appts)
                    {
                        if(a.CustomerId == customer.Id)
                        {
                            canDelete = false;
                        }
                    }
                    if (canDelete == false)
                    {
                        lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                        ChangeStatus("Cannot Delete a Customer with an existing Appointment");
                        break;
                    }
                    else
                    {
                        new DeleteWindow(customer, cboData.SelectedValue, this).ShowDialog();
                    }
                    break;
                case "Employee":
                    foreach(Appointment a in appts)
                    {
                        if(a.EmployeeId == employee.Id)
                        {
                            canDelete = false;
                        }
                    }
                    if (canDelete == false)
                    {
                        lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                        ChangeStatus("Cannot Delete an Employee with an existing Appointment");
                        break;
                    }
                    else
                    {
                        new DeleteWindow(employee, cboData.SelectedValue, this).ShowDialog(); 
                    }
                    break;
                case "Appointment":
                    if (appointment.Status == "InProgress" || appointment.Status == "Completed")
                    {
                        lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                        ChangeStatus("Cannot Delete an Appointment with status In Progress or Completed");
                        break;
                    }
                    else
                    {
                        new DeleteWindow(appointment, cboData.SelectedValue, this).ShowDialog();
                    }
                    break;
                case "ServiceType":
                    foreach(Appointment a in appts)
                    {
                        if(a.ServiceId == serviceType.Id)
                        {
                            canDelete = false;
                        }
                    }
                    if(canDelete == false)
                    {
                        lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                        ChangeStatus("Cannot Delete a Service Type with an existing Appointment");
                        break;
                    }
                    else
                    {
                        new DeleteWindow(serviceType, cboData.SelectedValue, this).ShowDialog(); 
                    }
                    break;
                case "Invoice":
                    new DeleteWindow(invoice, cboData.SelectedValue, this).ShowDialog();
                    break;
                case "User":
                    new DeleteWindow(user, cboData.SelectedValue, this).ShowDialog();
                    break;
            }
        }

        private void cboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnMagnify.IsEnabled = false;
            btnMagnify.Visibility = Visibility.Hidden;
            TurnButtons();

            if(cboFilter.SelectedIndex == 0)
            {
                switch(cboData.SelectedValue)
                {
                    case "Customer":
                        grdData.ItemsSource = customers;
                        break;
                    case "Employee":
                        grdData.ItemsSource = employees;
                        break;
                    case "Appointment":
                        appointmentInt = 1;
                        grdData.ItemsSource = appointments;
                        break;
                    case "Invoice":
                        grdData.ItemsSource = invoices;
                        break;
                    case "ServiceType":
                        grdData.ItemsSource = serviceTypes;
                        break;
                    case "User":
                        grdData.ItemsSource = users;
                        break;
                }

                LoadScreen(cboData.SelectedValue.ToString() + "/s");
                return;
            }
            else if(cboFilter.SelectedIndex == -1)
            {
                return;
            }

            switch (cboData.SelectedValue)
            {
                case null:
                    break;
                case "Customer":
                    List<Customer> filteredCustomers = new List<Customer>();
                    if(cboFilter.SelectedValue.ToString() == "Consultation")
                    {
                        foreach(Customer c in customers)
                        {
                            if(c.PropertySqFeet == 0)
                            {
                                filteredCustomers.Add(c);
                            }
                        }
                    }
                    else
                    {
                        foreach (Customer c in customers)
                        {
                            if (c.ZipCode == cboFilter.SelectedValue.ToString())
                            {
                                filteredCustomers.Add(c);
                            }
                        }
                    }
                    grdData.ItemsSource = filteredCustomers;
                    LoadScreen("Customer/s");
                    break;
                case "Employee":
                    List<Employee> filteremployees = new List<Employee>();
                    foreach (Employee emp in employees)
                    {
                        if (emp.ZipCode == cboFilter.SelectedValue.ToString())
                        {
                            filteremployees.Add(emp);
                        }
                    }
                    grdData.ItemsSource = filteremployees;
                    LoadScreen("Employee/s");
                    break;
                case "Appointment":
                    List<Appointment> filteredappointments = new List<Appointment>();
                    if(cboFilter.SelectedValue.ToString() == "Unscheduled")
                    {
                        foreach(Appointment a in appointments)
                        {
                            if(a.Status == "Unscheduled")
                            {
                                filteredappointments.Add(a);
                            }
                        }
                    }
                    else
                    {
                        foreach (Appointment a in appointments)
                        {
                            if (a.Status == cboFilter.SelectedValue.ToString())
                            {
                                filteredappointments.Add(a);
                            }
                        }
                    }
                    grdData.ItemsSource = filteredappointments;
                    LoadScreen("Appointment/s");
                    break;
                case "ServiceType":
                    List<ServiceType> filteredServiceTypes = new List<ServiceType>();
                    foreach(ServiceType st in serviceTypes)
                    {
                        if(st.CostPerSQFT.ToString() == cboFilter.SelectedValue.ToString())
                        {
                            filteredServiceTypes.Add(st);
                        }
                    }
                    grdData.ItemsSource = filteredServiceTypes;
                    LoadScreen("Service Type/s");
                    break;
                case "Invoice":
                    List<Invoice> filteredInvoices = new List<Invoice>();
                    foreach(Invoice i in invoices)
                    {
                        if(i.CustomerZip == cboFilter.SelectedValue.ToString())
                        {
                            filteredInvoices.Add(i);
                        }
                    }
                    grdData.ItemsSource = filteredInvoices;
                    LoadScreen("Invoice/s");
                    break;
                case "User":

                    break;
            }
        }

        private void btnMagnify_Click(object sender, RoutedEventArgs e)
        {
            switch (cboData.SelectedValue)
            {
                case "Customer":
                    new MaintainCustomersEmployees(ScreenMode.Customer, this, customer, true).ShowDialog();
                    break;
                case "Employee":
                    new MaintainCustomersEmployees(ScreenMode.Employee, this, employee, true).ShowDialog();
                    break;
                case "Appointment":
                    new MaintainAppointments(this, appointment, true).ShowDialog();
                    break;
                case "ServiceType":
                    new MaintainServiceTypes(this, serviceType, true).ShowDialog();
                    break;
                case "Invoice":
                    new MaintainInvoices(this, invoice, true).ShowDialog();
                    break;
                case "User":
                    new MaintainUsers(this, user, true).ShowDialog();
                    break;
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //checking if the value is null before i refresh the datagrid, needs a value to refresh
                if (cboData.SelectedIndex == -1)
                {
                    lblStatus.Foreground = new SolidColorBrush(Colors.White);
                    lblStatus.Content = "Welcome";
                    cboFilter.SelectedValue = -1;
                    return;
                }
                else if (cboFilter.SelectedIndex == 0)
                {
                    btnMagnify.Visibility = Visibility.Hidden;
                    btnMagnify.IsEnabled = false;
                    cboFilter.SelectedValue = 0;
                    RefreshDataGrid();
                    return;
                }
                else if (cboFilter.SelectedIndex == -1)
                {
                    return;
                }

                switch (cboData.SelectedValue)
                {
                    case null:
                        break;
                    case "Customer":
                        List<Customer> filteredCustomers = new List<Customer>();
                        if (cboFilter.SelectedValue.ToString() == "Consultation")
                        {
                            foreach (Customer c in customers)
                            {
                                if (c.PropertySqFeet == 0)
                                {
                                    filteredCustomers.Add(c);
                                }
                            }
                        }
                        else
                        {
                            foreach (Customer c in customers)
                            {
                                if (c.ZipCode == cboFilter.SelectedValue.ToString())
                                {
                                    filteredCustomers.Add(c);
                                }
                            }
                        }
                        grdData.ItemsSource = filteredCustomers;
                        LoadScreen("Customer/s");
                        break;
                    case "Employee":
                        List<Employee> filteremployees = new List<Employee>();
                        foreach (Employee emp in employees)
                        {
                            if (emp.ZipCode == cboFilter.SelectedValue.ToString())
                            {
                                filteremployees.Add(emp);
                            }
                        }
                        grdData.ItemsSource = filteremployees;
                        LoadScreen("Employee/s");
                        break;
                    case "Appointment":
                        List<Appointment> filteredappointments = new List<Appointment>();
                        if (cboFilter.SelectedValue.ToString() == "Unscheduled")
                        {
                            foreach (Appointment a in appointments)
                            {
                                if (a.Status == "Unscheduled")
                                {
                                    filteredappointments.Add(a);
                                }
                            }
                        }
                        else
                        {
                            foreach (Appointment a in appointments)
                            {
                                if (a.Status == cboFilter.SelectedValue.ToString())
                                {
                                    filteredappointments.Add(a);
                                }
                            }
                        }
                        grdData.ItemsSource = filteredappointments;
                        LoadScreen("Appointment/s");
                        break;
                    case "ServiceType":
                        List<ServiceType> filteredServiceTypes = new List<ServiceType>();
                        foreach (ServiceType st in serviceTypes)
                        {
                            if (st.CostPerSQFT.ToString() == cboFilter.SelectedValue.ToString())
                            {
                                filteredServiceTypes.Add(st);
                            }
                        }
                        grdData.ItemsSource = filteredServiceTypes;
                        LoadScreen("Service Type/s");
                        break;
                    case "Invoice":
                        List<Invoice> filteredInvoices = new List<Invoice>();
                        foreach (Invoice i in invoices)
                        {
                            if (i.CustomerZip == cboFilter.SelectedValue.ToString())
                            {
                                filteredInvoices.Add(i);
                            }
                        }
                        grdData.ItemsSource = filteredInvoices;
                        LoadScreen("Invoice/s");
                        break;
                    case "User":

                        break;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                lblStatus.Content = ex.Message;
            }
        }

    }
}
