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

namespace KRV.LawnPro.UI
{
    public enum ScreenMode
    {
        Customer = 1,
        Employee = 2
    }

    /// <summary>
    /// Interaction logic for MaintainCustomers.xaml
    /// </summary>
    public partial class MaintainCustomersEmployees : Window
    {
        ScreenMode screenMode;
        Customer customer;
        Guid customerId;
        Employee employee;
        Guid employeeId;
        LawnProMainWindow _owner;

        public MaintainCustomersEmployees(ScreenMode screenmode, LawnProMainWindow owner)
        {
            InitializeComponent();
            screenMode = screenmode;
            _owner = owner;
            MoveButtons(btnAdd, 0);
            LoadScreen();
            txtFirstName.Focus();

            lblTitle.Content = screenMode.ToString();
            this.Title = "Maintain " + screenMode.ToString();
        }

        public MaintainCustomersEmployees(ScreenMode screenmode, LawnProMainWindow owner, Customer passedcustomer)
        {
            InitializeComponent();
            screenMode = screenmode;
            _owner = owner;
            LoadScreen();
            MoveButtons(btnUpdate, 0);
            FillTextBoxes(passedcustomer);
            customerId = passedcustomer.Id;
            lblTitle.Content = screenMode.ToString();
            this.Title = "Maintain " + screenMode.ToString();
        }

        public MaintainCustomersEmployees(ScreenMode screenmode, LawnProMainWindow owner, Customer passedcustomer, bool magnify)
        {
            InitializeComponent();
            screenMode = screenmode;
            _owner = owner;
            btnAdd.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Hidden;
            LoadScreen();
            JustMagnify();
            FillTextBoxes(passedcustomer);

            customerId = passedcustomer.Id;

            lblTitle.Content = screenMode.ToString();
            this.Title = "Inspect " + screenMode.ToString();
        }

        public MaintainCustomersEmployees(ScreenMode screenmode, LawnProMainWindow owner, Employee employee, bool magnify)
        {
            InitializeComponent();
            screenMode = screenmode;
            _owner = owner;
            btnAdd.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Hidden;
            LoadScreen();
            FillTextBoxes(employee);
            JustMagnify();

            employeeId = employee.Id;

            lblTitle.Content = screenMode.ToString();
            this.Title = "Inspect " + screenMode.ToString();
        }

        public MaintainCustomersEmployees(ScreenMode screenmode, LawnProMainWindow owner, Employee employee)
        {
            InitializeComponent();
            screenMode = screenmode;
            _owner = owner;
            LoadScreen();
            MoveButtons(btnUpdate, 0);
            FillTextBoxes(employee);

            employeeId = employee.Id;

            lblTitle.Content = screenMode.ToString();
            this.Title = "Maintain " + screenMode.ToString();
        }

