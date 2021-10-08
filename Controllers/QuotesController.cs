using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Data;
using QuotesApi.Models;
using QuotesApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteRepository _quoteRepository;

        public QuotesController(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }
        // GET: api/<QuotesController>
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string sort)
        {
            try
            {
                var quotes = await _quoteRepository.Get(sort);

                if (quotes == null)
                {
                    return NotFound("No quotes posted");
                }

                return Ok(quotes);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }

        // GET api/<QuotesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var quote = await _quoteRepository.Get(id);

                if (quote == null)
                {
                    return NotFound("No quote found");
                }

                return Ok(quote);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        // GET: api/Quotes/Test/5
        //[HttpGet("[action]/{id}")]
        //public IActionResult Test(int id)
        //{
        //    return Ok(id);
        //}

        // POST api/<QuotesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Quote quote)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                return Unauthorized("You are not authorized to post");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    quote.UserId = userId;
                    var postedQuote = await _quoteRepository.Post(quote);

                    if (postedQuote == null)
                    {
                        return NotFound();
                    }

                    return StatusCode(StatusCodes.Status201Created, postedQuote);
                }
                catch (Exception e)
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }

        // PUT api/<QuotesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Quote quote)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                return Unauthorized("You are not authorized to update");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updatedQuote = await _quoteRepository.Put(userId, id, quote);

                    if (updatedQuote == null)
                    {
                        return NotFound();
                    }

                    return Ok(updatedQuote);
                }
                catch (Exception e)
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }

        // DELETE api/<QuotesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                return Unauthorized("You are not authorized to delete");
            }
            try
            {
                int result = await _quoteRepository.Delete(userId, id);

                if (result == -1)
                {
                    return NotFound();
                }

                return Ok("Successfully deleted");
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        //[HttpGet("[action]")]
        ////[Route("[action]")]
        //public IActionResult PagingQuote(int? pageNumber, int? pageSize)
        //{
        //    var quotes = _quotesDbContext.Quotes;
        //    int currentPageNumber = pageNumber ?? 1;
        //    int currentPageSize = pageSize ?? 5;

        //    //Apply Skip and Take algo
        //    return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        //}

        //[HttpGet]
        //[Route("[action]")]
        //public IActionResult SearchQuote(string type)
        //{
        //    var quotes = _quotesDbContext.Quotes.Where(q => q.Type.StartsWith(type));

        //    return Ok(quotes);
        //}

        //[HttpGet]
        //[Route("[action]")]
        //public IActionResult MyQuote()
        //{
        //    string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

        //    var quotes = _quotesDbContext.Quotes.Where(q => q.UserId == userId);

        //    return Ok(quotes);
        //}
    }
}
