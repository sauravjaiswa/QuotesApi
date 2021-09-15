using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Data;
using QuotesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly QuotesDbContext _quotesDbContext;

        public QuotesController(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }
        // GET: api/<QuotesController>
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public IActionResult Get(string sort)
        {
            IQueryable<Quote> quotes;

            switch (sort)
            {
                case "desc":
                    quotes=_quotesDbContext.Quotes.OrderByDescending(q => q.CreatedAt);
                    break;
                case "asc":
                    quotes = _quotesDbContext.Quotes.OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = _quotesDbContext.Quotes;
                    break;
            }

            //return _quotesDbContext.Quotes;
            return Ok(quotes);
            //return StatusCode(StatusCodes.Status200OK);
        }

        // GET api/<QuotesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var quote = _quotesDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound("No record found against this id...");
            }

            return Ok(quote);
        }

        // GET: api/Quotes/Test/5
        [HttpGet("[action]/{id}")]
        public IActionResult Test(int id)
        {
            return Ok(id);
        }

        // POST api/<QuotesController>
        [HttpPost]
        public IActionResult Post([FromBody] Quote quote)
        {
            _quotesDbContext.Quotes.Add(quote);
            _quotesDbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<QuotesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Quote quote)
        {
            var entity = _quotesDbContext.Quotes.Find(id);

            if (entity == null)
            {
                return NotFound("No record found against this id...");
            }
            else
            {
                entity.Title = quote.Title;
                entity.Author = quote.Author;
                entity.Description = quote.Description;
                entity.Type = quote.Type;
                entity.CreatedAt = quote.CreatedAt;

                _quotesDbContext.SaveChanges();

                return Ok("Record Updated Successfully...");
            }
        }

        // DELETE api/<QuotesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var quote = _quotesDbContext.Quotes.Find(id);

            if (quote == null)
            {
                return NotFound("No record found against this id...");
            }
            else
            {
                _quotesDbContext.Quotes.Remove(quote);
                _quotesDbContext.SaveChanges();
                return Ok("Record deleted...");
            }                
        }

        [HttpGet("[action]")]
        //[Route("[action]")]
        public IActionResult PagingQuote(int? pageNumber, int? pageSize)
        {
            var quotes = _quotesDbContext.Quotes;
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;

            //Apply Skip and Take algo
            return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult SearchQuote(string type)
        {
            var quotes = _quotesDbContext.Quotes.Where(q => q.Type.StartsWith(type));

            return Ok(quotes);
        }
    }
}
