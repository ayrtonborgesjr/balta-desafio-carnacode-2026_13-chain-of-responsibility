using Reembolsos.Console.Domain.Configuration;

namespace Reembolsos.Tests.Domain.Configuration;

public class ApprovalLimitsSettingsTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var settings = new ApprovalLimitsSettings();

        // Assert
        Assert.NotNull(settings);
        Assert.Equal(0m, settings.Supervisor);
        Assert.Equal(0m, settings.Manager);
        Assert.Equal(0m, settings.Director);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var settings = new ApprovalLimitsSettings
        {
            Supervisor = 100m,
            Manager = 500m,
            Director = 5000m
        };

        // Assert
        Assert.Equal(100m, settings.Supervisor);
        Assert.Equal(500m, settings.Manager);
        Assert.Equal(5000m, settings.Director);
    }

    [Theory]
    [InlineData(50, 250, 2500)]
    [InlineData(100, 500, 5000)]
    [InlineData(200, 1000, 10000)]
    public void Properties_ShouldAcceptDifferentValues(
        decimal supervisor,
        decimal manager,
        decimal director)
    {
        // Arrange
        var settings = new ApprovalLimitsSettings
        {
            Supervisor = supervisor,
            Manager = manager,
            Director = director
        };

        // Assert
        Assert.Equal(supervisor, settings.Supervisor);
        Assert.Equal(manager, settings.Manager);
        Assert.Equal(director, settings.Director);
    }
}

