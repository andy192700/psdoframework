using AutoFixture.Xunit2;
using FluentAssertions;
using PSDoFramework.Tool;
using System.Diagnostics;
using System.Text;

namespace PSDoFramework.ToolTests
{
    public class ToolingArgMapperTests
    {
        [Fact]
        public void ToolingArgMapper_NoParams()
        {
            // Arrange
            var mapper = new ToolingArgMapper();

            // Act
            var result = mapper.Map([]);

            // Assert
            result.Should().NotBeNull();

            result.StartInfo.FileName.Should().Be("pwsh");
            result.StartInfo.Arguments.Should().Be("-command doing");
        }

        [Theory]
        [InlineAutoData]
        public void ToolingArgMapper_SingleParam(string param)
        {
            // Arrange
            var mapper = new ToolingArgMapper();

            // Act
            var result = mapper.Map([ param ]);

            // Assert
            result.Should().NotBeNull();

            result.StartInfo.FileName.Should().Be("pwsh");
            result.StartInfo.Arguments.Should().Be($"-command doing {param}");
        }

        [Theory]
        [InlineAutoData]
        public void ToolingArgMapper_MultiParam(string[] parameters)
        {
            // Arrange
            var mapper = new ToolingArgMapper();

            // Act
            var result = mapper.Map(parameters);

            // Assert
            result.Should().NotBeNull();

            result.StartInfo.FileName.Should().Be("pwsh");
            result.StartInfo.Arguments.Should().Be($"-command doing{PredictCommand(parameters)}");
        }

        private string PredictCommand(string[] args)
        {
            var cmd = new StringBuilder();

            for (int i = 0; i < args.Length; i++)
            {
                cmd.Append($" {args[i]}");
            }

            return cmd.ToString();
        }
    }
}