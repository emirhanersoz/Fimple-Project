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
    public class UsersController : ApiControllerBase<UsersController>
    {
        public UsersController(ApiControllerDI<UsersController> baseDI) : base(baseDI)
        {
        }

        [ApiKey("8ec0e8d4-cabd-437e-8ac1-c52287fee601")]
        [ApiAuthorizationAttribute(ScopeKeys.View_User)]
        [HttpGet("{Id:int}")]
        public async Task<ApiResponse<UserOutputModel>> GetByKey(int Id)
        {
            var cmd = new GetUserByKeyQuery(Id);

            var result = await base.Execute<UserOutputModel>(cmd);

            return result;
        }

        [ApiKey("49c3a221-e86c-4384-bddf-9c4065fafade")]
        [ApiAuthorizationAttribute(ScopeKeys.List_User)]
        [HttpGet("ByParameters")]
        public async Task<ApiResponse<List<UserOutputModel>>> GetByParameters()
        {
            var cmd = new GetUserByParametersQuery();

            var result = await base.Execute<List<UserOutputModel>>(cmd);

            return result;
        }

        [ApiKey("66df23ea-2630-44b1-878b-e1343df0a107")]
        [ApiAuthorizationAttribute(ScopeKeys.Create_User)]
        [HttpPost]
        public async Task<ApiResponse<UserOutputModel>> Create([FromBody]UserInputModel model)
        {
            var cmd = new CreateUserCommand(model);

            var result = await base.Execute<UserOutputModel>(cmd);

            return result;
        }

        [ApiKey("89db007a-9583-4e70-974b-462bd65806a9")]
        [ApiAuthorizationAttribute(ScopeKeys.Update_User)]
        [HttpPut("{Id:int}")]
        public async Task<ApiResponse<UserOutputModel>> Update(int Id, [FromBody]UserInputModel model)
        {
            var cmd = new UpdateUserCommand(Id, model);

            var result = await base.Execute<UserOutputModel>(cmd);

            return result;
        }

        //If you really need delete api, take the [OnlyForDevelopment] tag as comment. Or you can delete the API completely.
        [OnlyForDevelopment]
        [ApiKey("5046bce9-6d4a-4bb4-88e3-a11d5226d803")]
        [ApiAuthorizationAttribute(ScopeKeys.Delete_User)]
        [HttpDelete("{Id:int}")]
        public async Task<ApiResponse> DeleteByKey(int Id)
        {
            var cmd = new DeleteUserCommand(Id);

            var result = await base.Execute<VoidResult>(cmd);

            return result;
        }

    }
}