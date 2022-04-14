using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Extensions
{

    // class that is responsible for adding a "pagination" header 
    // for response that we get back from the API
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage, 
            int itemsPerPage, int totalItems, int totalPages) {
            
            // creating an anonymous object
            var paginationHeader = new {
                currentPage,
                itemsPerPage,
                totalItems,
                totalPages
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}