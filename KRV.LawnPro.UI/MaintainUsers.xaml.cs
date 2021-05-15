using KRV.LawnPro.BL.Models;
using Newtonsoft.Json;
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
    /// Interaction logic for MaintainUsers.xaml
    /// </summary>
    public partial class MaintainUsers : Window
    {
        User user;
        Guid userId;
        LawnProMainWindow _owner;

        public MaintainUsers(LawnProMainWindow owner)
        {
            InitializeComponent();
            _owner = owner;
            txtFirstName.Focus();
            MoveButtons(btnAdd, 0);

            lblTitle.Content = "User";
            this.Title = "Maintain User";
        }

        public MaintainUsers(LawnProMainWindow owner, User user)
        {
            InitializeComponent();
            _owner = owner;
            MoveButtons(btnUpdate, 0);
            FillTextBoxes(user);
            userId = user.Id;

            lblTitle.Content = "User";
            this.Title = "Maintain User";
        }

        public MaintainUsers(LawnProMainWindow owner, User user, bool magnify)
        {
            InitializeComponent();
            _owner = owner;
            btnAdd.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Hidden;
            FillTextBoxes(user);
            JustMagnify();
            lblTitle.Content = "Lawn Pro : User";
            this.Title = "Inspect User";
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

        private void JustMagnify()
        {
            btnAdd.IsEnabled = false;
            btnAdd.Visibility = Visibility.Hidden;
            btnUpdate.IsEnabled = false;
            btnUpdate.Visibility = Visibility.Hidden;
            txtPassword.IsEnabled = false;
            txtPasswordCheck.IsEnabled = false;
            txtFirstName.Focusable = false;
            txtLastName.Focusable = false;
            txtUsername.Focusable = false;
        }

        private void FillTextBoxes(User user)
        {
            txtFirstName.Text = user.FirstName;
            txtLastName.Text = user.LastName;
            txtUsername.Text = user.UserName;
        }

        private bool CheckScreen()
        {
            bool isempty = false;

            TextBox[] textboxes = new TextBox[5];;
            textboxes[0] = txtFirstName;
            textboxes[1] = txtLastName;
            textboxes[2] = txtUsername;
            textboxes[3] = txtPassword;
            textboxes[4] = txtPasswordCheck;

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

        private int AddData(User user)
        {
            if(txtPassword.Text != txtPasswordCheck.Text)
            {
                return 0;
            }

            user.FirstName = txtFirstName.Text;
            user.LastName = txtLastName.Text;
            user.UserName = txtUsername.Text;
            user.UserPass = txtPassword.Text;
            user.UserPass2 = txtPasswordCheck.Text;


            HttpClient client = InitializeClient();
            string SerializedObject = JsonConvert.SerializeObject(user);
            var content = new StringContent(SerializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync("User", content).Result;
            return 1;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckScreen())
                {
                    lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatus.Content = "Please Fill out all the Information";
                    return;
                }
                user = new User();
                int result = AddData(user);
                if (result == 1)
                {
                    _owner.RefreshDataGrid();
                    _owner.ChangeStatus("User Succesfully Added");
                    this.Close();
                    user = null; 
                }
                else
                {
                    lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatus.Content = "The Passwords do not match";
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
                user = LoadUser(userId);
                if (user != null)
                {
                    string serializedObject = JsonConvert.SerializeObject(user);
                    var content = new StringContent(serializedObject);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = client.PutAsync("User/", content).Result;
                    var result = response.Content.ReadAsStringAsync().Result;

                    if (result == "1")
                    {
                        _owner.RefreshDataGrid();
                        _owner.ChangeStatus("User Succesfully Updated");
                        this.Close();
                        user = null;
                    }
                }
                else
                {
                    lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatus.Content = "Passwords do not match";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Foreground = new SolidColorBrush(Colors.Red);
                lblStatus.Content = ex.Message;
            }
        }

        private User LoadUser(Guid id)
        {
            if(txtPassword.Text != txtPasswordCheck.Text)
            {
                return null;
            }
            User loadedUser = new User();
            loadedUser.Id = id;
            loadedUser.FirstName = txtFirstName.Text;
            loadedUser.LastName = txtLastName.Text;
            loadedUser.UserName = txtUsername.Text;
            loadedUser.UserPass = txtPassword.Text;
            loadedUser.UserPass2 = txtPasswordCheck.Text;

            return loadedUser;
        }
    }
}
