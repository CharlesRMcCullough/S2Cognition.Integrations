using FakeItEasy;
using MailChimp.Net.Models;
using Shouldly;

namespace S2Cognition.Integrations.MailChimp.Core.Tests;


public class GetListsTest : IntegrationTests
{
    [Fact]
    public async Task EnsureGetListsReturnsExpectedResults()
    {
        var expectedResponse = new List<List>
        {
             new List
             {
                 Id = "Fake List Id 1",
                 Name = "Fake List Name 1"
             },
             new List
             {
                 Id = "Fake List Id 2",
                 Name = "Fake List Name 2"
             }
        };

        A.CallTo(() => _client.GetLists()).Returns(expectedResponse);

        var response = await _sut.GetLists();

        response.GetListResponseItems.ShouldNotBeNull();
        response.GetListResponseItems.Count.ShouldBe(2);
        response.GetListResponseItems.Any(_ => _.ListId == expectedResponse[0].Id);
        response.GetListResponseItems.Any(_ => _.ListName == expectedResponse[0].Name);
        response.GetListResponseItems.Any(_ => _.ListId == expectedResponse[1].Id);
        response.GetListResponseItems.Any(_ => _.ListName == expectedResponse[1].Name);
    }
}
