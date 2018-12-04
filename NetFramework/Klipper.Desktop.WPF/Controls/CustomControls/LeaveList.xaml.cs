using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Klipper.Desktop.WPF.Controls.CustomControls
{
    /// <summary>
    /// Interaction logic for LeaveList.xaml
    /// </summary>
    public partial class LeaveList : UserControl
    {
        public List<User> items { get; set; }
        public LeaveList()
        {
            InitializeComponent();

            // for testing list
            items = new List<User>();
            //List<User> items = new List<User>();
            items.Add(new User() { Name = "Naushad", Age = 24, Mail = "warsinak@gmail.com" });
            items.Add(new User() { Name = "Dhiren", Age = 27, Mail = "nakwarsi@hotmail.com" });
            items.Add(new User() { Name = "axz", Age = 38, Mail = "Kapil@gmail.com" });
            items.Add(new User() { Name = "xzx", Age = 38, Mail = "sxsa@gmail.com" });
            items.Add(new User() { Name = "dsfdg", Age = 355, Mail = "scsds@gmail.com" });
            items.Add(new User() { Name = "fdfd", Age = 65, Mail = "sdsad@gmail.com" });
            items.Add(new User() { Name = "sdfdsf", Age = 77, Mail = "sdad@gmail.com" });
            items.Add(new User() { Name = "sdfds", Age = 38, Mail = "qweqwe@gmail.com" });
            items.Add(new User() { Name = "sdfsdfsd", Age = 35, Mail = "qqwe@gmail.com" });
            items.Add(new User() { Name = "zcxvd", Age = 32, Mail = "rqwewd@gmail.com" });
            items.Add(new User() { Name = "Naushad", Age = 24, Mail = "warsinak@gmail.com" });
            items.Add(new User() { Name = "Dhiren", Age = 27, Mail = "nakwarsi@hotmail.com" });
            items.Add(new User() { Name = "axz", Age = 38, Mail = "Kapil@gmail.com" });
            items.Add(new User() { Name = "xzx", Age = 38, Mail = "sxsa@gmail.com" });
            items.Add(new User() { Name = "dsfdg", Age = 355, Mail = "scsds@gmail.com" });
            items.Add(new User() { Name = "fdfd", Age = 65, Mail = "sdsad@gmail.com" });
            items.Add(new User() { Name = "sdfdsf", Age = 35, Mail = "sdad@gmail.com" });
            items.Add(new User() { Name = "sdfds", Age = 38, Mail = "qweqwe@gmail.com" });
            items.Add(new User() { Name = "sdfsdfsd", Age = 35, Mail = "qqwe@gmail.com" });
            items.Add(new User() { Name = "zcxvd", Age = 32, Mail = "rqwewd@gmail.com" });
            items.Add(new User() { Name = "Naushad", Age = 24, Mail = "warsinak@gmail.com" });
            items.Add(new User() { Name = "Dhiren", Age = 27, Mail = "nakwarsi@hotmail.com" });
            items.Add(new User() { Name = "axz", Age = 38, Mail = "Kapil@gmail.com" });
            items.Add(new User() { Name = "xzx", Age = 38, Mail = "sxsa@gmail.com" });
            items.Add(new User() { Name = "dsfdg", Age = 355, Mail = "scsds@gmail.com" });
            items.Add(new User() { Name = "fdfd", Age = 65, Mail = "sdsad@gmail.com" });
            items.Add(new User() { Name = "sdfdsf", Age = 35, Mail = "sdad@gmail.com" });
            items.Add(new User() { Name = "sdfds", Age = 38, Mail = "qweqwe@gmail.com" });
            items.Add(new User() { Name = "sdfsdfsd", Age = 35, Mail = "qqwe@gmail.com" });
            items.Add(new User() { Name = "zcxvd", Age = 32, Mail = "rqwewd@gmail.com" });
            items.Add(new User() { Name = "Naushad", Age = 24, Mail = "warsinak@gmail.com" });
            items.Add(new User() { Name = "Dhiren", Age = 27, Mail = "nakwarsi@hotmail.com" });
            items.Add(new User() { Name = "axz", Age = 38, Mail = "Kapil@gmail.com" });
            items.Add(new User() { Name = "xzx", Age = 38, Mail = "sxsa@gmail.com" });
            items.Add(new User() { Name = "dsfdg", Age = 355, Mail = "scsds@gmail.com" });
            items.Add(new User() { Name = "fdfd", Age = 65, Mail = "sdsad@gmail.com" });
            items.Add(new User() { Name = "sdfdsf", Age = 35, Mail = "sdad@gmail.com" });
            items.Add(new User() { Name = "sdfds", Age = 38, Mail = "qweqwe@gmail.com" });
            items.Add(new User() { Name = "sdfsdfsd", Age = 35, Mail = "qqwe@gmail.com" });
            items.Add(new User() { Name = "zcxvd", Age = 32, Mail = "rqwewd@gmail.com" });
            ListView1.DataContext = items;
        }
    }
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Mail { get; set; }
    }
}
