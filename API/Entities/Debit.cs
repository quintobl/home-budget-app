namespace API.Entities
{
    public class Debit
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int? AccountId { get; set; }
    }
}