namespace Events.API.ResourceParameters
{
    public class EventResourceParameters
    {
        const int maxPageSize = 10;
        public string Type { get; set; }
        public string SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;

        private int pageSize = 5;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > maxPageSize ? maxPageSize : value;
        }

        public string OrderBy { get; set; } = "Title";
        public string Fields { get; set; }
    }
}
