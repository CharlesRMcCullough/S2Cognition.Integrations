using FakeItEasy;
using S2Cognition.Integrations.Zoom.Core.Data;
using S2Cognition.Integrations.Zoom.Core.Models;
using S2Cognition.Integrations.Zoom.Phones.Data;
using Shouldly;

namespace S2Cognition.Integrations.Zoom.Phones.Tests
{
    public class SetCallQueueMembersTests : IntegrationTests
    {
        [Fact]
        public async Task EnsureSetCallQueuesMembersChecksQueueNameIsNotNull()
        {
            await _sut.Initialize(_configuration);

            var ex = await Should.ThrowAsync<ArgumentException>(async () => await _sut.SetCallQueueMembers(new SetCallQueueMemberRequest()));

            ex.ShouldNotBeNull();
            ex.Message.ShouldBe(nameof(SetCallQueueMemberRequest.QueueName));
        }

        [Fact]
        public async Task EnsureSetCallQueuesMembersChecksUserEmailIsNotNull()
        {
            await _sut.Initialize(_configuration);

            var ex = await Should.ThrowAsync<ArgumentException>
                (async () => await _sut.SetCallQueueMembers(new SetCallQueueMemberRequest { QueueName = "fake Queue Name" }));

            ex.ShouldNotBeNull();
            ex.Message.ShouldBe(nameof(SetCallQueueMemberRequest.UserEmail));
        }

        [Fact]
        public async Task EnsureSetCallQueuesMembersExpectedResults()
        {
            var expectedQueues = new ZoomGetCallQueuesResponse
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

            var expectedUsers = new ZoomGetUsersPagedResponse
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

            A.CallTo(() => _client.GetZoomUsers(A<string>._, A<GetUsersRequest>._)).Returns(expectedUsers);

            A.CallTo(() => _client.GetZoomCallQueues(A<string>._, A<GetCallQueuesRequest>._)).Returns(expectedQueues);

            var response = await _sut.SetCallQueueMembers(new SetCallQueueMemberRequest
            {
                QueueName = "fake Call Queue Name",
                UserEmail = "test@test.com"
            });

            response.ShouldNotBeNull();
        }
    }
}
