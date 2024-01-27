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
    public class DepositAndWithdrawsController : ApiControllerBase<DepositAndWithdrawsController>
    {
        public DepositAndWithdrawsController(ApiControllerDI<DepositAndWithdrawsController> baseDI) : base(baseDI)
        {
        }

        [ApiKey("227337c6-33d5-4fd3-86a0-4c70acd86b58")]
        [ApiAuthorizationAttribute(ScopeKeys.View_Patika)]
        [HttpGet("{Id:int}")]
        public async Task<ApiResponse<DepositAndWithdrawOutputModel>> GetByKey(int Id)
        {
            var cmd = new GetDepositAndWithdrawByKeyQuery(Id);

            var result = await base.Execute<DepositAndWithdrawOutputModel>(cmd);

            return result;
        }

        [ApiKey("0b2b8964-dfe8-441c-aee7-77aef6bcf2f3")]
        [ApiAuthorizationAttribute(ScopeKeys.List_Patika)]
        [HttpGet("ByParameters")]
        public async Task<ApiResponse<List<DepositAndWithdrawOutputModel>>> GetByParameters()
        {
            var cmd = new GetDepositAndWithdrawByParametersQuery();

            var result = await base.Execute<List<DepositAndWithdrawOutputModel>>(cmd);

            return result;
        }

        [ApiKey("6d6f0a5b-dc02-4d03-8085-29c0c9850d39")]
        [ApiAuthorizationAttribute(ScopeKeys.Create_Patika)]
        [HttpPost]
        public async Task<ApiResponse<DepositAndWithdrawOutputModel>> Create([FromBody]DepositAndWithdrawInputModel model)
        {
            var cmd = new CreateDepositAndWithdrawCommand(model);

            var result = await base.Execute<DepositAndWithdrawOutputModel>(cmd);

            return result;
        }

        [ApiKey("6012b350-e7ab-44df-bfa6-76d91ef8073a")]
        [ApiAuthorizationAttribute(ScopeKeys.Create_Patika)]
        [HttpPut("{Id:int}/withdraw")]
        public async Task<ApiResponse<DepositAndWithdrawOutputModel>> Withdraw(int Id, [FromBody] DepositAndWithdrawInputModel model)
        {
            var cmd = new TransactionWithdrawCommand(Id, model);

            var result = await base.Execute<DepositAndWithdrawOutputModel>(cmd);

            return result;
        }

        [ApiKey("8ded44C2-22c4-4986-b1bc-375ad3d24742")]
        [ApiAuthorizationAttribute(ScopeKeys.Create_Patika)]
        [HttpPut("{Id:int}/deposit")]
        public async Task<ApiResponse<DepositAndWithdrawOutputModel>> Deposit(int Id, [FromBody] DepositAndWithdrawInputModel model)
        {
            var cmd = new TransactionDepositCommand(Id, model);

            var result = await base.Execute<DepositAndWithdrawOutputModel>(cmd);

            return result;
        }

        [ApiKey("3a9b5b96-7a15-4469-a2ea-8014737505ee")]
        [ApiAuthorizationAttribute(ScopeKeys.Update_Patika)]
        [HttpPut("{Id:int}")]
        public async Task<ApiResponse<DepositAndWithdrawOutputModel>> Update(int Id, [FromBody]DepositAndWithdrawInputModel model)
        {
            var cmd = new UpdateDepositAndWithdrawCommand(Id, model);

            var result = await base.Execute<DepositAndWithdrawOutputModel>(cmd);

            return result;
        }

        //If you really need delete api, take the [OnlyForDevelopment] tag as comment. Or you can delete the API completely.
        [OnlyForDevelopment]
        [ApiKey("b747992c-9d21-401f-a2e5-5fa501ea0c67")]
        [ApiAuthorizationAttribute(ScopeKeys.Delete_Patika)]
        [HttpDelete("{Id:int}")]
        public async Task<ApiResponse> DeleteByKey(int Id)
        {
            var cmd = new DeleteDepositAndWithdrawCommand(Id);

            var result = await base.Execute<VoidResult>(cmd);

            return result;
        }

    }
}