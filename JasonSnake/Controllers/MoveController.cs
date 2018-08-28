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
                    AdjustGrid(body.x, body.y, -1000);
                    AdjustScoreRadius(body.x, body.y, 3, -100);
                    AdjustScoreRadius(body.x, body.y, 2, -200);
                    AdjustScoreRadius(body.x, body.y, 1, -300);
                }
            }

        }

        void ScorePellets(Board board)
        {
            foreach (var pellet in board.food)
            {
                AdjustGrid(pellet.x, pellet.y, 1000);
                AdjustScoreRadius(pellet.x, pellet.y, 5, 50);
                AdjustScoreRadius(pellet.x, pellet.y, 4, 100);
                AdjustScoreRadius(pellet.x, pellet.y, 3, 200);
                AdjustScoreRadius(pellet.x, pellet.y, 2, 300);
            }
        }

        /// <summary>
        /// We create the GridScore to have a 1 cell border that we score very low
        /// regular x,y can be used afterwards since we start at -1.
        /// </summary>
        /// <param name="board"></param>
        void InitializeBoard(Board board)
        {
            //board loop
            Random r = new Random();
            for (int x = -1; x < board.width + 1; x++)
            {
                for (int y = -1; y < board.height + 1; y++)
                {
                    GridScores.Add(x, y, r.Next(1, 5));// give every cell a start of 1-5
                    //edges
                    if (y == -1 || y == board.height || x == -1 || x == board.width)
                    {
                        AdjustGrid(x, y, -100000);
                    }
                }
            }
            
        }

        void AdjustScoreRadius(int pelletx, int pellety, int radius, int score)
        {
            foreach (var coord in GridScores.Keys.ToList())
            {
                //basic math stolen from stackoverflow
                int deltaX = pelletx - coord.Item1, deltaY = pellety - coord.Item2;
                if ((deltaX * deltaX) + (deltaY * deltaY) <= (radius * radius))
                {
                    AdjustGrid(coord.Item1, coord.Item2, score);
                }
            }
        }

        int GetScore(int x, int y)
        {
            return GridScores[x, y];
        }

        string GetMove(Snake you)
        {
            string move = "";
            var head = you.body[0];
            int upscore = GetScore(head.x, head.y - 1);
            int downscore = GetScore(head.x, head.y + 1);
            int rightscore = GetScore(head.x + 1, head.y);
            int leftscore = GetScore(head.x - 1, head.y);

            //todo this is too predictable
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