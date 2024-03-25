using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Application.GetProjectLogBook;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using ProjectParticipant = deftq.Pieceworks.Application.GetProject.ProjectParticipant;

namespace deftq.Pieceworks.Test.Application.GetProjectLogBook
{
    public class GetProjectLogBookQueryTests
    {
        [Fact]
        public Task Should_Throw_NotFoundException()
        {
            var query = GetProjectLogBookQuery.Create(ProjectId.Create(Guid.NewGuid()));
            var projectRepository = new ProjectInMemoryRepository();
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            var handler = new GetProjectLogBookQueryHandler(projectRepository, logBookRepository);
            
            return Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Should_Return_ProjectLogBook()
        {
            var project = Any.Project();
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            
            var projectRepository = new ProjectInMemoryRepository();
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            await projectRepository.Add(project);
            await logBookRepository.Add(projectLogBook);
            
            ProjectLogBookUser.Create(Guid.NewGuid());
            
            var query = GetProjectLogBookQuery.Create(projectLogBook.ProjectId);
            var handler = new GetProjectLogBookQueryHandler(projectRepository, logBookRepository);
            
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(projectLogBook.ProjectId.Value, response.ProjectId);
            Assert.Single(response.Users);
        }

        [Fact]
        public async Task No_Duplicate_Should_Be_Returned()
        {
            var project = Any.Project();
            var projectParticipant = Any.ProjectParticipant();
            
            project.AddProjectParticipant(projectParticipant);
            var projectLogBook = ProjectLogBook.Create(project.ProjectId, Any.ProjectLogBookId());
            projectLogBook.RegisterWeek(ProjectLogBookUser.Create(LogBookName.Create(projectParticipant.Name.Value), projectParticipant.Id), LogBookYear.Create(2022), LogBookWeek.Create(32), LogBookNote.Empty(), new List<ProjectLogBookDay>()  );
            projectLogBook.RegisterWeek(ProjectLogBookUser.Create(Guid.NewGuid()) ,LogBookYear.Create(2022), LogBookWeek.Create(32), LogBookNote.Empty(), new List<ProjectLogBookDay>() );
            var projectRepository = new ProjectInMemoryRepository();
            var logBookRepository = new ProjectLogBookInMemoryRepository();
            
            await projectRepository.Add(project);
            await logBookRepository.Add(projectLogBook);

            var query = GetProjectLogBookQuery.Create(projectLogBook.ProjectId);
            var handler = new GetProjectLogBookQueryHandler(projectRepository, logBookRepository);
            
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(3, response.Users.Count);
            Assert.Single(response.Users.Where(p => p.UserId == projectParticipant.Id));
        }
    }
}
