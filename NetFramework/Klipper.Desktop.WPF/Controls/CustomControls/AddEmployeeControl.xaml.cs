using Models.Core;
using Models.Core.Employment;
using Newtonsoft.Json;
using Sparkle.Controls.Dialogs;
using Sparkle.DataStructures;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Klipper.Desktop.WPF.CustomControls
{
    /// <summary>
    /// Interaction logic for AddEmployeeControl.xaml
    /// </summary>
    public partial class AddEmployeeControl : UserControl
    {
        public event EventHandler Closed = null;
        private int _SelectedIndex = 0;
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
            BirthdateTextBox.Text = DateTime.Now.ToShortDateString();
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
                Employee empData = new Employee()
                {
                    ID = Convert.ToInt32(IDTextBox.Text),
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    Gender = (Gender)_selectedItem,
                    Prefix = PrefixComboBox.Text,
                    Title = TitleTextBox.Text,
                    BirthDate = (DateTime)BirthdateTextBox.SelectedDate,
                    Email = EmailTextBox.Text,
                    MobilePhone = MobilenumberTextBox.Text,
                    WorkPhone = WorkPhoneTextBox.Text,
                    ProvidentFundNumber = PFNumberTextBox.Text,
                    ProvidentFundUANNumber = PFUANNumberTextBox.Text,
                    PANNumber = PanNumberTextBox.Text,
                    AadharNumber = AadharNumberTextBox.Text,
                    JoiningDate = DateTime.UtcNow,
                };
                using (HttpClient _client = new HttpClient())
                {
                    _client.BaseAddress = new Uri("https://localhost:6001/");
                    string json = JsonConvert.SerializeObject(empData, Formatting.Indented);
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

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static bool IsTextAllowed(string text) //Input text validation : input only numbers
        {
            Regex _regex = new Regex("[^0-9.-]+");
            return !_regex.IsMatch(text);
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

        private void SaveButton_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
