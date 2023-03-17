using FakeItEasy;
using S2Cognition.Integrations.Zoom.Phones.Data;
using Shouldly;

namespace S2Cognition.Integrations.Zoom.Phones.Tests;

public class GetCallQueuesTests : IntegrationTests
{
    [Fact]
    public async Task EnsureCallQueuesReturnsExpectedResults()
    {
        var expectedResponse = new ZoomGetCallQueuesResponse
        {
            CallQueues = new List<CallQueueRecord>
                { new CallQueueRecord
                    {
                        ExtensionNumber = 123,
                        Extension_Id = "asdfasdf443534swdfd",
                        id = "seasdfsd434325saKJII",
                        Name = "fake Call Queue Name",
                        Status = "Activate",
                        PhoneNumber = new List<PhoneNumberRecord>
                             { new PhoneNumberRecord {
                                Id = "fake id",
                                Number = "fake number",
                                Source = "fake source",
                             }
                        },
                        Site = new SiteRecord
                        {
                            Id = "fake site id",
                            Name = "fake site name"
                        }
                }
            }
        };

        A.CallTo(() => _sut.Authenticate())
                .Returns("");
        A.CallTo(() => _client.GetZoomCallQueues(A<string>._, A<GetCallQueuesRequest>._)).Returns(expectedResponse);

        var response = await _sut.GetCallQueues(new GetCallQueuesRequest());

        response.ShouldNotBeNull();
        response.CallQueues.ShouldNotBeNull();
        response.CallQueues.Count.ShouldBe(1);
        response.CallQueues.Any(_ => _.id == expectedResponse.CallQueues[0].id);
        response.CallQueues.Any(_ => _.Name == expectedResponse.CallQueues[0].Name);
        response.CallQueues.Any(_ => _.Name == expectedResponse.CallQueues[0].Status);
        response.CallQueues.Any(_ => _.PhoneNumber?[0].Id == expectedResponse.CallQueues[0]?.PhoneNumber?[0].Id);
        response.CallQueues.Any(_ => _.PhoneNumber?[0].Number == expectedResponse.CallQueues[0]?.PhoneNumber?[0].Number);
        response.CallQueues.Any(_ => _.Site?.Id == expectedResponse.CallQueues[0]?.Site?.Id);
        response.CallQueues.Any(_ => _.Site?.Name == expectedResponse.CallQueues[0]?.Site?.Name);
    }

}
