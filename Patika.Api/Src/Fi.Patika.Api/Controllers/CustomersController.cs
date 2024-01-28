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
    public class CustomersController : ApiControllerBase<CustomersController>
    {
        public CustomersController(ApiControllerDI<CustomersController> baseDI) : base(baseDI)
        {
        }

        [ApiKey("ba06b221-7bca-4c7c-a033-8198407517ff")]
        [ApiAuthorizationAttribute(ScopeKeys.View_Customer)]
        [HttpGet("{Id:int}")]
        public async Task<ApiResponse<CustomerOutputModel>> GetByKey(int Id)
        {
            var cmd = new GetCustomerByKeyQuery(Id);

            var result = await base.Execute<CustomerOutputModel>(cmd);

            return result;
        }

        [ApiKey("bd58fb92-54be-4498-8eab-048f4f73834b")]
        [ApiAuthorizationAttribute(ScopeKeys.List_Customer)]
        [HttpGet("ByParameters")]
        public async Task<ApiResponse<List<CustomerOutputModel>>> GetByParameters()
        {
            var cmd = new GetCustomerByParametersQuery();

            var result = await base.Execute<List<CustomerOutputModel>>(cmd);

            return result;
        }

        [ApiKey("77fa76ab-7323-4a49-9800-eae4351797dd")]
        [ApiAuthorizationAttribute(ScopeKeys.Create_Customer)]
        [HttpPost]
        public async Task<ApiResponse<CustomerOutputModel>> Create([FromBody]CustomerInputModel model)
        {
            var cmd = new CreateCustomerCommand(model);

            var result = await base.Execute<CustomerOutputModel>(cmd);

            return result;
        }

        [ApiKey("418b4dcf-903c-4191-8b20-eb7eff3f428e")]
        [ApiAuthorizationAttribute(ScopeKeys.Update_Customer)]
        [HttpPut("{Id:int}")]
        public async Task<ApiResponse<CustomerOutputModel>> Update(int Id, [FromBody]CustomerInputModel model)
        {
            var cmd = new UpdateCustomerCommand(Id, model);

            var result = await base.Execute<CustomerOutputModel>(cmd);

            return result;
        }

        //If you really need delete api, take the [OnlyForDevelopment] tag as comment. Or you can delete the API completely.
        [OnlyForDevelopment]
        [ApiKey("79d1e541-81c9-466a-8dfe-0ecbcfd74952")]
        [ApiAuthorizationAttribute(ScopeKeys.Delete_Customer)]
        [HttpDelete("{Id:int}")]
        public async Task<ApiResponse> DeleteByKey(int Id)
        {
            var cmd = new DeleteCustomerCommand(Id);

            var result = await base.Execute<VoidResult>(cmd);

            return result;
        }

    }
}