using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PrimeChecker.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrimeController : ControllerBase
    {
        private readonly ILogger<PrimeController> _logger;

        public PrimeController(ILogger<PrimeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Fetch lower prime number
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A prime number equals to or is lower than input</returns>
        [HttpGet("/{input}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public IActionResult Get(int input)
        {
            if (input < 0)
            {
                return BadRequest("Negative number is not allowed");
            }


            if (input < 2)
                return NotFound("Cannot find lower prime number");

            return Ok(FetchLowerPrime(input));
        }

        private int FetchLowerPrime(int input)
        {
            for (int i = input; i >= 2; i--)
            {
                if (IsPrime(i))
                    return i;
            }

            return 0;
        }

        private bool IsPrime(int input)
        {
            if (input == 2)
                return true;

            if (input < 2 || input % 2 == 0)
                return false;

            var maxRangeCheck = Math.Round(Math.Sqrt(input), 0);
            for (int i = 3; i <= maxRangeCheck; i = i + 2)
            {
                if (input % i == 0)
                    return false;
            }

            return true;
        }
    }
}
