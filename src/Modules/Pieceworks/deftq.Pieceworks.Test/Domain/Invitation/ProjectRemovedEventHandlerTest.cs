using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.InvitationFlow;
using deftq.Pieceworks.Domain.project;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.Invitation
{
    public class ProjectRemovedEventHandlerTest
    {
        //[Fact]
        //public async Task ShouldNotReturnInvitationWhenProjectIsDeleted()
        //{
        //    var uow = new FakeUnitOfWork();
        //    var invitationRepository = new InvitationInMemoryRepository();

        //    var invitation = Any.Invitation();

        //    await invitationRepository.Add(invitation);

        //    var handler = new ProjectRemovedEventHandler(invitationRepository, uow);
        //    var evt = ProjectRemovedDomainEvent.Create(invitation.ProjectId);

        //    await handler.Handle(evt, CancellationToken.None);

        //    Assert.Empty(invitationRepository.Entities);
        //    Assert.True(invitationRepository.SaveChangesCalled);
        //}
    }
}
