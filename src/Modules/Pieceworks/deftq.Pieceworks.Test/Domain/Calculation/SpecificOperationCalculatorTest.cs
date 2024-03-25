using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using deftq.Pieceworks.Domain.projectSpecificOperation;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.Calculation.ProjectSpecificOperationCalculatorTest
{
    public class SpecificOperationCalculatorTest
    {
        [Fact]
        public void CalculateSpecificOperationTime_ShouldCalculate()
        { 
            var defaultBaseRateAndSupplement = BaseRateAndSupplementTestUtil.GetDefaultBaseRateAndSupplement();
            var folderRoot = ProjectFolderRoot.Create(Any.ProjectId(), Any.ProjectName(), Any.ProjectFolderRootId(),
                FolderRateAndSupplement.Create(defaultBaseRateAndSupplement));
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(defaultBaseRateAndSupplement, folderRoot.RootFolder);
            var calculator = new ProjectSpecificOperationCalculator(baseRateAndSupplementProxy, new SystemTime());
            ProjectSpecificOperationTime workTime = ProjectSpecificOperationTime.Create(392474.88m);
            var operationTime = calculator.CalculateOperationTime(workTime);
            Assert.Equal(223200, operationTime.Value);
        }
        
        [Fact]
        public void CalculateSpecificOperationTime_ShouldCalculateAnotherVariation()
        { 
            var defaultBaseRateAndSupplement = BaseRateAndSupplementTestUtil.GetDefaultBaseRateAndSupplement();
            var folderRoot = ProjectFolderRoot.Create(Any.ProjectId(), Any.ProjectName(), Any.ProjectFolderRootId(),
                FolderRateAndSupplement.Create(defaultBaseRateAndSupplement));
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(defaultBaseRateAndSupplement, folderRoot.RootFolder);
            var calculator = new ProjectSpecificOperationCalculator(baseRateAndSupplementProxy, new SystemTime());
            ProjectSpecificOperationTime workTime = ProjectSpecificOperationTime.Create(43960m);
            var operationTime = calculator.CalculateOperationTime(workTime);
            Assert.Equal(25000, operationTime.Value);
        }

        [Fact]
        public void CalculateSpecificOperationTime_IgnoreSupplements()
        {
            var defaultBaseRateAndSupplement = BaseRateAndSupplementTestUtil.GetDefaultBaseRateAndSupplement();
            var folderRoot = ProjectFolderRoot.Create(Any.ProjectId(), Any.ProjectName(), Any.ProjectFolderRootId(),
                FolderRateAndSupplement.Create(defaultBaseRateAndSupplement));
            
            var rootFolderSupplement = FolderSupplement.Create(SupplementId.Create(Any.Guid()), CatalogSupplementId.Create(Any.Guid()),
                SupplementNumber.Create("HLM42"), SupplementText.Create("Meget højt i lave steder 42%"), SupplementPercentage.Create(42));

            folderRoot.RootFolder.UpdateFolderSupplements(folderRoot.ProjectId, new List<FolderSupplement> { rootFolderSupplement });
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(defaultBaseRateAndSupplement, folderRoot.RootFolder);
            var calculator = new ProjectSpecificOperationCalculator(baseRateAndSupplementProxy, new SystemTime());
            ProjectSpecificOperationTime workTime = ProjectSpecificOperationTime.Create(43960m);
            var operationTime = calculator.CalculateOperationTime(workTime);
            Assert.Equal(25000, operationTime.Value);
        }
        
        [Fact]
        public void CalculateSpecificOperationTimeWithDifferentBaseRate_ShouldCalculate()
        {
            var defaultBaseRateAndSupplement = BaseRateAndSupplementTestUtil.CreateBaseRateAndSupplement(50, 4, 3, 320.75m, 0);
            var folderRoot = ProjectFolderRoot.Create(Any.ProjectId(), Any.ProjectName(), Any.ProjectFolderRootId(),
                FolderRateAndSupplement.Create(defaultBaseRateAndSupplement));
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(defaultBaseRateAndSupplement, folderRoot.RootFolder);
            var calculator = new ProjectSpecificOperationCalculator(baseRateAndSupplementProxy, new SystemTime());
            ProjectSpecificOperationTime workTime = ProjectSpecificOperationTime.Create(43960m);
            var operationTime = calculator.CalculateOperationTime(workTime);
            Assert.Equal(27735.02m, operationTime.Value, 2);
        }

        [Fact]
        public void CalculateSpecificOperationTimeWithZeroBaseRate_WorkTimeShouldBeSameAsOperationTime()
        {
            var defaultBaseRateAndSupplement = BaseRateAndSupplementTestUtil.CreateBaseRateAndSupplement(0, 0, 0, 0, 0);
            var folderRoot = ProjectFolderRoot.Create(Any.ProjectId(), Any.ProjectName(), Any.ProjectFolderRootId(),
                FolderRateAndSupplement.Create(defaultBaseRateAndSupplement));
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(defaultBaseRateAndSupplement, folderRoot.RootFolder);
            var calculator = new ProjectSpecificOperationCalculator(baseRateAndSupplementProxy, new SystemTime());
            ProjectSpecificOperationTime workTime = ProjectSpecificOperationTime.Create(43960m);
            var operationTime = calculator.CalculateOperationTime(workTime);
            Assert.Equal(43960, operationTime.Value);
        }

        [Fact]
        public void CalculateSpecificOperationTimeWithDifferentPersonalTime_ShouldCalculate()
        {
            var defaultBaseRateAndSupplement = BaseRateAndSupplementTestUtil.CreateBaseRateAndSupplement(64, 2, 20, 214.75m, 0);
            var folderRoot = ProjectFolderRoot.Create(Any.ProjectId(), Any.ProjectName(), Any.ProjectFolderRootId(),
                FolderRateAndSupplement.Create(defaultBaseRateAndSupplement));
            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(defaultBaseRateAndSupplement, folderRoot.RootFolder);
            var calculator = new ProjectSpecificOperationCalculator(baseRateAndSupplementProxy, new SystemTime());
            ProjectSpecificOperationTime workTime = ProjectSpecificOperationTime.Create(392474.88m);
            var operationTime = calculator.CalculateOperationTime(workTime);
            Assert.Equal(197421.97m, operationTime.Value, 2);
        }
    }
}
