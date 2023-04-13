using FakeItEasy;
using S2Cognition.Integrations.Zoom.Phones.Data;
using Shouldly;

namespace S2Cognition.Integrations.Zoom.Phones.Tests;

public class ClearCallQueueMemberTests : IntegrationTests
{
    [Fact]
    public async Task EnsureClearCallQueuesMembersChecksQueueNameIsNotNull()
    {
        await _sut.Initialize(_configuration);

        var ex = await Should.ThrowAsync<ArgumentException>(async () => await _sut.ClearCallQueueMembers(new ClearCallQueueMemberRequest()));

        ex.ShouldNotBeNull();
        ex.Message.ShouldBe(nameof(ClearCallQueueMemberRequest.QueueName));
    }

    [Fact]
    public async Task EnsureGetCallQueuesMembersReturnsExpectedResults()
    {
        var expectedQueues = new ZoomGetCallQueuesResponse
        {
            CallQueues = new List<CallQueueRecord>
                { new CallQueueRecord
                    {
                        ExtensionNumber = 123,
                        Extension_Id = "asdfasdf443534swdfd",
                        id = "seasdfsd434325saKJII",
                        Name = "Fake Call Queue Name",
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
        A.CallTo(() => _client.GetZoomCallQueues(A<string>._, A<GetCallQueuesRequest>._)).Returns(expectedQueues);

        var response = await _sut.ClearCallQueueMembers(new ClearCallQueueMemberRequest
        {
            QueueName = "Fake Call Queue Name"
        });

        response.ShouldNotBeNull();
    }
}
