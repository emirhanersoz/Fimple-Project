using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fi.ApiBase.Controller;
using Fi.ApiBase.Attribute;
using Fi.Mediator.Message;
using Fi.Template.Api.Cqrs;
using Fi.Template.Api.Domain;
using Fi.Template.Api.Impl;
using Fi.TemplateUniqueName.Schema.Model;

namespace Fi.Template.Api.Controllers
{
    [ApiController]
    [Route("api/v1/TemplateUniqueName/[controller]")]
    public class SamplesController : ApiControllerBase<SamplesController>
    {
        public SamplesController(ApiControllerDI<SamplesController> baseDI) : base(baseDI)
        {
        }

        [ApiKey("20a0b529-d4e4-4557-bae3-726f0a696c2d")]
        [ApiAuthorizationAttribute(ScopeKeys.View_TemplateUniqueName)]
        [HttpGet("{Id:int}")]
        public async Task<ApiResponse<SampleOutputModel>> GetByKey(int Id)
        {
            var cmd = new GetSampleByKeyQuery(Id);

            var result = await base.Execute<SampleOutputModel>(cmd);

            return result;
        }

        [ApiKey("f81b0abe-57b1-4166-b1e6-5b6032d1ca28")]
        [ApiAuthorizationAttribute(ScopeKeys.List_TemplateUniqueName)]
        [HttpGet]
        public async Task<ApiResponse<List<SampleOutputModel>>> GetAllList()
        {
            var cmd = new GetAllSampleQuery();

            var result = await base.Execute<List<SampleOutputModel>>(cmd);

            return result;
        }

        [ApiKey("cdb2c879-e465-4e15-982e-650ab50bedc4")]
        [ApiAuthorizationAttribute(ScopeKeys.List_TemplateUniqueName)]
        [HttpGet("BySampleCode/{SampleCode:length(1,3)}")]
        public async Task<ApiResponse<List<SampleOutputModel>>> GetBySampleCode(string SampleCode)
        {
            var cmd = new GetSampleByCodeQuery(SampleCode);

            var result = await base.Execute<List<SampleOutputModel>>(cmd);

            return result;
        }

        [ApiKey("336d7131-f153-4caa-895e-9a68c900de97")]
        [ApiAuthorizationAttribute(ScopeKeys.Create_TemplateUniqueName)]
        [HttpPost]
        [OnlyForDevelopment]
        public async Task<ApiResponse<SampleOutputModel>> Create([FromBody] SampleInputModel model)
        {
            var cmd = new CreateSampleCommand(model);

            var result = await base.Execute<SampleOutputModel>(cmd);

            return result;
        }
        [ApiKey("dc40e3c8-d69c-4eed-bc08-c6b457e3cef7")]
        [ApiAuthorizationAttribute(ScopeKeys.Update_TemplateUniqueName)]
        [HttpPut("{Id:int}")]
        [OnlyForDevelopment]
        public async Task<ApiResponse<SampleOutputModel>> Update(int Id, [FromBody] SampleInputModel model)
        {
            var cmd = new UpdateSampleCommand(Id, model);

            var result = await base.Execute<SampleOutputModel>(cmd);

            return result;
        }

        [ApiKey("fbb08e30-6ea2-4beb-98e7-065a785ea68a")]
        [ApiAuthorizationAttribute(ScopeKeys.Delete_TemplateUniqueName)]
        [HttpDelete("{Id:int}")]
        [OnlyForDevelopment]
        public async Task<ApiResponse> DeleteByKey(int Id)
        {
            var cmd = new DeleteSampleCommand(Id);

            await base.Execute<VoidResult>(cmd);

            return new ApiResponse();
        }
    }
}