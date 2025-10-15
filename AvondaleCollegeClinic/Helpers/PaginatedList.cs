using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AvondaleCollegeClinic.Helpers
{
    // A helper list that knows about pages.
    // It holds the items for one page plus info like the current page number and total pages.
    public class PaginatedList<T> : List<T>
    {
        // The page we are on. First page = 1.
        public int PageIndex { get; private set; }

        // How many pages exist in total.
        public int TotalPages { get; private set; }

        // Build the paginated list with the items for this page.
        // count = total number of items across all pages.
        // pageIndex = which page we want (1-based).
        // pageSize = how many items per page.
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;

            // Compute total pages by dividing total items by page size and rounding up.
            // Example: 23 items with page size 10 -> 3 pages.
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            // Put the page items into this list (since we inherit from List<T>).
            this.AddRange(items);
        }

        // True when there is a page before the current one.
        public bool HasPreviousPage => PageIndex > 1;

        // True when there is a page after the current one.
        public bool HasNextPage => PageIndex < TotalPages;

        // Factory method that creates a PaginatedList from an IQueryable.
        // This runs efficient SQL using EF Core because it applies Count, Skip and Take on the server.
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            // Count how many rows match the query in total (no pagination here).
            var count = await source.CountAsync();

            // Skip rows from earlier pages then take only the rows for this page.
            // Example: pageIndex 2 and pageSize 10 -> skip 10 then take 10.
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return the paginated list with the page items and the total count.
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
