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
        private T LoadType<T>(string typeName) where T : class
        {
            var assembly = Assembly.Load("Maverick.TypeLoader.Tests");
            var loader = new Loader<T>();

            return loader.LoadImplementingTypeFromAssembly(assembly, typeName);
        }

        [Fact]
        public void LoadImplementingTypeFromAssembly_AssemblyGiven_ReturnsType()
        {
            // Arrange
            var type = LoadType<IMath>("AdderMath");

            // Assert
            Assert.IsType<AdderMath>(type);
        }

        [Fact]
        public void LoadImplementingTypeFromAssembly_AssemblyGiven_TypeWorks()
        {
            // Arrange
            var type = LoadType<IMath>("AdderMath");

            // Act
            var result = type.Add(1, 2);

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public void LoadImplementingTypeFromAssembly_AssemblyGivenButNoType_ThrowsException()
        {
            // Arrange
            Action loadAction = () => LoadType<IMath>("SubtractorMath");

            // Act & Assert
            Assert.ThrowsAny<TypeLoadException>(loadAction);
        }
    }
}
