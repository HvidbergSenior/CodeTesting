namespace deftq.BuildingBlocks.Validation.Webapi.Messages
{
    public static class ExceptionMessages
    {
        public static string InvalidEmail(string email) { return $"The email: {email} is not valid."; }
        public static string InvitationDoesNotExist(Guid invitationId, Guid projectId) { return $"Invitation with id {invitationId} and projectId {projectId} does not exist"; }

    }
}
