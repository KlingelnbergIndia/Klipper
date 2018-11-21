using System;
using System.Collections.Generic;
using System.Text;

namespace Klipper.Desktop.Service.Login
{
    internal class TokenResponse
    {
        public string Token { get; set; } = "";
        public DateTime Expiration { get; set; }
        public string Username { get; set; } = "";
    }
}