        private void JustMagnify()
        {
            btnAdd.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnAdd.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Hidden;
            txtCity.Focusable = false;
            txtEmail.Focusable = false;
            txtFirstName.Focusable = false;
            txtLastName.Focusable = false;
            txtPhone.Focusable = false;
            txtProperty.Focusable = false;
            txtStreetAddress.Focusable = false;
            txtZipCode.Focusable = false;
            cboState.IsEnabled = false;
            cboUser.IsEnabled = false;
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

        private void FillTextBoxes(Customer customer)
        {
            txtFirstName.Text = customer.FirstName;
            txtLastName.Text = customer.LastName;
            txtStreetAddress.Text = customer.StreetAddress;
            txtCity.Text = customer.City;
            txtZipCode.Text = customer.ZipCode;
            cboState.SelectedValue = customer.State;
            txtEmail.Text = customer.Email;
            txtPhone.Text = customer.Phone;
            txtProperty.Text = customer.PropertySqFeet.ToString();
            cboUser.SelectedValue = customer.UserId;
        }

        private void FillTextBoxes(Employee employee)
        {
            txtFirstName.Text = employee.FirstName;
            txtLastName.Text = employee.LastName;
            txtStreetAddress.Text = employee.StreetAddress;
            txtCity.Text = employee.City;
            txtZipCode.Text = employee.ZipCode;
            cboState.SelectedValue = employee.State;
            txtEmail.Text = employee.Email;
            txtPhone.Text = employee.Phone;
            txtProperty.Visibility = Visibility.Hidden;
            lblProperty.Visibility = Visibility.Hidden;
            cboUser.SelectedValue = employee.UserId;
        }

        private void LoadScreen()
        {
            if (ScreenMode.Employee == screenMode)
            {
                lblProperty.Content = "";
                txtProperty.Visibility = Visibility.Hidden;
            }

            HttpClient client = InitializeClient();
            HttpResponseMessage response;

            response = client.GetAsync("User").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = (JArray)JsonConvert.DeserializeObject(result);
            var data = items.ToObject<List<User>>();
            List<User> users = data;
            IEnumerable<User> usersResult = users.OrderBy(users => users.FullName);
            cboUser.ItemsSource = usersResult;
            cboUser.DisplayMemberPath = "FullName";
            cboUser.SelectedValuePath = "Id";

            cboState.ItemsSource = MakeCombobox();
            cboState.DisplayMemberPath = "Name";
            cboState.SelectedValuePath = "Value";

        }

        private static HttpClient InitializeClient()
        {
            HttpClient client = new HttpClient();
            // Will need to update address as needed, start API with UI
            client.BaseAddress = new Uri("https://localhost:44375/");
            return client;
        }

        public class Combo
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private IEnumerable<Combo> MakeCombobox()
        {
            //Need to add all the states
            var datasource = new List<Combo>();
            datasource.Add(new Combo { Name = "Wisconsin", Value = "WI" });
            datasource.Add(new Combo { Name = "Michigan", Value = "MI" });
            datasource.Add(new Combo { Name = "California", Value = "CA" });
            datasource.Add(new Combo { Name = "Florida", Value = "FL" });
            datasource.Add(new Combo { Name = "Minnesota", Value = "MN" });
            datasource.Add(new Combo { Name = "New York", Value = "NY" });
            datasource.Add(new Combo { Name = "Colorado", Value = "CO" });
            datasource.Add(new Combo { Name = "Oregon", Value = "OR" });
            datasource.Add(new Combo { Name = "Arizona", Value = "AZ" });
            datasource.Add(new Combo { Name = "Nevada", Value = "NV" });
            datasource.Add(new Combo { Name = "Illinois", Value = "IL" });
            datasource.Add(new Combo { Name = "Ohio", Value = "OH" });
            datasource.Add(new Combo { Name = "Kentucky", Value = "KY" });
            datasource.Add(new Combo { Name = "Iowa", Value = "IA" });
            datasource.Add(new Combo { Name = "Georgia", Value = "GA" });
            datasource.Add(new Combo { Name = "North Dakota", Value = "ND" });
            datasource.Add(new Combo { Name = "South Dakota", Value = "SD" });
            datasource.Add(new Combo { Name = "Lousiana", Value = "LA" });
            datasource.Add(new Combo { Name = "South Carolina", Value = "SC" });
            datasource.Add(new Combo { Name = "North Carolina", Value = "NC" });
            datasource.Add(new Combo { Name = "Wyoming", Value = "WY" });
            datasource.Add(new Combo { Name = "Missisippi", Value = "MS" });
            datasource.Add(new Combo { Name = "Idaho", Value = "ID" });
            datasource.Add(new Combo { Name = "Alaska", Value = "AL" });
            datasource.Add(new Combo { Name = "Arkansas", Value = "AR" });
            datasource.Add(new Combo { Name = "Connecticutt", Value = "CT" });
            datasource.Add(new Combo { Name = "Deleware", Value = "DE" });
            datasource.Add(new Combo { Name = "Hawaii", Value = "HI" });
            datasource.Add(new Combo { Name = "Indiana", Value = "IN" });
            datasource.Add(new Combo { Name = "Kansas", Value = "KS" });
            datasource.Add(new Combo { Name = "Maine", Value = "ME" });
            datasource.Add(new Combo { Name = "Mississippi", Value = "MS" });
            datasource.Add(new Combo { Name = "Maryland", Value = "MD" });
            datasource.Add(new Combo { Name = "Massachusetts", Value = "MA" });
            datasource.Add(new Combo { Name = "Missouri", Value = "MI" });
            datasource.Add(new Combo { Name = "Montant", Value = "MO" });
            datasource.Add(new Combo { Name = "Nebraska", Value = "NE" });
            datasource.Add(new Combo { Name = "New Jersey", Value = "NJ" });
            datasource.Add(new Combo { Name = "New Hampshire", Value = "NH" });
            datasource.Add(new Combo { Name = "New Mexico", Value = "NM" });
            datasource.Add(new Combo { Name = "Oklahoma", Value = "OK" });
            datasource.Add(new Combo { Name = "Pennsylania", Value = "PA" });
            datasource.Add(new Combo { Name = "Rhode Island", Value = "RI" });
            datasource.Add(new Combo { Name = "Texas", Value = "TX" });
            datasource.Add(new Combo { Name = "Utah", Value = "UT" });
            datasource.Add(new Combo { Name = "Vermont", Value = "VT" });
            datasource.Add(new Combo { Name = "Virginia", Value = "VI" });
            datasource.Add(new Combo { Name = "West Virginia", Value = "WV" });
            IEnumerable<Combo> result = datasource.OrderBy(datasource => datasource.Name);
            return result;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (screenMode)
                {

                    case ScreenMode.Customer:
                        if (CheckScreen())
                        {
                            lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                            lblStatus.Content = "Please Fill out all the Information";
                            break;
                        }
                        customer = new Customer();
                        AddData(customer);
                        _owner.RefreshDataGrid();
                        _owner.ChangeStatus("Customer Succesfully Added");
                        this.Close();
                        customer = null;
                        break;
                    case ScreenMode.Employee:
                        if (CheckScreen())
                        {
                            lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                            lblStatus.Content = "Please Fill out all the Information";
                            break;
                        }
                        employee = new Employee();
                        AddData(employee);
                        _owner.RefreshDataGrid();
                        _owner.ChangeStatus("Employee Succesfully Added");
                        this.Close();
                        employee = null;
                        break;
                }
            }
            catch (Exception ex)
            {

                lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                lblStatus.Content = ex.Message;
            }
        }

