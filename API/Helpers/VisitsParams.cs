namespace API.Helpers
{
    public class VisitsParams: PaginationParams
    {
        public int UserId { get; set; }
        public string predicate { get; set; }
    }
}