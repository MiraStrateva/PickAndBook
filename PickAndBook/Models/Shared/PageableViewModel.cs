using PickAndBook.Models.Shared.Contracts;
using System.Collections.Generic;

namespace PickAndBook.Models.Shared
{
    public class PageableViewModel<T> : IPageable
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }
    }
}