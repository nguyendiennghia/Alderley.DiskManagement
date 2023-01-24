using System.Collections.Generic;

namespace Alderley.DiskManagement.Service.Models
{
    public class Directory
    {
        public string Name { get; set; }
        public IList<Directory> Directories { get; set; } = new List<Directory>();
        public IList<File> Files { get; set; } = new List<File>();
    }
}
