using AutoMapper;
//using Fi.Template.Api.Domain;
#if (RelationalDatabase)
//using Fi.Template.Api.Domain.Entity;
#elif (NoSqlDatabase)
//using Fi.Template.Api.Domain.Document;
#endif
using Fi.TemplateUniqueName.Schema.Model;

namespace Fi.Template.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            
        }
    }
}