using QuotesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesApi.Repository
{
    public interface IQuoteRepository
    {
        Task<IEnumerable<Quote>> Get(string sort);
        Task<Quote> Get(int? id);
        Task<Quote> Post(Quote quote);
        Task<Quote> Put(string userId, int? id, Quote quote);
        Task<int> Delete(string userId, int? id);
    }
}
