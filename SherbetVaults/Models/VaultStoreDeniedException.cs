using System;

namespace SherbetVaults.Models
{
    public class VaultStoreDeniedException : Exception
    {
        public VaultStoreDeniedException() : base("A player attempted to store an item in their vault that is blacklisted")
        {
        }
    }
}