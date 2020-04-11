namespace LittleGarden.Data
{
    public class PageConfig
    {
        public int Page { get; }
        public int PageSize { get; }

        public PageConfig(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}