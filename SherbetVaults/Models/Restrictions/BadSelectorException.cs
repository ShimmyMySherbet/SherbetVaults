using System;

namespace SherbetVaults.Models.Restrictions
{
    public sealed class BadSelectorException : Exception
    {
        public BadSelectorException(string message) : base(message)
        {
        }
    }
}