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
    public class LoadTypeFromAssembly
    {
        [Fact]
        public void NoType_ThrowsException()
        {
            var assembly = Assembly.Load("Maverick.TypeLoader.Tests");
            var loader = new Loader<IMath>();
            Action loadAction = () => loader.LoadTypeFromAssembly(assembly, "SubtractorMath");

            // Act & Assert
            Assert.ThrowsAny<TypeLoadException>(loadAction);
        }

        [Fact]
        public void ValidType_ReturnsType()
        {
            var assembly = Assembly.Load("Maverick.TypeLoader.Tests");
            var loader = new Loader<IMath>();
            var type = loader.LoadTypeFromAssembly(assembly, "AdderMath");

            // Assert
            Assert.IsType<AdderMath>(type);
        }
    }
    
    public class LoadTypeFromFiles
    {
        [Fact]
        public void NoType_ThrowsException()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var loader = new Loader<IMath>();
            Action loadAction = () => loader.LoadTypeFromFiles(new[] { path }, "SubtractorMath");

            // Act & Assert
            Assert.ThrowsAny<TypeLoadException>(loadAction);
        }

        [Fact]
        public void ValidType_ReturnsType()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var loader = new Loader<IMath>();
            var type = loader.LoadTypeFromFiles(new[] { path }, "AdderMath");

            // Assert
            Assert.IsType<AdderMath>(type);
        }
    }
}
