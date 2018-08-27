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
    public class MoveController : ControllerBase
    {

        Dictionary<int, int, int> GridScores;

        public MoveController()
        {
            GridScores = new Dictionary<int, int, int>();
        }

        [HttpPost]
        public MoveResponse Get(SnakeRequest sr)
        {
            InitializeBoard(sr.board);
            ScoreSnakes(sr.board);
            ScorePellets(sr.board);

            var bestmove = GetMove(sr.you);
            //return new MoveResponse() { move = "up" };
            return new MoveResponse() { move = bestmove };
        }

        void ScoreSnakes(Board board)
        {
            foreach (var snake in board.snakes)
            {
                foreach (var body in snake.body)
                {
                    AdjustGrid(body.x + 1, body.y + 1, -1000); //account for board being a bit longer
                    AdjustScoreRadius(board, body.x + 1, body.y + 1, 3, -100);
                    AdjustScoreRadius(board, body.x + 1, body.y + 1, 2, -200);
                    AdjustScoreRadius(board, body.x + 1, body.y + 1, 1, -300);
                }
            }

        }

        void ScorePellets(Board board)
        {
            foreach (var pellet in board.food)
            {
                AdjustGrid(pellet.x + 1, pellet.y + 1, 1000);
                AdjustScoreRadius(board, pellet.x + 1, pellet.y + 1, 5, 50);
                AdjustScoreRadius(board, pellet.x + 1, pellet.y + 1, 4, 100);
                AdjustScoreRadius(board, pellet.x + 1, pellet.y + 1, 3, 200);
                AdjustScoreRadius(board, pellet.x + 1, pellet.y + 1, 2, 300);
            }
        }

        void InitializeBoard(Board board)
        {
            //board loop
            Random r = new Random();
            for (int x = 0; x < board.width + 2; x++) //2 so we can do edges
            {
                for (int y = 0; y < board.height + 2; y++)
                {
                    GridScores.Add(x, y, r.Next(1, 5));// give every cell a start of 1-5
                    //edges
                    if (y == 0 || y == board.height + 1 || x == 0 || x == board.width + 1)
                    {
                        AdjustGrid(x, y, -100000);
                    }
                }
            }
        }

        void AdjustScoreRadius(Board board, int pelletx, int pellety, int radius, int score)
        {
            //https://stackoverflow.com/questions/12364690/calculate-the-coordinates-in-a-circle
            //I'd rather iterate the dictionary but this will do
            for (int x = 0; x < board.width + 2; x++) //2 so we can do edges
            {
                for (int y = 0; y < board.height + 2; y++)
                {
                    int deltaX = pelletx - x, deltaY = pellety - y;
                    // compare the square distance, to avoid an unnecessary square-root
                    if ((deltaX * deltaX) + (deltaY * deltaY) <= (radius * radius))
                    {
                        AdjustGrid(x, y, score);
                    }
                }
            }

        }

        int GetScore(int x, int y)
        {
            return GridScores[x + 1, y + 1];
        }

        string GetMove(Snake you)
        {
            string move = "";
            var head = you.body[0];
            int upscore = GetScore(head.x, head.y - 1);
            int downscore = GetScore(head.x, head.y + 1);
            int rightscore = GetScore(head.x + 1, head.y);
            int leftscore = GetScore(head.x - 1, head.y);

            //todo this is to predictable
            if (upscore >= downscore && upscore >= rightscore && upscore >= leftscore)
            {
                move = "up";
            }
            if (downscore >= upscore && downscore >= rightscore && downscore >= leftscore)
            {
                move = "down";
            }
            if (rightscore >= upscore && rightscore >= downscore && rightscore >= leftscore)
            {
                move = "right";
            }
            if (leftscore >= upscore && leftscore >= downscore && leftscore >= rightscore)
            {
                move = "left";
            }
            return move;
        }

        void AdjustGrid(int x, int y, int adj)
        {
            GridScores[x, y] = GridScores[x, y] + adj;
        }

    }



    public class Dictionary<TKey1, TKey2, TValue> : Dictionary<Tuple<TKey1, TKey2>, TValue>, IDictionary<Tuple<TKey1, TKey2>, TValue>
    {

        public TValue this[TKey1 key1, TKey2 key2]
        {
            get { return base[Tuple.Create(key1, key2)]; }
            set { base[Tuple.Create(key1, key2)] = value; }
        }

        public void Add(TKey1 key1, TKey2 key2, TValue value)
        {
            base.Add(Tuple.Create(key1, key2), value);
        }

        public bool ContainsKey(TKey1 key1, TKey2 key2)
        {
            return base.ContainsKey(Tuple.Create(key1, key2));
        }
    }
}