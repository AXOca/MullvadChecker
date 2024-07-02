# Mullvad VPN Checker

Mullvad VPN once showed me as "Connected" while it actually leaked my IP and was in a disconnected state for 30 minutes. Reconnecting fixed the issue, but I didn't want to let this happen again. Therefore, I created this tool.

This application checks your Mullvad VPN connection status periodically and updates you on your connection status, server, and IP address. It also provides an option to start checking automatically on Windows startup.

## Features
- Periodically checks Mullvad VPN connection status.
- Updates the connection status, server, and IP address in real-time.
- Provides an option to start checking automatically on Windows startup.
- Alerts you if the VPN is not connected and your IP is being leaked.