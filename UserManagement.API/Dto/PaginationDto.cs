namespace UserManagement.API.Dto
{
    public class PaginationDto
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }

        public PaginationDto() { 
            pageNumber = 0; 
            pageSize = 5; 
            totalCount = 0;
        }
    }
}
