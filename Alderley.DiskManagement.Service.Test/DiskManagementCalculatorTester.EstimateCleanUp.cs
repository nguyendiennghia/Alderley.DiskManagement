using System.Collections;
using Directory = Alderley.DiskManagement.Service.Models.Directory;
using File = Alderley.DiskManagement.Service.Models.File;

namespace Alderley.DiskManagement.Service.Test
{
    partial class DiskManagementCalculatorTester
    {
        [Trait("Category", "EstimateCleanUp")]
        [Fact]
        public void EstimateCleanUp_EmptyRoot_ReturnsEmptySinceRootHasBeenExcluded()
        {
            IDiskManagementCalculator service = new DiskManagementCalculator();
            var list = service.EstimateCleanUp(new Directory { Name = "/" });
            Assert.NotNull(list);
            Assert.Equal(0, list.Count);
        }

        [Trait("Category", "EstimateCleanUp")]
        [Fact]
        public void EstimateCleanUp_SimpleRoot_ReturnsEmpty()
        {
            IDiskManagementCalculator service = new DiskManagementCalculator();
            var root = new Directory { Name = "/", Files = new[] { new File { Size = 12345f } } };

            var list = service.EstimateCleanUp(root);

            Assert.Equal(0, list.Count);
        }

        [Trait("Category", "EstimateCleanUp")]
        [Fact]
        public void EstimateCleanUp_HierarchicalRoot_ReturnsLists()
        {
            IDiskManagementCalculator service = new DiskManagementCalculator();
            var root = new Directory 
            { 
                Name = "/",
                Directories = new[]
                {
                    new Directory
                    {
                        Name = "a",
                        Directories = new[]
                        {
                            new Directory 
                            {
                                Name = "e",
                                Files = new[] 
                                {
                                    new File { Name = "i", Size = 584f }
                                }
                            }
                        },
                        Files = new[]
                        {
                            new File { Name= "f", Size = 29116f },
                            new File { Name= "g", Size = 2557f },
                            new File { Name= "h.lst", Size = 62596f },
                        }
                    },
                    new Directory
                    {
                        Name = "d",
                        Files = new[]
                        {
                            new File { Name= "j", Size = 4060174f },
                            new File { Name= "d.log", Size = 8033020f },
                            new File { Name= "d.ext", Size = 5626152f },
                            new File { Name= "j", Size = 7214296f }

                        }
                    },
                },
                Files = new[] 
                { 
                    new File { Name = "b.txt", Size = 14848514f },
                    new File { Name = "c.dat", Size = 8504156f },
                }
            };

            var list = service.EstimateCleanUp(root);

            Assert.Equal(2, list.Count);
            Assert.Equal("a", list[0].Name);
            Assert.Equal("e", list[1].Name);
        }
    }

    public class HierarchicalRootData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var root1 = new Directory { Name = "/", Files = new[] { new File { Size = 12345f } } };
            yield return new object[]
            {
                root1,
                new[] { root1 }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
