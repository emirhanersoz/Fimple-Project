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
    public class SupportRequestsController : ApiControllerBase<SupportRequestsController>
    {
        public SupportRequestsController(ApiControllerDI<SupportRequestsController> baseDI) : base(baseDI)
        {
        }

        [ApiKey("d7b5dd30-2125-48a9-b847-b9f70decade8")]
        [ApiAuthorizationAttribute(ScopeKeys.View_SupportRequest)]
        [HttpGet("{Id:int}")]
        public async Task<ApiResponse<SupportRequestOutputModel>> GetByKey(int Id)
        {
            var cmd = new GetSupportRequestByKeyQuery(Id);

            var result = await base.Execute<SupportRequestOutputModel>(cmd);

            return result;
        }

        [ApiKey("e4b97443-9afa-42d1-9ecd-88737023bda0")]
        [ApiAuthorizationAttribute(ScopeKeys.List_SupportRequest)]
        [HttpGet("ByParameters")]
        public async Task<ApiResponse<List<SupportRequestOutputModel>>> GetByParameters()
        {
            var cmd = new GetSupportRequestByParametersQuery();

            var result = await base.Execute<List<SupportRequestOutputModel>>(cmd);

            return result;
        }

        [ApiKey("ff9d567f-c395-4b22-aea3-f64a19bab2a8")]
        [ApiAuthorizationAttribute(ScopeKeys.Create_SupportRequest)]
        [HttpPost]
        public async Task<ApiResponse<SupportRequestOutputModel>> Create([FromBody]SupportRequestInputModel model)
        {
            var cmd = new CreateSupportRequestCommand(model);

            var result = await base.Execute<SupportRequestOutputModel>(cmd);

            return result;
        }

        [ApiKey("6b875f52-28d0-4a95-b507-5ffbfea5501e")]
        [ApiAuthorizationAttribute(ScopeKeys.Update_SupportRequest)]
        [HttpPut("{Id:int}")]
        public async Task<ApiResponse<SupportRequestOutputModel>> Update(int Id, [FromBody]SupportRequestInputModel model)
        {
            var cmd = new UpdateSupportRequestCommand(Id, model);

            var result = await base.Execute<SupportRequestOutputModel>(cmd);

            return result;
        }

        //If you really need delete api, take the [OnlyForDevelopment] tag as comment. Or you can delete the API completely.
        [OnlyForDevelopment]
        [ApiKey("5dc85b3f-ddb1-4571-b98b-1cbcfc363c74")]
        [ApiAuthorizationAttribute(ScopeKeys.Delete_SupportRequest)]
        [HttpDelete("{Id:int}")]
        public async Task<ApiResponse> DeleteByKey(int Id)
        {
            var cmd = new DeleteSupportRequestCommand(Id);

            var result = await base.Execute<VoidResult>(cmd);

            return result;
        }

    }
}