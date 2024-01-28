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
    public class PayeesController : ApiControllerBase<PayeesController>
    {
        public PayeesController(ApiControllerDI<PayeesController> baseDI) : base(baseDI)
        {
        }

        [ApiKey("3d816889-7a2f-4ff5-b7fa-2bbc5ddd20ab")]
        [ApiAuthorizationAttribute(ScopeKeys.View_Payee)]
        [HttpGet("{Id:int}")]
        public async Task<ApiResponse<PayeeOutputModel>> GetByKey(int Id)
        {
            var cmd = new GetPayeeByKeyQuery(Id);

            var result = await base.Execute<PayeeOutputModel>(cmd);

            return result;
        }

        [ApiKey("f13ef5c7-f7fb-4114-9d79-75e588fe1f82")]
        [ApiAuthorizationAttribute(ScopeKeys.List_Payee)]
        [HttpGet("ByParameters")]
        public async Task<ApiResponse<List<PayeeOutputModel>>> GetByParameters()
        {
            var cmd = new GetPayeeByParametersQuery();

            var result = await base.Execute<List<PayeeOutputModel>>(cmd);

            return result;
        }

        [ApiKey("abc630ee-ec61-4343-bc5f-8b9549569a99")]
        [ApiAuthorizationAttribute(ScopeKeys.Create_Payee)]
        [HttpPost]
        public async Task<ApiResponse<PayeeOutputModel>> Create([FromBody]PayeeInputModel model)
        {
            var cmd = new CreatePayeeCommand(model);

            var result = await base.Execute<PayeeOutputModel>(cmd);

            return result;
        }

        [ApiKey("ba9df860-4e2a-48cf-b274-03d9036a01f4")]
        [ApiAuthorizationAttribute(ScopeKeys.Payment_Payee)]
        [HttpPut("Payment/{Id:int}")]
        public async Task<ApiResponse<PayeeOutputModel>> Payment(int Id, [FromBody] PayeeInputModel model)
        {
            var cmd = new PaymentPayeeCommand(Id, model);

            var result = await base.Execute<PayeeOutputModel>(cmd);

            return result;
        }

        [ApiKey("87fad470-4840-4b6e-bc99-30ac8affb5aa")]
        [ApiAuthorizationAttribute(ScopeKeys.Update_Payee)]
        [HttpPut("{Id:int}")]
        public async Task<ApiResponse<PayeeOutputModel>> Update(int Id, [FromBody]PayeeInputModel model)
        {
            var cmd = new UpdatePayeeCommand(Id, model);

            var result = await base.Execute<PayeeOutputModel>(cmd);

            return result;
        }

        //If you really need delete api, take the [OnlyForDevelopment] tag as comment. Or you can delete the API completely.
        [OnlyForDevelopment]
        [ApiKey("7fa96219-1d7f-491a-a0ee-1bc091f42abc")]
        [ApiAuthorizationAttribute(ScopeKeys.Delete_Payee)]
        [HttpDelete("{Id:int}")]
        public async Task<ApiResponse> DeleteByKey(int Id)
        {
            var cmd = new DeletePayeeCommand(Id);

            var result = await base.Execute<VoidResult>(cmd);

            return result;
        }

    }
}