
using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Utility;

namespace Fi.Patika.Schema.Model
{
    public class PayeeType : FiSmartEnum<PayeeType, byte>
    {
        public static readonly PayeeType Phone = new("Phone", 1);
        public static readonly PayeeType Internet = new("Internet", 2);
        public static readonly PayeeType Electric = new("Electric", 3);
        public static readonly PayeeType Water = new("Water", 4);
        public static readonly PayeeType Gas = new("Gas", 5);

        public PayeeType(string code, byte value, string name = null) : base(code, value, name)
        {
        }
    }
    
}