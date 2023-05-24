using FakeItEasy;
using MailChimp.Net.Models;
using S2Cognition.Integrations.MailChimp.Core.Data;
using Shouldly;

namespace S2Cognition.Integrations.MailChimp.Core.Tests
{
    public class MemberAddUpdateTests : IntegrationTests
    {
        [Fact]
        public async Task EnsureMailChimpAdUpdateMemberListIdIsNotNull()
        {
            await _sut.Initialize(_configuration);

            var ex = await Should.ThrowAsync<ArgumentException>(async () => await _sut.MailChimpAddUpdateMember(new AddUpdateMemberRequest()));

            ex.ShouldNotBeNull();
            ex.Message.ShouldBe(nameof(AddUpdateMemberRequest.ListId));
        }

        [Fact]
        public async Task EnsureMailChimpAdUpdateMemberEmailAddressIsNotNull()
        {
            await _sut.Initialize(_configuration);

            var ex = await Should.ThrowAsync<ArgumentException>(async () => await _sut.MailChimpAddUpdateMember(new AddUpdateMemberRequest()
            {
                ListId = "fake list Id"
            }
                ));

            ex.ShouldNotBeNull();
            ex.Message.ShouldBe(nameof(AddUpdateMemberRequest.EmailAddress));
        }

        [Fact]
        public async Task EnsureMailChimpAdUpdateMemberReturnsExpectedResults()
        {
            await _sut.Initialize(_configuration);

            string expectedEmailAddress = "fakename@fake.com";
            string expectedListId = "fakeListId";

            var expectedResponse = new Member
            {
                ListId = expectedListId,
                EmailAddress = expectedEmailAddress,
                FullName = "fakeFullName",
                Status = Status.Subscribed
            };

            A.CallTo(() => _client.MemberAddOrUpdate(A<string>._, A<Member>._)).Returns(expectedResponse);

            var response = await _sut.MailChimpAddUpdateMember(new AddUpdateMemberRequest
            {
                ListId = "fakelist",
                EmailAddress = "fake@email.com",
                Subscribed = true,
            });

            response.ShouldNotBeNull();
            response.ListId.ShouldBe(expectedListId);
            response.EmailAddress.ShouldBe(expectedEmailAddress);

        }
    }
}
