using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Api.Domain;
using Fi.Patika.Schema.Model;
using Fi.Patika.Api.Impl;
using Fi.ApiBase.Controller;
using Fi.Mediator.Message;
using Fi.ApiBase.Attribute;
using Fi.Infra.Schema.Const;
using Fi.Infra.Const;
using Fi.Infra.Context;
using Fi.Infra.Schema.Model;

namespace Fi.Patika.Api.Controllers
{
    [ApiController]
    [Route("api/v1/Patika/[controller]")]
    public class AccountCreditsController : ApiControllerBase<AccountCreditsController>
    {
        public AccountCreditsController(ApiControllerDI<AccountCreditsController> baseDI) : base(baseDI)
        {
        }

        [ApiKey("f5316152-835b-4722-8598-1c00ec6e8091")]
        [ApiAuthorizationAttribute(ScopeKeys.List_Patika)]
        [HttpGet("ByParameters")]
        public async Task<ApiResponse<List<AccountCreditOutputModel>>> GetByParameters()
        {
            var cmd = new GetAccountCreditByParametersQuery();

            var result = await base.Execute<List<AccountCreditOutputModel>>(cmd);

            return result;
        }

        [ApiKey("040d69ee-c731-4c99-81fd-52a919fd137e")]
        [ApiAuthorizationAttribute(ScopeKeys.Create_Patika)]
        [HttpPost]
        public async Task<ApiResponse<AccountCreditOutputModel>> Create([FromBody]AccountCreditInputModel model)
        {
            var cmd = new CreateAccountCreditCommand(model);

            var result = await base.Execute<AccountCreditOutputModel>(cmd);

            return result;
        }

    }
}