using Models.Core;
using Models.Core.Employment;
using Newtonsoft.Json;
using Sparkle.Controls.Dialogs;
using Sparkle.DataStructures;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Klipper.Desktop.WPF.CustomControls
{
    /// <summary>
    /// Interaction logic for AddEmployeeControl.xaml
    /// </summary>
    public partial class AddEmployeeControl : UserControl
    {
        public event EventHandler Closed = null;
        private int _SelectedIndex = 0;
        Employee EmployeeModel = InitEmployeeModel();

        private readonly byte[] defaultProfileImage;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged();
            }
        }
        public SelectableItem SelectedItem
        {
            get
            {
                if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
                    return Items[SelectedIndex];
                return null;
            }
            set
            {
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<SelectableItem> Items { get; } = new ObservableCollection<SelectableItem>()
        {
            new SelectableItem("M", null),
            new SelectableItem("F", null),
            //dependent function : ItemSelectionChanged()
        };
        public AddEmployeeControl()
        {
            InitializeComponent();
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapImage DefaultProfileImage = new BitmapImage(new Uri("pack://application:,,,/Images/profilePic.png"));
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(DefaultProfileImage));
                encoder.Save(ms);
                defaultProfileImage = ms.ToArray();
                EmployeeModel.Photo = defaultProfileImage;
            }
            BirthdateTextBox.Text = DateTime.Now.ToShortDateString();
            DataContext = EmployeeModel;
        }

        private void CloseDialog_clicked(object sender, RoutedEventArgs e)
        {
            Closed?.Invoke(this, null);
        }
        private static int _selectedItem { get; set; } = 1;

        private async void AddEmployee_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    _client.BaseAddress = new Uri("https://localhost:6001/");
                    string json = JsonConvert.SerializeObject(EmployeeModel, Formatting.Indented);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _client.PostAsync("/api/Employees/", httpContent);
                    response.EnsureSuccessStatusCode();
                    MessageDialog.Show("Success Message", "Employee saved successsfully !!", true, "Close", false, "", DialogFlavour.Information, true);
                }
            }
            catch (Exception exp)
            {
                MessageDialog.Show("Error Message", exp.Message, true, "Close", false, "", DialogFlavour.Error, true);
            }
        }

        internal async void LoadEmployeeData(int empId)
        {
            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    _client.BaseAddress = new Uri("https://localhost:6001/api/Employees/");
                    HttpResponseMessage response = await _client.GetAsync($"/api/Employees/{empId}");
                    string jsonString = await response.Content.ReadAsStringAsync();
                    EmployeeModel = JsonConvert.DeserializeObject<Employee>(jsonString);
                    if (EmployeeModel.Photo == null)
                    {
                        EmployeeModel.Photo = defaultProfileImage;
                    }
                    DataContext = EmployeeModel; //updated values that have been set
                }
            }
            catch (Exception exp)
            {
                MessageDialog.Show("Error Message", exp.Message, true, "Close", false, "", DialogFlavour.Error, true);
            }
        }


        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex _regex = new Regex("[^0-9.-]+");
            e.Handled = _regex.IsMatch(e.Text);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ItemSelectionChanged(object sender, SelectableItemSelectionChangedEventArgs args)
        {
            string header = args.Current.Header;
            _selectedItem = header == "M" ? 1 : 2;
            OnPropertyChanged(nameof(_selectedItem));
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            var profileImagePicker = new Microsoft.Win32.OpenFileDialog();
            profileImagePicker.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";

            if ((bool)profileImagePicker.ShowDialog())
            {
                byte[] profileImageByteSream = File.ReadAllBytes(profileImagePicker.FileName);
                using (var memStream = new MemoryStream(profileImageByteSream))
                {
                    BitmapFrame image = BitmapFrame.Create(memStream,
                        BitmapCreateOptions.IgnoreImageCache,
                        BitmapCacheOption.OnLoad);
                    ProfileImage.Source = image;
                    EmployeeModel.Photo = profileImageByteSream;
                }
            }
        }

        private static Employee InitEmployeeModel()
        {
            return new Employee()
            {
                ProvidentFundNumber = string.Empty,
                ProvidentFundUANNumber = string.Empty,
                PANNumber = string.Empty,
                AadharNumber = string.Empty,
            };
        }
    }
}
