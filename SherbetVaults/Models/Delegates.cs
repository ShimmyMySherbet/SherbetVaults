using System.Threading.Tasks;

namespace SherbetVaults.Models
{
    public delegate Task AsyncDatabaseAction<T>(T Table);
}