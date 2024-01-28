using Fi.Infra.Exceptions;
using System.Collections.Generic;

namespace Fi.Patika.Api.Impl
{
    public class ErrorCodes : BaseErrorCodes
    {
        public static FiBusinessReason SampleAlreadyExists => new FiBusinessReason(1,
            "A sample with same Code already exists. {0} ({1}).");

        public static FiBusinessReason NotEnoughBalance => new FiBusinessReason(1,
            "Account balance is insufficient. {0} ({1)}.");

        public static FiBusinessReason ExceedTransferLimit => new FiBusinessReason(1,
            "Daily transfer limits exceed. {0} ({1)}.");

        public static FiBusinessReason ExceedSinlgleTransferLimit => new FiBusinessReason(1,
            "Single transfer limits exceed. {0} ({1)}.");
    }
}
