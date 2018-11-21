using Models.Core.Employment;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlipperAuthorization
{
    public static class SessionCache
    {
        static public Dictionary<string, Employee> Employees { get; set; } = new Dictionary<string, Employee>();
    }
}
