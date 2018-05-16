using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    public class LesiController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {            
            var colors = new SortedSet<string>(new string[] {"red", "blue"});
            var name = new SortedSet<string>(new string[] {"george", "nick"});
            
            return new string[] {"value1", "value2"};
        }
    }
}