using AutoMapper;
using Fi.Patika.Api.Domain.Entity;

//using Fi.Patika.Api.Domain;
//using Fi.Patika.Api.Domain.Entity;
using Fi.Patika.Schema.Model;

namespace Fi.Patika.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Login, LoginOutputModel>();
            CreateMap<LoginInputModel, Login>();

            CreateMap<User, UserOutputModel>();
            CreateMap<UserInputModel, User>();

            CreateMap<AccountCredit, AccountCreditOutputModel>();
            CreateMap<AccountCreditInputModel, AccountCredit>();

            CreateMap<Credit, CreditOutputModel>();
            CreateMap<CreditInputModel, Credit>();

            CreateMap<SupportRequest, SupportRequestOutputModel>();
            CreateMap<SupportRequestInputModel, SupportRequest>();

            CreateMap<DepositAndWithdraw, DepositAndWithdrawOutputModel>();
            CreateMap<DepositAndWithdrawInputModel, DepositAndWithdraw>();

            CreateMap<Payee, PayeeOutputModel>();
            CreateMap<PayeeInputModel, Payee>();

            CreateMap<MoneyTransfer, MoneyTransferOutputModel>();
            CreateMap<MoneyTransferInputModel, MoneyTransfer>();

            CreateMap<Account, AccountOutputModel>();
            CreateMap<AccountInputModel, Account>();
  
            CreateMap<Customer, CustomerOutputModel>();
            CreateMap<CustomerInputModel, Customer>();
        }
    }
}