using System;

namespace Iaphub
{
    public class IaphubException : Exception
    {
        public IaphubError Error { get; }

        public IaphubException(IaphubError error) : base(error.Message)
        {
            Error = error;
        }

        public string Code => Error.Code;
        public string? Subcode => Error.Subcode;
    }
}
