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
    public class AccountsController : ApiControllerBase<AccountsController>
    {
        public AccountsController(ApiControllerDI<AccountsController> baseDI) : base(baseDI)
        {
        }

        [ApiKey("176a17e7-00f6-4faf-a5fc-246ac3b8a1b9")]
        [ApiAuthorizationAttribute(ScopeKeys.View_Patika)]
        [HttpGet("{Id:int}")]
        public async Task<ApiResponse<AccountOutputModel>> GetByKey(int Id)
        {
            var cmd = new GetAccountByKeyQuery(Id);

            var result = await base.Execute<AccountOutputModel>(cmd);

            return result;
        }

        [ApiKey("6cb911ce-0623-491e-b151-70fd47d03c8d")]
        [ApiAuthorizationAttribute(ScopeKeys.List_Patika)]
        [HttpGet("ByParameters")]
        public async Task<ApiResponse<List<AccountOutputModel>>> GetByParameters()
        {
            var cmd = new GetAccountByParametersQuery();

            var result = await base.Execute<List<AccountOutputModel>>(cmd);

            return result;
        }

        [ApiKey("af0e1b50-616b-47db-b64e-f20f4e3958e4")]
        [ApiAuthorizationAttribute(ScopeKeys.Create_Patika)]
        [HttpPost]
        public async Task<ApiResponse<AccountOutputModel>> Create([FromBody]AccountInputModel model)
        {
            var cmd = new CreateAccountCommand(model);

            var result = await base.Execute<AccountOutputModel>(cmd);

            return result;
        }

        [ApiKey("6a047881-ddfa-4d2f-981c-28fb0cc39356")]
        [ApiAuthorizationAttribute(ScopeKeys.Update_Patika)]
        [HttpPut("{Id:int}")]
        public async Task<ApiResponse<AccountOutputModel>> Update(int Id, [FromBody]AccountInputModel model)
        {
            var cmd = new UpdateAccountCommand(Id, model);

            var result = await base.Execute<AccountOutputModel>(cmd);

            return result;
        }

        //If you really need delete api, take the [OnlyForDevelopment] tag as comment. Or you can delete the API completely.
        [OnlyForDevelopment]
        [ApiKey("805120e0-22eb-42f9-9c5d-e988d34bdc83")]
        [ApiAuthorizationAttribute(ScopeKeys.Delete_Patika)]
        [HttpDelete("{Id:int}")]
        public async Task<ApiResponse> DeleteByKey(int Id)
        {
            var cmd = new DeleteAccountCommand(Id);

            var result = await base.Execute<VoidResult>(cmd);

            return result;
        }

    }
}