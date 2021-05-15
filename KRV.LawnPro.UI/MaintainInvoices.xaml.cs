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
    /// <summary>
    /// Interaction logic for MaintainInvoices.xaml
    /// </summary>
    public partial class MaintainInvoices : Window
    {
        Invoice invoice;
        Guid customerId;
        Guid invoiceId;
        LawnProMainWindow _owner;

        public MaintainInvoices(LawnProMainWindow owner)
        {
            InitializeComponent();
            _owner = owner;
            MoveButtons(btnAdd, 0);
            LoadScreen();
            dpStartTime.Focus();

            lblStatus.Content = "";
            lblTitle.Content = "Invoice";
            this.Title = "Maintain Invoice";
        }

        public MaintainInvoices(LawnProMainWindow owner, Invoice invoice)
        {
            InitializeComponent();
            _owner = owner;
            MoveButtons(btnUpdate, 0);
            LoadScreen();
            FillInformation(invoice);

            invoiceId = invoice.Id;
            customerId = invoice.CustomerId;

            lblStatus.Content = "";
            lblTitle.Content = "Invoice";
            this.Title = "Maintain Invoice";
        }

        public MaintainInvoices(LawnProMainWindow owner, Invoice invoice, bool magnify)
        {
            InitializeComponent();
            _owner = owner;
            btnAdd.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Hidden;
            LoadScreen();
            FillInformation(invoice);
            JustMagnify();

            lblStatus.Content = "";
            lblTitle.Content = "Invoice";
            this.Title = "Inspect Invoice";
        }

        private static HttpClient InitializeClient()
        {
            HttpClient client = new HttpClient();
            // Will need to update address as needed, start API with UI
            client.BaseAddress = new Uri("https://localhost:44375/");
            return client;
        }

        private void MoveButtons(Button button, int margin)
        {
            if(button == btnAdd)
            {
                Thickness m = btnAdd.Margin;
                m.Left = margin;
                btnAdd.Margin = m;

                btnUpdate.Visibility = Visibility.Hidden;
                btnAdd.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if(button == btnUpdate)
            {
                Thickness m = btnUpdate.Margin;
                m.Right = margin;
                btnUpdate.Margin = m;

                btnAdd.Visibility = Visibility.Hidden;
                btnUpdate.HorizontalAlignment = HorizontalAlignment.Center;
            }

        }

        private void JustMagnify()
        {
            btnUpdate.IsEnabled = false;
            btnUpdate.Visibility = Visibility.Hidden;
            txtAddress.Focusable = false;
            cboStatus.Focusable = false;
            cboEmployee.Focusable = false;
            cboService.Focusable = false;
            txtPropSize.Focusable = false;
            txtRate.Focusable = false;
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
            cboEmployee.SelectedValuePath = "FullName";

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

        private void FillInformation(Invoice invoice)
        {
            //populate the combo boxes
            cboCustomer.SelectedValue = invoice.CustomerId;
            cboEmployee.SelectedValue = invoice.EmployeeFullName;
            cboService.Text = invoice.ServiceType;
            cboStatus.SelectedValue = invoice.Status;

            // populate the text boxes
            txtRate.Text = GetRate((Guid)cboService.SelectedValue).ToString();
            Customer cus = GetCustomerInfo(invoice.CustomerId);
            txtAddress.Text = cus.FullAddress;
            txtPropSize.Text = cus.PropertySqFeet.ToString();

            //populate the Date picker
            dpStartTime.SelectedDate = invoice.ServiceDate;
        }

        private decimal GetRate(Guid stId)
        {
            HttpClient client = InitializeClient();
            HttpResponseMessage response;

            response = client.GetAsync("ServiceType/" + stId).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = JsonConvert.DeserializeObject(result);
            var data = items.ToObject<ServiceType>();

            ServiceType st = new ServiceType();
            st = data;

            return st.CostPerSQFT;
        }

        private Customer GetCustomerInfo(Guid cusId)
        {
            HttpClient client = InitializeClient();
            HttpResponseMessage response;

            response = client.GetAsync("Customer/byId/" + cusId).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = JsonConvert.DeserializeObject(result);
            var data = items.ToObject<Customer>();

            Customer cus = new Customer();
            cus = data;

            return cus;
        }

        public class Combo
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private IEnumerable<Combo> MakeStatusComboBox()
        {
            var datasource = new List<Combo>();
            datasource.Add(new Combo { Name = "Draft", Value = "Draft" });
            datasource.Add(new Combo { Name = "Issued", Value = "Issued" });
            datasource.Add(new Combo { Name = "Paid", Value = "Paid" });
            datasource.Add(new Combo { Name = "Void", Value = "Void" });
            IEnumerable<Combo> result = datasource.OrderBy(datasource => datasource.Name);
            return result;
        }

        private bool CheckScreen()
        {
            bool isempty = false;

            dynamic[] info = new dynamic[7];

            info[0] = txtRate;
            info[1] = cboCustomer;
            info[2] = cboEmployee;
            info[3] = cboService;
            info[4] = cboStatus;
            info[5] = txtAddress;
            info[6] = txtPropSize;

            foreach(dynamic inf in info)
            {
                switch(inf)
                {
                    case TextBox:
                        if(string.IsNullOrEmpty(inf.Text))
                        {
                            isempty = true;
                        }
                        continue;
                    case ComboBox:
                        if(string.IsNullOrEmpty(inf.Text))
                        {
                            isempty = true;
                        }
                        continue;
                }
            }
            return isempty;
        }

        private bool AreAnyObjectsSet(dynamic dynam)
        {
            foreach(object ob in dynam)
            {
                if(ob.GetType() == null)
                {
                    return false;
                }
            }

            return true;
        }

        private int AddData(Invoice invoice)
        {
            bool result = AreAnyObjectsSet(invoice);

                invoice.CustomerId = (Guid)cboCustomer.SelectedValue;
                invoice.ServiceType = cboService.Text.ToString();
                invoice.ServiceRate = Convert.ToDecimal(txtRate.Text);
                invoice.ServiceDate = (DateTime)dpStartTime.SelectedDate;
                invoice.EmployeeFullName = cboEmployee.SelectedValue.ToString();
                invoice.Status = cboStatus.SelectedValue.ToString();

                HttpClient client = InitializeClient();
                string SerializedObject = JsonConvert.SerializeObject(invoice);
                var content = new StringContent(SerializedObject);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = client.PostAsync("Invoice", content).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return 1;
                }
                else
                {
                    return 0;
                } 
        }

        private bool PropSize(TextBox text)
        {
            if (text.Text == 0.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Invoice LoadInvoice(Guid id)
        {
            Invoice loadedInvoice = new Invoice();
            loadedInvoice.Id = id;
            loadedInvoice.CustomerId = (Guid)cboCustomer.SelectedValue;
            loadedInvoice.ServiceType = cboService.Text;
            loadedInvoice.ServiceRate = Convert.ToDecimal(txtRate.Text);
            loadedInvoice.ServiceDate = (DateTime)dpStartTime.SelectedDate;
            loadedInvoice.EmployeeFullName = cboEmployee.SelectedValue.ToString();
            loadedInvoice.Status = cboStatus.SelectedValue.ToString();
            loadedInvoice.PropertySqFt = Convert.ToInt32(txtPropSize.Text);

            return loadedInvoice;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(PropSize(txtPropSize))
                {
                    lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatus.Content = "That Customer has not had a Consultation yet.";
                    LoadScreen();
                    txtAddress.Text = string.Empty;
                    txtPropSize.Text = string.Empty;
                    txtRate.Text = string.Empty;
                    return;
                }
                if(CheckScreen())
                {
                    lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatus.Content = "Please Fill out all the Information.";
                    LoadScreen();
                    txtAddress.Text = string.Empty;
                    txtPropSize.Text = string.Empty;
                    txtRate.Text = string.Empty;
                    return;
                }
                invoice = new Invoice();
                int result = AddData(invoice);
                if (result == 1)
                {
                    _owner.RefreshDataGrid();
                    _owner.ChangeStatus("Invoice Successfully Added.");
                    invoice = null;
                    this.Close(); 
                }
                else
                {
                    lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatus.Content = "Invoice Failed to Add";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                lblStatus.Content = ex.Message;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpClient client = InitializeClient();
                invoice = LoadInvoice(invoiceId);
                if(invoice.PropertySqFt == 0)
                {
                    lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatus.Content = "Cannot Update Customer Invoice before a Consultation";
                    return;
                }
                string serializedObject = JsonConvert.SerializeObject(invoice);
                var content = new StringContent(serializedObject);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = client.PutAsync("Invoice/", content).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                if(result == "1")
                {
                    _owner.RefreshDataGrid();
                    _owner.ChangeStatus("Invoice Succesfully Updated");
                    this.Close();
                    invoice = null;
                }
                else
                {
                    _owner.RefreshDataGrid();
                    _owner.ChangeStatus("Invoice Succesfully Updated");
                    this.Close();
                    invoice = null;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                lblStatus.Content = ex.Message;
            }
        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cboCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboCustomer.SelectedIndex != -1)
            {
                Customer cus = new Customer();
                cus = GetCustomerInfo((Guid)cboCustomer.SelectedValue);
                txtPropSize.Text = cus.PropertySqFeet.ToString();
                txtAddress.Text = cus.FullAddress;
            }
        }

        private void cboService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboService.SelectedIndex != -1)
            {
                txtRate.Text = GetRate((Guid)cboService.SelectedValue).ToString();
            }

        }

    }
}
