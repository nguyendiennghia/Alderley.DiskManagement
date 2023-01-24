using Alderley.DiskManagement.Service.Models;
using System.Collections.Generic;
using System.Linq;

namespace Alderley.DiskManagement.Service
{
    public class DiskManagementCalculator : IDiskManagementCalculator
    {
        private void Collect(IList<Directory> directories, Directory current, float atMost, bool isRoot)
        {
            if (!isRoot && SizeOf(current) <= atMost)
                directories.Add(current);

            foreach (var dir in current.Directories)
                Collect(directories, dir, atMost, false);
        }

        IList<Directory> IDiskManagementCalculator.EstimateCleanUp(Directory root, float atMost)
        {
            var list = new List<Directory>();
            Collect(list, root, atMost, true);
            return list;
        }

        public float SizeOf(Directory directory)
        {
            return directory.Files.Sum(d => d.Size) + directory.Directories.Sum(SizeOf);
        }
    }
}