        private void AddData(dynamic data)
        {
            switch(data)
            {
                case Customer:
                    customer.FirstName = txtFirstName.Text;
                    customer.LastName = txtLastName.Text;
                    customer.City = txtCity.Text;
                    customer.StreetAddress = txtStreetAddress.Text;
                    customer.State = cboState.SelectedValue.ToString();
                    customer.ZipCode = txtZipCode.Text;
                    customer.Phone = txtPhone.Text;
                    customer.Email = txtEmail.Text;
                    customer.PropertySqFeet = Convert.ToInt32(txtProperty.Text);
                    customer.UserId = (Guid)cboUser.SelectedValue;

                    HttpClient client = InitializeClient();
                    string SerializedObject = JsonConvert.SerializeObject(customer);
                    var content = new StringContent(SerializedObject);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = client.PostAsync("Customer", content).Result;
                    break;
                case Employee:
                    employee.FirstName = txtFirstName.Text;
                    employee.LastName = txtLastName.Text;
                    employee.City = txtCity.Text;
                    employee.StreetAddress = txtStreetAddress.Text;
                    employee.State = cboState.SelectedValue.ToString();
                    employee.ZipCode = txtZipCode.Text;
                    employee.Phone = txtPhone.Text;
                    employee.Email = txtEmail.Text;
                    employee.UserId = (Guid)cboUser.SelectedValue;

                    client = InitializeClient();
                    SerializedObject = JsonConvert.SerializeObject(employee);
                    content = new StringContent(SerializedObject);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    response = client.PostAsync("Employee", content).Result;
                    break;
            }
        }

