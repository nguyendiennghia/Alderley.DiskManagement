using Alderley.DiskManagement.Service.Models;
using System.Threading.Tasks;

namespace Alderley.DiskManagement.Service
{
    public interface IDiskImportService<TIn>
    {
        Task<Directory> ImportAsync(TIn input);
    }
}
