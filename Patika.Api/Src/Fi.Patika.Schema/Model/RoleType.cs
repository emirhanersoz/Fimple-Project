
using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Utility;

namespace Fi.Patika.Schema.Model
{
    public class RoleType : FiSmartEnum<RoleType, byte>
    {
        public static readonly RoleType Admin = new("Admin", 1);
        public static readonly RoleType Support = new("Support", 2);
        public static readonly RoleType Auditor = new("Auditor", 3);
        public static readonly RoleType User = new("User", 4);
        public static readonly RoleType PlatinumUser = new("PlatinumUser", 5);
        public static readonly RoleType EliteUser = new("EliteUser", 5);

        public RoleType(string code, byte value, string name = null) : base(code, value, name)
        {
        }
    }
    
}