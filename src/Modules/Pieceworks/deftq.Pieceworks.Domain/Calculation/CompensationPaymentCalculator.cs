using deftq.Pieceworks.Domain.Calculation.Expression;
using deftq.Pieceworks.Domain.projectLogBook;

namespace deftq.Pieceworks.Domain.Calculation
{
    public class CompensationPaymentCalculator
    {
        public CompensationPaymentResult CalculateUsersCompensationPayment(IList<ProjectLogBookUser> logBookParticipants, DateOnly periodStart, DateOnly periodEnd,
            decimal amount)
        {
            if (periodStart > periodEnd)
            {
                throw new ArgumentOutOfRangeException(nameof(periodStart), "Period start is later then period End");
            }
            var list = new List<CompensationPaymentParticipantResult>();
            foreach (var participant in logBookParticipants)
            {
                var hours = participant.GetSumClosedHoursInPeriod(periodStart, periodEnd);
                var payment = hours.Value.TotalHours == 0 || amount == 0 ? 0 : (decimal)hours.Value.TotalHours * amount;
                list.Add(new CompensationPaymentParticipantResult(participant.UserId, DecimalNumber.Number((decimal)hours.Value.TotalHours),
                    DecimalNumber.Number(payment)));
            }

            return new CompensationPaymentResult(list);
        }
    }

    public class CompensationPaymentResult
    {
        public IEnumerable<CompensationPaymentParticipantResult> Participants { get; private set; }

        public CompensationPaymentResult(IEnumerable<CompensationPaymentParticipantResult> participants)
        {
            Participants = participants;
        }
    }

    public class CompensationPaymentParticipantResult
    {
        public Guid ProjectParticipantId { get; private set; }
        public DecimalNumber Hours { get; private set; }
        public DecimalNumber Payment { get; private set; }

        public CompensationPaymentParticipantResult(Guid projectParticipantId, DecimalNumber hours, DecimalNumber payment)
        {
            ProjectParticipantId = projectParticipantId;
            Hours = hours;
            Payment = payment;
        }
    }
}
