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
using KRV.LawnPro.BL.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace KRV.LawnPro.UI
{
    /// <summary>
    /// Interaction logic for MaintainServiceTypes.xaml
    /// </summary>
    public partial class MaintainServiceTypes : Window
    {
        ServiceType serviceType;
        Guid serviceTypeId;
        LawnProMainWindow _owner;
        bool update = false;

        public MaintainServiceTypes(LawnProMainWindow owner)
        {
            InitializeComponent();
            _owner = owner;
            MoveButtons(btnAdd, 0);
            txtDescription.Focus();

            lblTitle.Content = "Service Type";
            this.Title = "Maintain Service Type";
        }

        public MaintainServiceTypes(LawnProMainWindow owner, ServiceType serviceType)
        {
            InitializeComponent();
            _owner = owner;
            update = true;
            MoveButtons(btnUpdate, 0);
            FillTextBoxes(serviceType);

            serviceTypeId = serviceType.Id;

            lblTitle.Content = "Service Type";
            this.Title = "Maintain Service Type";
        }

        public MaintainServiceTypes(LawnProMainWindow owner, ServiceType serviceType, bool maginfy)
        {
            InitializeComponent();
            _owner = owner;
            btnAdd.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Hidden;
            FillTextBoxes(serviceType);
            JustMagnify();

            lblTitle.Content = "Service Type";
            this.Title = "Inspect Service Type";
        }

        public class Combo
        {
            public string Name { get; set; }
            public string Value { get; set; }
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

        private static HttpClient InitializeClient()
        {
            HttpClient client = new HttpClient();
            // Will need to update address as needed, start API with UI
            client.BaseAddress = new Uri("https://localhost:44375/");
            return client;
        }

        private IEnumerable<Combo> MakeCostComboBox(List<ServiceType> serviceTypes)
        {
            var datasource = new List<Combo>();

            foreach(ServiceType st in serviceTypes)
            {
                decimal cost = new decimal();
                cost = st.CostPerSQFT;
                datasource.Add(new Combo { Name = cost.ToString(), Value = cost.ToString() });
            }

            IEnumerable<Combo> result = datasource.OrderBy(datasource => datasource.Name);
            return result;
        }

        private void JustMagnify()
        {
            btnAdd.IsEnabled = false;
            btnAdd.Visibility = Visibility.Hidden;
            btnUpdate.IsEnabled = false;
            btnUpdate.Visibility = Visibility.Hidden;
            txtCost.Focusable = false;
            txtDescription.Focusable = false;
        }

        private void FillTextBoxes(ServiceType serviceType)
        {
            txtCost.Text = serviceType.CostPerSQFT.ToString();
            txtDescription.Text = serviceType.Description;
            update = false;
        }

        private bool CheckScreen()
        {
            bool isempty = false;

            TextBox[] textboxes = new TextBox[2]; ;
            textboxes[0] = txtCost;
            textboxes[1] = txtDescription;

            foreach (TextBox tb in textboxes)
            {
                if (string.IsNullOrEmpty(tb.Text))
                {
                    isempty = true;
                }
                continue;
            }

            return isempty;
        }


        private bool AddData(ServiceType serviceType)
        {
           
            decimal.TryParse(txtCost.Text, out decimal result);
            if(result == 0)
            {
                lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                lblStatus.Content = "Please Fill out a correct Rate";
                return false;
            }
            serviceType.CostPerSQFT = result;
            serviceType.Description = txtDescription.Text;
            

            HttpClient client = InitializeClient();
            string SerializedObject = JsonConvert.SerializeObject(serviceType);
            var content = new StringContent(SerializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync("ServiceType", content).Result;

            return true;
        }


        private ServiceType GetInfo(Guid stId)
        {
            HttpClient client = InitializeClient();
            HttpResponseMessage response;

            ServiceType st = new ServiceType();
            response = client.GetAsync("ServiceType/" + stId).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            dynamic items = JsonConvert.DeserializeObject(result);
            var data = items.ToObject<ServiceType>();
            st = data;

            return st;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(CheckScreen())
                {
                    lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatus.Content = "Please Fill out all the information";
                    return;
                }
                serviceType = new ServiceType();
                bool correctInfo = AddData(serviceType);

                if (correctInfo == true)
                {
                    _owner.RefreshDataGrid();
                    _owner.ChangeStatus("Service Type Successfully Added.");
                    serviceType = null;
                    this.Close(); 
                }
                else
                {
                    lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatus.Content = "Please Fill out a correct Rate";
                    txtCost.Text = string.Empty;
                    txtCost.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                lblStatus.Content = ex.Message;
            }
        }

        private ServiceType LoadServiceType(Guid id)
        {
            ServiceType loadedServiceType = new ServiceType();
            loadedServiceType.Id = id;
            loadedServiceType.Description = txtDescription.Text;
            loadedServiceType.CostPerSQFT = Convert.ToDecimal(txtCost.Text);

            return loadedServiceType;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpClient client = InitializeClient();
                serviceType = LoadServiceType(serviceTypeId);
                string serializedObject = JsonConvert.SerializeObject(serviceType);
                var content = new StringContent(serializedObject);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = client.PutAsync("ServiceType/", content).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                if (result == "1")
                {
                    _owner.RefreshDataGrid();
                    _owner.ChangeStatus("Service Type Succesfully Updated");
                    this.Close();
                    serviceType = null;
                }
                else
                {
                    _owner.RefreshDataGrid();
                    _owner.ChangeStatus("Service Type Failed Updating");
                    this.Close();
                    serviceType = null;
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
