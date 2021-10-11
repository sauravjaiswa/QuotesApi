using Microsoft.EntityFrameworkCore;
using QuotesApi.Data;
using QuotesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesApi.Repository
{
    public class SqlQuoteRepository : IQuoteRepository
    {
        private readonly QuotesDbContext _quotesDbContext;

        public SqlQuoteRepository(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }
        public async Task<int> Delete(string userId, int? id)
        {
            int result = -1;
            if (_quotesDbContext != null)
            {
                var quote = await _quotesDbContext.Quotes.FindAsync(id);

                if (quote != null)
                {
                    if (userId != quote.UserId)
                    {
                        throw new Exception();
                    }

                    _quotesDbContext.Quotes.Remove(quote);
                    result = await _quotesDbContext.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<IQueryable<Quote>> Get(string sort)
        {
            IQueryable<Quote> quotes = null;

            if (_quotesDbContext != null)
            {
                if (_quotesDbContext.Quotes.Count() == 0)
                {
                    return null;
                }

                switch (sort)
                {
                    case "desc":
                        quotes = _quotesDbContext.Quotes.OrderByDescending(q => q.CreatedAt);
                        break;
                    case "asc":
                        quotes = _quotesDbContext.Quotes.OrderBy(q => q.CreatedAt);
                        break;
                    default:
                        //throw new Exception();
                        quotes = _quotesDbContext.Quotes;
                        break;
                }
            }

            return quotes;
        }

        public async Task<Quote> Get(int? id)
        {
            if (_quotesDbContext != null)
            {
                var quote = await _quotesDbContext.Quotes.FindAsync(id);

                return quote;
            }

            return null;
        }

        public async Task<Quote> Post(Quote quote)
        {
            if (_quotesDbContext != null)
            {
                await _quotesDbContext.Quotes.AddAsync(quote);
                await _quotesDbContext.SaveChangesAsync();

                return quote;
            }

            return null;

        }

        public async Task<Quote> Put(string userId, int? id, Quote quote)
        {
            if (_quotesDbContext != null)
            {
                var entity = await _quotesDbContext.Quotes.FindAsync(id);

                if (entity != null)
                {
                    if (userId != entity.UserId)
                    {
                        throw new Exception();
                    }

                    entity.Title = quote.Title;
                    entity.Author = quote.Author;
                    entity.Description = quote.Description;
                    entity.Type = quote.Type;
                    entity.CreatedAt = quote.CreatedAt;

                    await _quotesDbContext.SaveChangesAsync();

                    return entity;
                }

                return null;
            }
            return null;
        }
    }
}
