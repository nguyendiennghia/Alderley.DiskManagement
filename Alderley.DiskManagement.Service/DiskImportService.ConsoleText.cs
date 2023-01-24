using Alderley.DiskManagement.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alderley.DiskManagement.Service
{
    public class DiskImportByConsoleTextService : IDiskImportService<string>
    {
        /// <example>- abc.test (file, size=123) ; or - myfolder (dir)</example>
        private static readonly Regex rowPattern = new Regex(@"-\s+([\/a-zA-Z0-9.-]+)\s+\((dir|file)(,\s+size=([\d.]+))?\)", RegexOptions.Compiled);

        public async Task<Directory> ImportAsync(string inputs)
        {
            if (string.IsNullOrWhiteSpace(inputs))
                return null;

            var lines = InitiateBy(inputs);
            Traverse(lines);
            return lines.FirstOrDefault()?.Obj as Directory 
                ?? throw new InvalidOperationException("Invalid disk structure");
        }

        private static IList<LevelObj> InitiateBy(string inputs)
        {
            return inputs.Split("\r\n").Select(line =>
            {
                var match = rowPattern.Match(line);
                var type = match.Groups[2].Value;
                return new LevelObj
                {
                    Level = line.Split("-")[0].Count(c => c == '\t'),
                    Obj = type == "dir"
                        ? DirectoryFrom(match.Groups)
                        : FileFrom(match.Groups) as object
                };
            }).ToList();
        }

        private static void Traverse(IList<LevelObj> levelledObjects)
        {
            for (var i = 0; i < levelledObjects.Count; i++)
            {
                var parentLine = levelledObjects[i];
                if (!(parentLine.Obj is Directory parent)) continue;

                for (var j = i + 1; j < levelledObjects.Count; j++)
                {
                    var childLine = levelledObjects[j];
                    if (parentLine.Level == childLine.Level) break;

                    var isChild = childLine.Level == parentLine.Level + 1;
                    if (isChild)
                    {
                        if (childLine.Obj is Directory dir)
                            parent.Directories.Add(dir);
                        else
                            parent.Files.Add(levelledObjects[j].Obj as File);
                    }
                }
            }
        }

        private static Directory DirectoryFrom(GroupCollection groups) => new Directory
        {
            Name = groups[1].Value
        };

        private static File FileFrom(GroupCollection groups) => new File
        {
            Name = groups[1].Value,
            Size = groups.Count > 3 && float.TryParse(groups[4].Value, out var size)
                ? size
                : 0
        };

        private class LevelObj
        {
            public int Level { get; set; }
            public object Obj { get; set; }
        }
    }
}
