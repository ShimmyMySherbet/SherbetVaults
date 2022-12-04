using System;

namespace SherbetVaults.Models
{
    public class VaultStoreDeniedException : Exception
    {
        public VaultStoreDeniedException() : base("A player attemped to store an item in their vault that is blacklisted")
        {
        }
    }
}