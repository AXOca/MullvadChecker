using System.Media;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;

namespace MullvadChecker
{
    internal class StatusChecker
    {
        private TextBlock _statusText;
        private TextBlock _serverText;
        private TextBlock _ipText;

        public StatusChecker(TextBlock statusText, TextBlock serverText, TextBlock ipText)
        {
            _statusText = statusText;
            _serverText = serverText;
            _ipText = ipText;
        }

        public void UpdateStatus(string response)
        {
            if (response.Contains("You are not connected to Mullvad"))
            {
                UpdateDisconnectedStatus(response);
            }
            else
            {
                UpdateConnectedStatus(response);
            }
        }

        private void UpdateDisconnectedStatus(string response)
        {
            var match = Regex.Match(response, @"Your IP address is (.+)");
            var ip = match.Success ? match.Groups[1].Value : "Unknown";

            UpdateStatusText("Status: Not Connected", Brushes.Red);
            _serverText.Text = "Server: none";
            _ipText.Text = $"IP: {ip}";
            SystemSounds.Exclamation.Play();
        }

        private void UpdateConnectedStatus(string response)
        {
            var match = Regex.Match(response, @"You are connected to Mullvad \(server (.+?)\)\. Your IP address is (.+)");
            if (match.Success)
            {
                var server = match.Groups[1].Value;
                var ip = match.Groups[2].Value;

                UpdateStatusText("Status: Connected", Brushes.Green);
                _serverText.Text = $"Server: {server}";
                _ipText.Text = $"IP: {ip}";
            }
        }

        private void UpdateStatusText(string text, Brush color)
        {
            _statusText.Dispatcher.Invoke(() =>
            {
                _statusText.Text = text;
                _statusText.Foreground = color;
            });
        }

        public void DisplayErrorStatus()
        {
            UpdateStatusText("Status: Error checking Mullvad status.", Brushes.Red);
            _serverText.Text = "Server: Unknown";
            _ipText.Text = "IP: Unknown";
        }
    }
}
