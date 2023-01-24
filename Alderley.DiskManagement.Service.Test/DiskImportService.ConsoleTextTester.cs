namespace Alderley.DiskManagement.Service.Test
{
    public class DiskImportByConsoleTextServiceTester
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ImportAsync_InvalidInput_ReturnsNull(string input)
        {
            var service = new DiskImportByConsoleTextService();

            var directory = await service.ImportAsync(input);

            Assert.Null(directory);
        }

        [Fact]
        public async Task ImportAsync_EmptyRoot_ReturnsRootWithoutChild()
        {
            var service = new DiskImportByConsoleTextService();

            var root = await service.ImportAsync("- / (dir)");

            Assert.NotNull(root);
            Assert.Equal("/", root.Name);
            Assert.Empty(root.Directories);
            Assert.Empty(root.Files);
        }

        [Fact]
        public async Task ImportAsync_RootWith1File_ReturnsRootWith1File()
        {
            var service = new DiskImportByConsoleTextService();

            var root = await service.ImportAsync("- / (dir)\r\n" + "\t- abc.ext (file, size=12345)");

            Assert.NotNull(root);
            Assert.Equal("/", root.Name);
            Assert.Empty(root.Directories);
            Assert.Equal(1, root.Files.Count);
            var file = root.Files[0];
            Assert.NotNull(file);
            Assert.Equal("abc.ext", file.Name);
            Assert.Equal(12345f, file.Size);
        }

        [Fact]
        public async Task ImportAsync_RootWith1Directory_ReturnsRootWith1Directory()
        {
            var service = new DiskImportByConsoleTextService();

            var root = await service.ImportAsync("- / (dir)\r\n" + "\t- abc (dir)");

            Assert.NotNull(root);
            Assert.Equal("/", root.Name);
            Assert.Empty(root.Files);
            Assert.Equal(1, root.Directories.Count);
            var dir = root.Directories[0];
            Assert.NotNull(dir);
            Assert.Equal("abc", dir.Name);
        }

        [Fact]
        public async Task ImportAsync_RootWith1Directory_ReturnsRootWith1DirectoryWith1File()
        {
            var service = new DiskImportByConsoleTextService();

            var root = await service.ImportAsync(
                "- / (dir)\r\n" + 
                    "\t- a (dir)\r\n" +
                        "\t\t- e (dir)\r\n" +
                            "\t\t\t- i (file, size=584)\r\n" +
                        "\t\t- f (file, size=29116)\r\n" +
                        "\t\t- g (file, size=2557)\r\n" +
                        "\t\t- h.lst (file, size=62596)\r\n" +
                    "\t- b.txt (file, size=14848514)\r\n" +
                    "\t- c.dat (file, size=8504156)\r\n" +
                    "\t- d (dir)\r\n" +
                        "\t\t- j (file, size=4060174)\r\n" +
                        "\t\t- d.log (file, size=8033020)\r\n" +
                        "\t\t- d.ext (file, size=5626152)\r\n" +
                        "\t\t- k (file, size=7214296)"
            );

            Assert.NotNull(root);
            Assert.Equal("/", root.Name);

            Assert.NotNull(root.Files);
            Assert.Equal(2, root.Files.Count);
            Assert.Equal("b.txt", root.Files[0].Name);
            Assert.Equal(14848514f, root.Files[0].Size);
            Assert.Equal("c.dat", root.Files[1].Name);
            Assert.Equal(8504156f, root.Files[1].Size);

            Assert.Equal(2, root.Directories.Count);

            var dir1 = root.Directories[0];
            Assert.NotNull(dir1);
            Assert.Equal("a", dir1.Name);
            
            Assert.Equal(3, dir1.Files.Count);
            Assert.Equal("f", dir1.Files[0].Name);
            Assert.Equal(29116f, dir1.Files[0].Size);
            Assert.Equal("g", dir1.Files[1].Name);
            Assert.Equal(2557f, dir1.Files[1].Size);
            Assert.Equal("h.lst", dir1.Files[2].Name);
            Assert.Equal(62596f, dir1.Files[2].Size);

            Assert.Equal(1, dir1.Directories.Count);
            Assert.Equal("e", dir1.Directories[0].Name);
            Assert.Equal(1, dir1.Directories[0].Files.Count);
            Assert.Equal("i", dir1.Directories[0].Files[0].Name);
            Assert.Equal(584f, dir1.Directories[0].Files[0].Size);

            var dir2 = root.Directories[1];
            Assert.NotNull(dir2);
            Assert.Equal("d", dir2.Name);
            Assert.Equal(4, dir2.Files.Count);
            Assert.Equal("j", dir2.Files[0].Name);
            Assert.Equal(4060174f, dir2.Files[0].Size);
            Assert.Equal("d.log", dir2.Files[1].Name);
            Assert.Equal(8033020f, dir2.Files[1].Size);
            Assert.Equal("d.ext", dir2.Files[2].Name);
            Assert.Equal(5626152f, dir2.Files[2].Size);
            Assert.Equal("k", dir2.Files[3].Name);
            Assert.Equal(7214296f, dir2.Files[3].Size);
        }
    }
}