using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JasonSnake.Models;

namespace JasonSnake.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class EndController : ControllerBase
    {
        [HttpPost]
        public void Post(SnakeRequest sr)
        {
            //It'd be fun to put some logic about win loss ratio into a table of some sort.
        }
    }
}