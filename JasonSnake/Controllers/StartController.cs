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
    public class StartController : ControllerBase
    {
        [HttpPost]
        public StartResponse Post(SnakeRequest sr)
        {
            return new StartResponse() { color = "#ff00ff" };
        }
    }
}