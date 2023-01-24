using System.Collections;
using Directory = Alderley.DiskManagement.Service.Models.Directory;
using File = Alderley.DiskManagement.Service.Models.File;

namespace Alderley.DiskManagement.Service.Test
{
    public partial class DiskManagementCalculatorTester
    {
        [Trait("Category", "SizeOf")]
        [Theory]
        [ClassData(typeof(EmptyRootData))]
        public void SizeOf_EmptyRoot_ReturnsZero(Directory rootWithoutFileSize)
        {
            var service = new DiskManagementCalculator();
            var size = service.SizeOf(rootWithoutFileSize);
            Assert.Equal(0, size);
        }

        [Trait("Category", "SizeOf")]
        [Theory]
        [ClassData(typeof(SizedRootData))]
        public void SizeOf_Root_ReturnsSumOfSizes(Directory root, float expectedSize) 
        {
            var service = new DiskManagementCalculator();
            Assert.Equal(expectedSize, service.SizeOf(root));
        }
    }

    public class EmptyRootData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Directory() };
            yield return new object[] { new Directory
            {
                Directories = new[]
                {
                    new Directory(),
                    new Directory
                    {
                        Directories = new[] 
                        { 
                            new Directory(),
                            new Directory()
                        }
                    }
                }
            } };
            yield return new object[] { new Directory
            {
                Files = new []
                {
                    new File { Size = 0f },
                    new File()
                }
            } };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class SizedRootData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new Directory { Files = new[] { new File { Size = 12345f } } },
                12345f
            };

            yield return new object[]
            {
                new Directory
                {
                    Directories = new[]
                    {
                        new Directory {  Files = new[] { new File { Size = 1234f } } },
                        new Directory {  Files = new[] { new File { Size = 5678f } } },
                    }
                },
                1234f + 5678f
            };

            yield return new object[]
            {
                new Directory
                {
                    Files = new[]
                    {
                        new File { Size = 12f },
                        new File { Size = 34f }
                    },
                    Directories = new[]
                    {
                        new Directory {  Files = new[] { new File { Size = 56f } } },
                        new Directory
                        {
                            Files = new[] { new File { Size = 78f } },
                            Directories = new []
                            {
                                new Directory { Files = new[] { new File { Size = 910f } } }
                            }
                        },
                    }
                },
                12f + 34f + 56f + 78f + 910f
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
