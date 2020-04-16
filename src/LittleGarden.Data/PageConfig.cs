namespace LittleGarden.Data
{
    public class PageConfig
    {
        public PageConfig(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; }
        public int PageSize { get; }
    }
}