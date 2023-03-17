using FakeItEasy;
using S2Cognition.Integrations.Zoom.Phones.Data;
using Shouldly;

namespace S2Cognition.Integrations.Zoom.Phones.Tests;

public class GetCallQueueMembersTests : IntegrationTests
{
    [Fact]
    public async Task EnsureGetCallQueuesMembersChecksCallQueueNameIsNotNull()
    {
        await _sut.Initialize(_configuration);

        var ex = await Should.ThrowAsync<ArgumentException>(async () => await _sut.GetCallQueueMembers(new GetCallQueueMemberRequest()));

        ex.ShouldNotBeNull();
        ex.Message.ShouldBe(nameof(GetCallQueueMemberRequest.CallQueueName));
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

        var expectedUsers = new ZoomGetCallQueueMemberResponse
        {
            CallQueueMembers = new List<CallQueueMember>
             {
                new CallQueueMember {
                       Id = "A123456",
                       Name = "Test Queue Member"
                 }
             }
        };

        A.CallTo(() => _sut.Authenticate())
                .Returns("");
        A.CallTo(() => _client.GetZoomCallQueues(A<string>._, A<GetCallQueuesRequest>._)).Returns(expectedQueues);

        A.CallTo(() => _client.GetZoomCallQueueMembers(A<string>._, A<ZoomGetCallQueueMemberRequest>._)).Returns(expectedUsers);

        var response = await _sut.GetCallQueueMembers(new GetCallQueueMemberRequest
        {
            CallQueueName = "Fake Call Queue Name"
        });

        response.ShouldNotBeNull();
        response.Id.ShouldBeEquivalentTo("A123456");
        response.Name.ShouldBeEquivalentTo("Test Queue Member");

    }
}
