using NexusERP.Domain.Identity.ValueObjects;
using NexusERP.Domain.Exceptions;

namespace NexusERP.UnitTests.Domain.Identity.Aggregates;
public class PersonNameTests
{
    [Fact]
    public void Should_Create_PersonName_When_Value_Is_Valid()
    {
        // Arrange
        var value = "Mario";

        // Act
        var personName = new PersonName(value);

        // Assert
        Assert.Equal("Mario", personName.Value);
    }

    [Fact]
    public void Should_Throw_Exception_When_Value_Is_Empty()
    {
        Assert.Throws<DomainException>(() =>
            new PersonName(""));
    }
}