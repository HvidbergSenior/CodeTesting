using System.Collections.ObjectModel;
using System.Text;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Domain.projectFolderRoot.Work
{
    public class WorkItemTest
    {
        [Fact]
        public void SerializeTest()
        {
            var workItem = WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), Any.CatalogMaterialId(), WorkItemDate.Create(new DateOnly()),
                WorkItemUser.Create(Guid.NewGuid(), "test"), WorkItemText.Create("3x1,5 kabel"), WorkItemEanNumber.Create("1234567891234"),
                WorkItemMountingCode.FromCode(3), WorkItemDuration.Create(2578), WorkItemAmount.Empty(),
                WorkItemUnit.Empty(), new Collection<SupplementOperation>(), new List<Supplement>());

            var serializer = Registration.GetJsonNetSerializer();
            var json = serializer.ToJson(workItem);

            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var obj = serializer.FromJson<WorkItem>(ms);
            Assert.NotNull(obj);
        }

        [Fact]
        public void When_Created_Should_Raise_Event()
        {
            var project = Any.WorkItem();
            Assert.Single(project.DomainEvents);
        }

        [Fact]
        public void When_Created_Should_Raise_WorkItemCreatedEvent()
        {
            var workItem = Any.WorkItem();
            Assert.IsType<WorkItemCreatedDomainEvent>(workItem.PublishedEvent<WorkItemCreatedDomainEvent>());
        }

        [Fact]
        public void When_Removed_Should_Raise_WorkItemRemovedEvent()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();

            projectFolderRoot.AddFolder(folder);

            var folderWork = Any.ProjectFolderWork();
            var workItem = Any.WorkItem();
            folderWork.AddWorkItem(workItem, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder));
            
            folderWork.RemoveWorkItems(new List<WorkItemId>{workItem.WorkItemId});
            Assert.IsType<WorkItemRemovedDomainEvent>(folderWork.PublishedEvent<WorkItemRemovedDomainEvent>());
        }
    }
}
