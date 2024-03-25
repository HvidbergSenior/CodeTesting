using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Integration;
using deftq.BuildingBlocks.Integration.Inbox;
using Xunit;

namespace deftq.BuildingBlocks.Test.Integration
{
    public class InboxMessageTest
    {
        internal sealed class UserCreatedEvent : IntegrationEvent
        {
            public UserCreatedEvent(Guid id, DateTime occuredOn) : base(id, occuredOn)
            {
                
            }
        }

        internal sealed class CreateUserCommand : IInternalCommand, ICommand<EmptyCommandResponse>
        {
            public int Age { get; set; }
            public string Name { get; set; } = string.Empty;

            public CreateUserCommand(int age, string name)
            {
                Age = age;
                Name = name;
            }

            public CreateUserCommand()
            {
                
            }
        }

        [Fact]
        public void Will_Create_InboxMessage_With_MessageType()
        {
            var cmd = new CreateUserCommand(42, "do something");
            var inboxMessage = InboxMessage.From(cmd, Guid.NewGuid());

            Assert.Equal(cmd.GetType().AssemblyQualifiedName, inboxMessage.MessageType);
        }
    }
}
