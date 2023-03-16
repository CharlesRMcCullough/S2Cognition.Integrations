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

        //[Fact]
        //public async Task EnsureSetCallQueuesMembersExpectedResults()
        //{
        //    var expectedResponse = new ZoomGetUsersPagedResponse
        //    {
        //        TotalRecords = 1,
        //        Users = new List<ZoomUserRecord>
        //    {
        //        new ZoomUserRecord
        //        {
        //            Id = "asdfsdf9342535",
        //             FirstName = "TestFirstName",
        //             LastName = "TestLastName",
        //             Email = "test@test.com"
        //        }
        //    }
        //    };

        //    A.CallTo(() => _sut.Authenticate())
        //        .Returns("");
        //    A.CallTo(() => _client.GetZoomUsers(A<string>._, A<GetUsersRequest>._)).Returns(expectedResponse);

        //    var response = await _sut.GetUsers(new GetUsersRequest());

        //    response.ShouldNotBeNull();
        //    response.TotalRecords.ShouldBeEquivalentTo(expectedResponse.TotalRecords);
        //    response.Users.ShouldNotBeNull();
        //    response.Users.Count.ShouldBe(1);
        //    response.Users.Any(_ => _.Id == expectedResponse.Users[0].Id);
        //    response.Users.Any(_ => _.FirstName == expectedResponse.Users[0].FirstName);
        //    response.Users.Any(_ => _.LastName == expectedResponse.Users[0].LastName);
        //    response.Users.Any(_ => _.Email == expectedResponse.Users[0].Email);

        //}
    }
}
