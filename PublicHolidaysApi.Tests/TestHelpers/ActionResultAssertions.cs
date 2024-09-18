using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;

namespace PublicHolidaysApi.Tests.TestHelpers;

public static class ActionResultAssertions
{
    public static void ShouldReturnInternalServerError<T>(this ActionResult<T> result, string expectedMessage)
    {
        var objectResult = result.Result as ObjectResult;

        using (new AssertionScope())
        {
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(500);
            objectResult.Value.Should().Be(expectedMessage);
        }
    }
    
    public static void ShouldBeOkObjectResultWithValue<T>(this ActionResult<T> result, T expectedValue)
    {
        var okResult = result.Result as OkObjectResult;

        using (new AssertionScope())
        {
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedValue);
        }
    }
}