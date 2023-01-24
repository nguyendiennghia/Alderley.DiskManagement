using Alderley.DiskManagement.Service.Models;
using System.Collections.Generic;

namespace Alderley.DiskManagement.Service
{
    public interface IDiskManagementCalculator
    {
        float SizeOf(Directory directory);

        IList<Directory> EstimateCleanUp(Directory root, float atMost = 100000);
    }
}
