using Fi.Infra.Exceptions;

namespace Fi.Patika.Api.Impl
{
    public class ErrorCodes : BaseErrorCodes
    {
        public static FiBusinessReason SampleAlreadyExists => new FiBusinessReason(1,
            "A sample with same Code already exists. {0} ({1}).");

        public static FiBusinessReason NotEnoughBalance => new FiBusinessReason(1,
            "Account balance is insufficient. {0} ({1)}.");
    }
}