        private bool CheckScreen()
        {
            bool isempty = false;

            dynamic[] info = new dynamic[8];
            info[0] = txtCity;
            info[1] = txtEmail;
            info[2] = txtFirstName;
            info[3] = txtLastName;
            info[4] = txtPhone;
            info[5] = txtStreetAddress;
            info[6] = txtZipCode;
            info[7] = txtProperty;

            switch(screenMode)
            {
                case ScreenMode.Customer:
                    foreach(dynamic i in info)
                    {
                        switch(i)
                        {
                            case TextBox:
                                if(string.IsNullOrEmpty(i.Text))
                                {
                                    isempty = true;
                                }
                                continue;
                            case ComboBox:
                                if(string.IsNullOrEmpty(i.Text))
                                {
                                    isempty = true;
                                }
                                continue;
                        }
                    }
                    break;
                case ScreenMode.Employee:
                    foreach (dynamic i in info)
                    {
                        switch (i)
                        {
                            case TextBox:
                                if (string.IsNullOrEmpty(i.Text))
                                {
                                    isempty = true;
                                }
                                continue;
                            case ComboBox:
                                if (string.IsNullOrEmpty(i.Text))
                                {
                                    isempty = true;
                                }
                                continue;
                        }
                    }
                    break;
            }

            return isempty;
        }

        private Customer LoadCustomer(Guid id)
        {
            Customer loadedcustomer = new Customer();
            loadedcustomer.Id = id;
            loadedcustomer.City = txtCity.Text;
            loadedcustomer.Email = txtEmail.Text;
            loadedcustomer.FirstName = txtFirstName.Text;
            loadedcustomer.LastName = txtLastName.Text;
            loadedcustomer.Phone = txtPhone.Text;
            loadedcustomer.PropertySqFeet = Convert.ToInt32(txtProperty.Text);
            loadedcustomer.State = cboState.SelectedValue.ToString();
            loadedcustomer.StreetAddress = txtStreetAddress.Text;
            loadedcustomer.UserId = (Guid)cboUser.SelectedValue;
            loadedcustomer.ZipCode = txtZipCode.Text;

            return loadedcustomer;
        }

        private Employee LoadEmployee(Guid id)
        {
            Employee loadedemployee = new Employee();
            loadedemployee.Id = id;
            loadedemployee.City = txtCity.Text;
            loadedemployee.Email = txtEmail.Text;
            loadedemployee.FirstName = txtFirstName.Text;
            loadedemployee.LastName = txtLastName.Text;
            loadedemployee.Phone = txtPhone.Text;
            loadedemployee.State = cboState.SelectedValue.ToString();
            loadedemployee.StreetAddress = txtStreetAddress.Text;
            loadedemployee.UserId = (Guid)cboUser.SelectedValue;
            loadedemployee.ZipCode = txtZipCode.Text;

            return loadedemployee;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (screenMode)
                {
                    case ScreenMode.Customer:
                        HttpClient client = InitializeClient();
                        customer = LoadCustomer(customerId);
                        string serializedObject = JsonConvert.SerializeObject(customer);
                        var content = new StringContent(serializedObject);
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = client.PutAsync("Customer/", content).Result;
                        var result = response.Content.ReadAsStringAsync().Result;

                        if (result == "1")
                        {
                            _owner.RefreshDataGrid();
                            _owner.ChangeStatus("Customer Succesfully Updated");
                            this.Close();
                            customer = null;
                        }
                        break;
                    case ScreenMode.Employee:
                        client = InitializeClient();
                        employee = LoadEmployee(employeeId);
                        serializedObject = JsonConvert.SerializeObject(employee);
                        content = new StringContent(serializedObject);
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        response = client.PutAsync("Employee/", content).Result;
                        result = response.Content.ReadAsStringAsync().Result;

                        if (result == "1")
                        {
                            _owner.RefreshDataGrid();
                            _owner.ChangeStatus("Customer Succesfully Updated");
                            this.Close();
                            employee = null;
                        }
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
