using System;

namespace SherbetVaults.Models.Restrictions
{
    public class BadSelectorException : Exception
    {
        public BadSelectorException(string message) : base(message)
        {
        }
    }
}