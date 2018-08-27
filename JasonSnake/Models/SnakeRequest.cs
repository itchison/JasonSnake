using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JasonSnake.Models
{
    public class SnakeRequest
    {
        public Game game { get; set; }
        public int turn { get; set; }
        public Board board { get; set; }
        public Snake you { get; set; }
    }
}
