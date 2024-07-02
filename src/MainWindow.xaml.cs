using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using MullvadChecker;
using Microsoft.Win32;

namespace MullvadChecker
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private bool _isChecking = false;
        private const string StartupKey = "MullvadChecker";
        private StatusChecker _statusChecker;

        public MainWindow()
        {
            InitializeComponent();
            CheckStartupStatus();
            _statusChecker = new StatusChecker(StatusText, ServerText, IPText);
            ToggleCheckMullvad.IsChecked = true; // Start checking by default
        }

        private void ToggleCheckMullvad_Checked(object sender, RoutedEventArgs e)
        {
            _isChecking = true;
            ToggleCheckMullvad.Content = "Stop Checking Mullvad Connection";
            StartCheckingMullvadStatus();
        }

        private void ToggleCheckMullvad_Unchecked(object sender, RoutedEventArgs e)
        {
            _isChecking = false;
            ToggleCheckMullvad.Content = "Start Checking Mullvad Connection";
        }

        private async void StartCheckingMullvadStatus()
        {
            while (_isChecking)
            {
                await CheckMullvadStatus();
                await Task.Delay(5000); // Check every 5 seconds
            }
        }

        private async Task CheckMullvadStatus()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://am.i.mullvad.net/connected");
                _statusChecker.UpdateStatus(response);
            }
            catch (Exception ex)
            {
                _statusChecker.DisplayErrorStatus();
                Console.WriteLine($"Error checking Mullvad status: {ex.Message}");
            }
        }

        private void ToggleStartup_Checked(object sender, RoutedEventArgs e)
        {
            SetStartup(true);
        }

        private void ToggleStartup_Unchecked(object sender, RoutedEventArgs e)
        {
            SetStartup(false);
        }

        private void SetStartup(bool enable)
        {
            using (var rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if (rk != null)
                {
                    if (enable)
                    {
                        rk.SetValue(StartupKey, System.Reflection.Assembly.GetExecutingAssembly().Location);
                    }
                    else
                    {
                        rk.DeleteValue(StartupKey, false);
                    }
                }
            }
        }

        private void CheckStartupStatus()
        {
            using (var rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false))
            {
                if (rk != null)
                {
                    ToggleStartup.IsChecked = rk.GetValue(StartupKey) != null;
                }
            }
        }
    }
}
