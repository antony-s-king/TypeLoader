using System;
using System.Reflection;
using FakeItEasy;
using Xunit;
using Xunit.Abstractions;
using Maverick.TypeLoader;
using Maverick.TypeLoader.Tests.TestCase;
using Xunit.Sdk;

namespace Maverick.TypeLoader.Tests
{
    public class LoaderTests
    {
        private T LoadTypeFromInMemoryAssembly<T>(string typeName) where T : class
        {
            // Load from this test assembly in memory
            var assembly = Assembly.Load("Maverick.TypeLoader.Tests");
            var loader = new Loader<T>();

            return loader.LoadImplementingTypeFromAssembly(assembly, typeName);
        }

        private T LoadTypeFromDisk<T>(string typeName) where T : class
        {
            // Load from this assembly located on disk
            var path = Assembly.GetExecutingAssembly().Location;
            var loader = new Loader<T>();

            return loader.LoadImplementingTypeFromFiles(new string[] {path}, typeName);
        }

        [Fact]
        public void LoadImplementingTypeFromAssembly_AssemblyGiven_ReturnsType()
        {
            // Arrange
            var type = LoadTypeFromInMemoryAssembly<IMath>("AdderMath");

            // Assert
            Assert.IsType<AdderMath>(type);
        }
        
        [Fact]
        public void LoadImplementingTypeFromAssembly_AssemblyGivenButNoType_ThrowsException()
        {
            // Arrange
            Action loadAction = () => LoadTypeFromInMemoryAssembly<IMath>("SubtractorMath");

            // Act & Assert
            Assert.ThrowsAny<TypeLoadException>(loadAction);
        }

        [Fact]
        public void LoadImplementingTypeFromFiles_AssemblyGiven_ReturnsType()
        {
            // Arrange
            var type = LoadTypeFromDisk<IMath>("AdderMath");

            // Assert
            Assert.IsType<AdderMath>(type);
        }

        [Fact]
        public void LoadImplementingTypeFromFiles_AssemblyGivenButNoType_ThrowsException()
        {
            // Arrange
            Action loadAction = () => LoadTypeFromDisk<IMath>("SubtractorMath");

            // Act & Assert
            Assert.ThrowsAny<TypeLoadException>(loadAction);
        }
    }
}
