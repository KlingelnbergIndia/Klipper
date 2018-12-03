using Models.Core;
using Models.Core.Employment;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Klipper.Desktop.WPF.CustomControls
{
    /// <summary>
    /// Interaction logic for EmployeeListPanelControl.xaml
    /// </summary>
    public partial class EmployeeListPanelControl : UserControl
    {
        private readonly byte[] defaultProfileImage;
        public EmployeeListPanelControl()
        {
            InitializeComponent();
           
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapImage DefaultProfileImage = new BitmapImage(new Uri("pack://application:,,,/Images/profilePic.png"));
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(DefaultProfileImage));
                encoder.Save(ms);
                defaultProfileImage = ms.ToArray();
            }
        }

        private async Task<IQueryable> GetAllEmployeesAsync()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:6001/")
            };
            HttpResponseMessage response = await client.GetAsync("/api/Employees");
            string jsonString = await response.Content.ReadAsStringAsync();
            IQueryable jsonData = JsonConvert.DeserializeObject<Employee[]>(jsonString)
                .Select(x => new {
                    x.ID,
                    x.FirstName,
                    x.LastName,
                    x.Title,
                    BirthDate = x.BirthDate.ToShortDateString(),
                    x.Email,
                    FullName = $"{x.Prefix} {x.FirstName} {x.LastName}",
                    Gender = ((Gender)x.Gender).ToString(),
                    Photo = x.Photo ?? defaultProfileImage,
                })
                .OrderByDescending(x=>x.ID)
                .AsQueryable();
            return jsonData;
        }

        private async void EmployeeList_LoadedAsync(object sender, System.Windows.RoutedEventArgs e)
        {
            DataLoaderAnimation.LoadingText = "Loading data...";
            DataLoaderAnimation.SwitchToLoader();
            employeeList.ItemsSource = await GetAllEmployeesAsync();
            await Task.Delay(1000);
            LoaderPanel.Visibility = Visibility.Hidden;
            employeeList.Visibility = Visibility.Visible;
        }
    }
}
