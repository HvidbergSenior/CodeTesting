using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public class NotAllowedToMoveWorkItemToOtherProjectException : Exception
    {
        public NotAllowedToMoveWorkItemToOtherProjectException() : base() { }

        public NotAllowedToMoveWorkItemToOtherProjectException(string? message) : base(message) { }

        public NotAllowedToMoveWorkItemToOtherProjectException(string? message, Exception? innerException) : base(message, innerException) { }

        public NotAllowedToMoveWorkItemToOtherProjectException(ProjectId sourceProjectId, ProjectId destinationProjectId) : 
            base($"Not allowed to move work items from project {sourceProjectId} to other project {destinationProjectId}") { }
    }
}
