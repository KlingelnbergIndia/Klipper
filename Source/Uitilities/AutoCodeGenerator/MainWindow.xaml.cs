using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace AutoCodeGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateRepoControllersButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> replacements = new Dictionary<string, string>()
            {
                { "NNNN", NamespaceTextbox.Text },
                { "XXXX", TypeTextbox.Text },
                { "YYYY", ContextTextbox.Text }
            };

            List<string> files = new List<string>()
            {
                "Templates/IXXXXRepository.cs", "Templates/XXXXRepository.cs", "Templates/XXXXsController.cs", "Templates/YYYYContext.cs"
            };

            var directory = @"C:\Temp\WebApiGenCode";
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            foreach (var file in files)
            {
                ReplacePlaceholders(replacements, directory, file);
            }
            Process.Start("explorer.exe", directory);
        }

        private void ReplacePlaceholders(Dictionary<string, string> replacements, string directory, string file)
        {
            var txt = File.ReadAllText(file);
            foreach (var k in replacements.Keys)
            {
                txt = txt.Replace(k, replacements[k]);
            }
            var newFilename = file.Replace("XXXX", TypeTextbox.Text).Replace("YYYY", ContextTextbox.Text).Replace("Templates/", "");
            var newFilePath = Path.Combine(directory, newFilename);
            File.WriteAllText(newFilePath, txt);
        }

        private void GenerateStartupButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> replacements = new Dictionary<string, string>()
            {
                { "NNNN", NamespaceTextbox.Text },
                { "XXXX", TypeTextbox.Text },
                { "YYYY", ContextTextbox.Text }
            };
            ReplacePlaceholders(replacements, @"C:\Temp\WebApiGenCode", "Startup.cs");
            Process.Start("explorer.exe", @"C:\Temp\WebApiGenCode");
        }

        private void CopySerilogButton_Click(object sender, RoutedEventArgs e)
        {
            File.Copy("SerilogMiddleware.cs", Path.Combine(@"C:\Temp\WebApiGenCode", "SerilogMiddleware.cs"), true);
        }

        private void CopyDBSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            File.Copy("DBConnectionSettings.cs", Path.Combine(@"C:\Temp\WebApiGenCode", "DBConnectionSettings.cs"), true);
        }
    }
}
