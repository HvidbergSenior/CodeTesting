namespace deftq.BuildingBlocks.Integration.Scheduled
{
    public sealed class ScheduledCommand : IScheduledCommand
    {
        public Guid Id { get; private set; }
        public string CommandType { get; private set; } = string.Empty;
        public string Payload { get; private set; } = string.Empty;
        public DateTime EnqueueDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public int FailCount { get; private set; }
        public bool Failed { get; private set; }
        public DateTime? ProcessedDateTime { get; private set; }

        private ScheduledCommand(string commandType, string payload, DateTime dueDate)
        {
            CommandType = commandType;
            Payload = payload;
            DueDate = dueDate;
            EnqueueDate = DateTime.UtcNow;
        }

        private ScheduledCommand()
        {
        }

        internal static ScheduledCommand With(string commandType, string payload, DateTime duedate)
        {
            return new ScheduledCommand(commandType, payload, duedate);
        }

        internal void Published()
        {
            ProcessedDateTime = DateTime.UtcNow;
            Failed = false;
        }

        internal void PublishFailed()
        {
            FailCount++;
            switch (FailCount)
            {
                case 1:
                    DueDate = DueDate.AddSeconds(30);
                    break;

                case 2:
                    DueDate = DueDate.AddMinutes(5);
                    break;

                case 3:
                    DueDate = DueDate.AddMinutes(30);
                    break;

                case 4:
                    DueDate = DueDate.AddHours(5);
                    break;

                default:
                    Failed = true;
                    break;
            }
        }
    }
}
