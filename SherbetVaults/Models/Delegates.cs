using System.Threading.Tasks;
using ShimmyMySherbet.MySQL.EF.Core;
using ShimmyMySherbet.MySQL.EF.Models.Interfaces;

namespace SherbetVaults.Models
{
    public delegate Task AsyncDatabaseAction<T>(T Table);
}