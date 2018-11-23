using Models.Core.Employment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Klipper.Desktop.Service.Session
{
    static public class SessionContext
    {
        static public Employee CurrentSubject { get; set; } = null;
    }
}
