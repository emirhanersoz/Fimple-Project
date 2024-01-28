using Fi.Patika.Schema.Model;
using FizzWare.NBuilder;
using System.Security.Cryptography;
using Fi.Patika.Api.Domain.Entity;
using Fi.Patika.Api.IntegrationTests.Initialization;
using Fi.Test.Extensions;
using Newtonsoft.Json;
using Should;
using Xunit;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using Fi.ApiBase.Controller;

namespace Fi.Patika.Api.IntegrationTests.Controllers
{
    public class HelperMethodsForTests : PatikaScenariosBase
    {
        public HelperMethodsForTests(PatikaApplicationFactory fiTestApplicationFactory) : base(fiTestApplicationFactory)
        {
        }

        public byte[] GeneratorByteCodes()
        {
            int byteCount = 16;
            byte[] byteArray = new byte[byteCount];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(byteArray);
            }

            return byteArray;
        }
    }
}
