namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveProjectSpecificOperations
{
    public class RemoveProjectSpecificOperationsRequest
    {
        public IList<Guid> ProjectSpecificOperationIds { get; private set; }

        public RemoveProjectSpecificOperationsRequest(IList<Guid> projectSpecificOperationIds)
        {
            ProjectSpecificOperationIds = projectSpecificOperationIds;
        }
    }
}