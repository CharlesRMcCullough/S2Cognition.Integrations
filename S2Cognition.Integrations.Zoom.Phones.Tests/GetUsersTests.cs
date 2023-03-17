using FakeItEasy;
using S2Cognition.Integrations.Zoom.Core.Data;
using S2Cognition.Integrations.Zoom.Core.Models;
using Shouldly;

namespace S2Cognition.Integrations.Zoom.Phones.Tests;

public class GetUsersTests : IntegrationTests
{
    [Fact]
    public async Task EnsureGetUsersReturnsExpectedResults()
    {
        var expectedResponse = new ZoomGetUsersPagedResponse
        {
            TotalRecords = 1,
            Users = new List<ZoomUserRecord>
            {
                new ZoomUserRecord
                {
                    Id = "asdfsdf9342535",
                     FirstName = "TestFirstName",
                     LastName = "TestLastName",
                     Email = "test@test.com"
                }
            }
        };

        A.CallTo(() => _sut.Authenticate())
            .Returns("");
        A.CallTo(() => _client.GetZoomUsers(A<string>._, A<GetUsersRequest>._)).Returns(expectedResponse);

        var response = await _sut.GetUsers(new GetUsersRequest());

        response.ShouldNotBeNull();
        response.TotalRecords.ShouldBeEquivalentTo(expectedResponse.TotalRecords);
        response.Users.ShouldNotBeNull();
        response.Users.Count.ShouldBe(1);
        response.Users.Any(_ => _.Id == expectedResponse.Users[0].Id);
        response.Users.Any(_ => _.FirstName == expectedResponse.Users[0].FirstName);
        response.Users.Any(_ => _.LastName == expectedResponse.Users[0].LastName);
        response.Users.Any(_ => _.Email == expectedResponse.Users[0].Email);
    }
}
