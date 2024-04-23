namespace Domain
{
    public record InvoiceRequestEvent
    {
        public int StoreId { get; set; }
        public int PosId { get; set; }
        public string OperatorId { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
