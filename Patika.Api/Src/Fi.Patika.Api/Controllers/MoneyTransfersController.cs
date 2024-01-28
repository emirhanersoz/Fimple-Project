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
using Fi.Patika.Api.Domain.Entity;

namespace Fi.Patika.Api.Controllers
{
    [ApiController]
    [Route("api/v1/Patika/[controller]")]
    public class MoneyTransfersController : ApiControllerBase<MoneyTransfersController>
    {
        public MoneyTransfersController(ApiControllerDI<MoneyTransfersController> baseDI) : base(baseDI)
        {
        }

        [ApiKey("c361e3ba-20f2-443f-97ba-93f1c438a274")]
        [ApiAuthorizationAttribute(ScopeKeys.View_MoneyTransfer)]
        [HttpGet("{Id:int}")]
        public async Task<ApiResponse<MoneyTransferOutputModel>> GetByKey(int Id)
        {
            var cmd = new GetMoneyTransferByKeyQuery(Id);

            var result = await base.Execute<MoneyTransferOutputModel>(cmd);

            return result;
        }

        [ApiKey("411e7324-c64a-4452-a0ed-b2b3b1d2894c")]
        [ApiAuthorizationAttribute(ScopeKeys.List_MoneyTransfer)]
        [HttpGet("list")]
        public async Task<ApiResponse<List<MoneyTransferOutputModel>>> GetByParameters()
        {
            var cmd = new GetMoneyTransferByParametersQuery();

            var result = await base.Execute<List<MoneyTransferOutputModel>>(cmd);

            return result;
        }

        [ApiKey("44f5c66c-2ba1-4be4-b984-28cbf0de71c2")]
        [ApiAuthorizationAttribute(ScopeKeys.Create_MoneyTransfer)]
        [HttpPost]
        public async Task<ApiResponse<MoneyTransferOutputModel>> Create([FromBody]MoneyTransferInputModel model)
        {
            var cmd = new CreateMoneyTransferCommand(model);

            var result = await base.Execute<MoneyTransferOutputModel>(cmd);

            return result;
        }

        [ApiKey("44f5c66c-2ba1-4be4-b984-28cbf0de71c2")]
        [ApiAuthorizationAttribute(ScopeKeys.Transaction_MoneyTransfer)]
        [HttpPost("transfer")]
        public async Task<ApiResponse<MoneyTransferOutputModel>> TransferMoneyCommand([FromBody] MoneyTransferInputModel model)
        {
            var cmd = new TransferMoneyCommand(model);

            var result = await base.Execute<MoneyTransferOutputModel>(cmd);

            return result;
        }

        [ApiKey("d1c77d99-42f6-4285-b134-58c489B9ff7f")]
        [ApiAuthorizationAttribute(ScopeKeys.Update_MoneyTransfer)]
        [HttpPut("{Id:int}")]
        public async Task<ApiResponse<MoneyTransferOutputModel>> Update(int Id, [FromBody]MoneyTransferInputModel model)
        {
            var cmd = new UpdateMoneyTransferCommand(Id, model);

            var result = await base.Execute<MoneyTransferOutputModel>(cmd);

            return result;
        }

        //If you really need delete api, take the [OnlyForDevelopment] tag as comment. Or you can delete the API completely.
        [OnlyForDevelopment]
        [ApiKey("19ed0299-0f31-4bc6-8786-7a00825fe7d7")]
        [ApiAuthorizationAttribute(ScopeKeys.Delete_MoneyTransfer)]
        [HttpDelete("{Id:int}")]
        public async Task<ApiResponse> DeleteByKey(int Id)
        {
            var cmd = new DeleteMoneyTransferCommand(Id);

            var result = await base.Execute<VoidResult>(cmd);

            return result;
        }
    }
}