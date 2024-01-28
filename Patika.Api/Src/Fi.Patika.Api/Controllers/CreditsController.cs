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
    public class CreditsController : ApiControllerBase<CreditsController>
    {
        public CreditsController(ApiControllerDI<CreditsController> baseDI) : base(baseDI)
        {
        }

        [ApiKey("6ade85be-16ab-450b-a0ba-929da8f9414c")]
        [ApiAuthorizationAttribute(ScopeKeys.View_Credit)]
        [HttpGet("{Id:int}")]
        public async Task<ApiResponse<CreditOutputModel>> GetByKey(int Id)
        {
            var cmd = new GetCreditByKeyQuery(Id);

            var result = await base.Execute<CreditOutputModel>(cmd);

            return result;
        }

        [ApiKey("eed21e39-1447-43fd-a9e6-727ffc0401ab")]
        [ApiAuthorizationAttribute(ScopeKeys.List_Credit)]
        [HttpGet("ByParameters")]
        public async Task<ApiResponse<List<CreditOutputModel>>> GetByParameters()
        {
            var cmd = new GetCreditByParametersQuery();

            var result = await base.Execute<List<CreditOutputModel>>(cmd);

            return result;
        }

        [ApiKey("46b6551e-0cc3-415b-9ca1-3accdf0bf750")]
        [ApiAuthorizationAttribute(ScopeKeys.Create_Credit)]
        [HttpPost]
        public async Task<ApiResponse<CreditOutputModel>> Create([FromBody]CreditInputModel model)
        {
            var cmd = new CreateCreditCommand(model);

            var result = await base.Execute<CreditOutputModel>(cmd);

            return result;
        }

        [ApiKey("7d24ab98-645b-469e-a572-661eda22318e")]
        [ApiAuthorizationAttribute(ScopeKeys.Update_Credit)]
        [HttpPut("{Id:int}")]
        public async Task<ApiResponse<CreditOutputModel>> Update(int Id, [FromBody]CreditInputModel model)
        {
            var cmd = new UpdateCreditCommand(Id, model);

            var result = await base.Execute<CreditOutputModel>(cmd);

            return result;
        }

        //If you really need delete api, take the [OnlyForDevelopment] tag as comment. Or you can delete the API completely.
        [OnlyForDevelopment]
        [ApiKey("299283ed-6df1-40ee-8edf-87ebebfdde67")]
        [ApiAuthorizationAttribute(ScopeKeys.Delete_Credit)]
        [HttpDelete("{Id:int}")]
        public async Task<ApiResponse> DeleteByKey(int Id)
        {
            var cmd = new DeleteCreditCommand(Id);

            var result = await base.Execute<VoidResult>(cmd);

            return result;
        }

    }
}