using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    [Serializable]
    public class Axis
    {
        public int Vertical;
        public int Horizontal;
    }

    public class GameState
    {
        public List<Player> Players { get; set; }
        public GameState()
        {
            Players = new List<Player>();
        }
    }
    internal class Game
    {
        const int WorldWidth = 500;
        const int WorldHeigh = 400;
        const int LoopPeriod = 10;
        public GameState State { get; set; }

        private  Dictionary<string, Axis> Axes;
        public Game()
        {
            State = new GameState();
            Axes = new Dictionary<string, Axis>();

            StartGameLoop();
        }
        public void SpawnPlayer(string id,string username)
        {
            Random random = new Random();
            State.Players.Add(new Player()
            {
                Id = id,
                Username = username,
                x = random.Next(10, WorldWidth - 10),
                y = random.Next(10, WorldHeigh - 10),
                Speed = 2,
                Radius = 10
            });

            Axes[id] = new Axis{Horizontal= 0,Vertical= 0 };

        }

        public void SetAxis(string id,Axis axis)
        {
            Axes[id]= axis;
        }

        public void Update()
        {
            foreach (var player in State.Players)
            {
                var axis = Axes[player.Id];

                if (axis.Horizontal > 0 && player.x < WorldWidth - player.Radius)
                {
                    player.x += player.Speed;
                }
                else if (axis.Horizontal < 0 && player.x > 0 + player.Radius)
                {
                    player.x -= player.Speed;
                }
                if (axis.Vertical > 0 && player.y < WorldHeigh - player.Radius)
                {
                    player.y += player.Speed;
                }
                else if (axis.Vertical < 0 && player.y > 0 + player.Radius)
                {
                    player.y -= player.Speed;
                }
            }
        }
        public void RemovePlayer(string id)
        {
            State.Players = State.Players.Where(player => player.Id != id).ToList();
            Axes.Remove(id);
        }

        void StartGameLoop()
        {
            var timer = new Timer((e) => Update(), null, 0, LoopPeriod);
        }

    }
}
