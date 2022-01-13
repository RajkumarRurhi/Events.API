namespace Events.API.ResourceParameters
{
    public class GuestsResourceParameters
    {
        const int maxPageSize = 10;
        public string SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;

        private int pageSize = 5;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > maxPageSize ? maxPageSize : value;
        }

        public string OrderBy { get; set; } = "Email";
        public string Fields { get; set; }
    }
}
