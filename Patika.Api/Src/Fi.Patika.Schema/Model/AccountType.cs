
using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Utility;

namespace Fi.Patika.Schema.Model
{
    public class AccountType : FiSmartEnum<AccountType, byte>
    {
        public static readonly AccountType Checking = new ("Checking", 1);
        public static readonly AccountType Savings = new ("Savings", 2);
        public static readonly AccountType Business = new ("Business", 3);
        public static readonly AccountType Investment = new ("Investment", 4);
        public static readonly AccountType ForeignCurrency = new ("ForeignCurrency", 5);
        public static readonly AccountType Deposit = new ("Deposit", 6);

        public AccountType(string code, byte value, string name = null) : base(code, value, name)
        {
        }
    }
    
}