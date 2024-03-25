namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RemoveExtraWorkAgreement
{
    public class RemoveExtraWorkAgreementRequest
    {
        public IList<Guid> ExtraWorkAgreementIds { get; }

        public RemoveExtraWorkAgreementRequest(IList<Guid> extraWorkAgreementIds)
        {
            ExtraWorkAgreementIds = extraWorkAgreementIds;
        }
    }
}
