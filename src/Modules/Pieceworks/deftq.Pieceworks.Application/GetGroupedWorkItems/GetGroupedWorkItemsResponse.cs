namespace deftq.Pieceworks.Application.GetGroupedWorkItems
{
    public class GetGroupedWorkItemsQueryResponse
    {
        public IList<GroupedWorkItemsResponse> GroupedWorkItems { get; private set; }

        private GetGroupedWorkItemsQueryResponse()
        {
            GroupedWorkItems = new List<GroupedWorkItemsResponse>();
        }
        
        public GetGroupedWorkItemsQueryResponse(IList<GroupedWorkItemsResponse> groupedWorkItems)
        {
            GroupedWorkItems = groupedWorkItems;
        }

    }
    
    public class GroupedWorkItemsResponse
    {
        public string Id { get; private set; }
        public string Text { get; private set; }
        public decimal Amount { get; private set; }
        public decimal PaymentDkr { get; private set; }
        
        private GroupedWorkItemsResponse()
        {
            Id = string.Empty;
            Text = string.Empty;
            Amount = decimal.Zero;
            PaymentDkr = decimal.Zero;
        }
        
        public GroupedWorkItemsResponse( string id, string text, decimal amount, decimal paymentDkr )
        {
            Id = id;
            Text = text;
            Amount = amount;
            PaymentDkr = paymentDkr;
        }

    }
}
