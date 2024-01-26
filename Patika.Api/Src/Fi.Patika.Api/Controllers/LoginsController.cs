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
    public class LoginsController : ApiControllerBase<LoginsController>
    {
        public LoginsController(ApiControllerDI<LoginsController> baseDI) : base(baseDI)
        {
        }

        [ApiKey("2118ac55-22cb-4876-9e0e-01263fc3e7e3")]
        [ApiAuthorizationAttribute(ScopeKeys.View_Patika)]
        [HttpGet("{Id:int}")]
        public async Task<ApiResponse<LoginOutputModel>> GetByKey(int Id)
        {
            var cmd = new GetLoginByKeyQuery(Id);

            var result = await base.Execute<LoginOutputModel>(cmd);

            return result;
        }

        [ApiKey("0c148e71-cb32-4d53-a8db-a1f02a8026e5")]
        [ApiAuthorizationAttribute(ScopeKeys.List_Patika)]
        [HttpGet("ByParameters")]
        public async Task<ApiResponse<List<LoginOutputModel>>> GetByParameters()
        {
            var cmd = new GetLoginByParametersQuery();

            var result = await base.Execute<List<LoginOutputModel>>(cmd);

            return result;
        }

        [ApiKey("51c4f249-2f54-4569-90f8-4f15a65768d8")]
        [ApiAuthorizationAttribute(ScopeKeys.Create_Patika)]
        [HttpPost]
        public async Task<ApiResponse<LoginOutputModel>> Create([FromBody]LoginInputModel model)
        {
            var cmd = new CreateLoginCommand(model);

            var result = await base.Execute<LoginOutputModel>(cmd);

            return result;
        }

        [ApiKey("e025e7e1-ee85-4a9b-a1cd-fb6c0d26a295")]
        [ApiAuthorizationAttribute(ScopeKeys.Update_Patika)]
        [HttpPut("{Id:int}")]
        public async Task<ApiResponse<LoginOutputModel>> Update(int Id, [FromBody]LoginInputModel model)
        {
            var cmd = new UpdateLoginCommand(Id, model);

            var result = await base.Execute<LoginOutputModel>(cmd);

            return result;
        }

        //If you really need delete api, take the [OnlyForDevelopment] tag as comment. Or you can delete the API completely.
        [OnlyForDevelopment]
        [ApiKey("36835544-759e-4e13-b4b9-603e78396e86")]
        [ApiAuthorizationAttribute(ScopeKeys.Delete_Patika)]
        [HttpDelete("{Id:int}")]
        public async Task<ApiResponse> DeleteByKey(int Id)
        {
            var cmd = new DeleteLoginCommand(Id);

            var result = await base.Execute<VoidResult>(cmd);

            return result;
        }

    }
}