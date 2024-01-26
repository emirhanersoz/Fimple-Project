using Fi.Infra.Exceptions;

namespace Fi.Patika.Api.Impl
{
    public class ErrorCodes : BaseErrorCodes
    {
        public static FiBusinessReason SampleAlreadyExists => new FiBusinessReason(1,
            "A sample with same Code already exists. {0} ({1}).");
    }
}
