using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JasonSnake.Models
{
    public class Board
    {
        public int height { get; set; }
        public int width { get; set; }
        public List<Coords> food { get; set; }
        public List<Snake> snakes { get; set; }
    }
}
