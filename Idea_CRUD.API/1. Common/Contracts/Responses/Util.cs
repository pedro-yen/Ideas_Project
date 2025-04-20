using System.Collections.Generic;

namespace Backend.Challenge._1._Common.Contracts.Responses
{
    public class Util
    {
        public class PaginatedResponse<T>
        {
            public List<T> Items { get; set; }
            public int TotalItems { get; set; }
            public int TotalPages { get; set; }
            public int CurrentPage { get; set; }
            public int PageSize { get; set; }
        }
    }
}
