using NexusERP.Domain.Identity.ValueObjects;

namespace NexusERP.UnitTests.Domain.Identity.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Emails_With_Same_Value_Should_Be_Equal()
    {
        var email1 = new Email("mario@test.com");
        var email2 = new Email("mario@test.com");

        Assert.Equal(email1, email2);
    }
}