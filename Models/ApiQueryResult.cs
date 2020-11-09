using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myWebApi.Models {
    public class ApiQueryResult<T> {
        private ApiQueryResult(List<T> data, int count, int pageIndex, int pageSize){
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
        public static async Task<ApiQueryResult<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize
        ) {
            var count = await source.CountAsync();
            var data  = await source
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return new ApiQueryResult<T>(
                data, count, pageIndex, pageSize
            );
        }
        public List<T> Data { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage {
            get { return (PageIndex > 0); }
        }
        public bool HasNextPage {
            get { return (PageIndex < TotalPages); }
        }
    }
}