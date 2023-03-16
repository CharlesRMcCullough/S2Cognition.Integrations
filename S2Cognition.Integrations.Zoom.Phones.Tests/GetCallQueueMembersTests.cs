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
}
