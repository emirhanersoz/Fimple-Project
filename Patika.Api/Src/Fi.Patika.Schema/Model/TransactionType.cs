
using System;
using System.Collections.Generic;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Attributes;
using Newtonsoft.Json;
using Fi.Infra.Schema.Utility;

namespace Fi.Patika.Schema.Model
{
    public class TransactionType : FiSmartEnum<TransactionType, byte>
    {
        public static readonly TransactionType Withdraw = new("Withdraw", 1);
        public static readonly TransactionType Deposit = new("Deposit", 2);

        public TransactionType(string code, byte value, string name = null) : base(code, value, name)
        {
        }
    }
    
}