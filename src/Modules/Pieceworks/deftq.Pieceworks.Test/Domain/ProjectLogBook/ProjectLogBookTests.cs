using deftq.BuildingBlocks.Serialization;
using deftq.Pieceworks.Domain.projectLogBook;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.projectLogBook
{
    public class ProjectLogBookTests
    {
        [Fact]
        public void CanSerializeAndDeserialize()
        {
            var user = ProjectLogBookUser.Create(LogBookName.Create("Hans"), new Guid());
            var year = LogBookYear.Create(2022);
            var week = LogBookWeek.Create(12);
            var serializer = new JsonSerializer<ProjectLogBook>();
            var obj = Any.ProjectLogBook();
            
            obj.RegisterWeek(user, year, week, Any.LogBookNote(), 
                new List<ProjectLogBookDay>() );
            obj.UpdateSalaryAdvance(user, year, week, LogBookSalaryAdvanceRole.Apprentice, LogBookSalaryAdvanceAmount.Create(42));
            
            var json = serializer.Serialize(obj);
            var result = serializer.Deserialize(json);
            Assert.Equal(obj.ProjectLogBookId.Id, result?.ProjectLogBookId.Id);
            Assert.Equal(21, result?.ProjectLogBookUsers[0].ProjectLogBookWeeks[0].LogBookDays[0].Date.Value.Day);
            Assert.Equal(3, result?.ProjectLogBookUsers[0].ProjectLogBookWeeks[0].LogBookDays[0].Date.Value.Month);
            Assert.Equal(2022, result?.ProjectLogBookUsers[0].ProjectLogBookWeeks[0].LogBookDays[0].Date.Value.Year);
            Assert.Equal(LogBookSalaryAdvanceRole.Apprentice, result?.ProjectLogBookUsers[0].ProjectLogBookWeeks[0].SalaryAdvance.Role);
            Assert.Equal(42, result?.ProjectLogBookUsers[0].ProjectLogBookWeeks[0].SalaryAdvance?.Amount?.Value);
        }

        [Fact]
        public void When_Created_Should_Have_ProjectLogBookId()
        {
            var projectId = Any.ProjectId();
            var projectLogBookId = Any.ProjectLogBookId();
            var projectLogBook = ProjectLogBook.Create(projectId, projectLogBookId);

            Assert.Equal(projectLogBookId, projectLogBook.ProjectLogBookId);
        }
    }
}
