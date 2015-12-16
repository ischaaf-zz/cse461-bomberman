using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BombermanObjects.Logical
{
    public class PowerUp : AbstractGameObject
    {
        public enum PowerUpType
        {
            None, Speed, BombCap, BombPower, AutoBomb, Pierce, BombPass
        }

        protected int xPos;
        protected int yPos;

        public PowerUpType Type { get; }

        public PowerUp(GameManager m, PowerUpType type, int x, int y) : base(m)
        {
            Type = type;
            xPos = x;
            yPos = y;
        }

        public override Rectangle Position
        {
            get
            {
                return new Rectangle(xPos*GameManager.BOX_WIDTH, yPos*GameManager.BOX_WIDTH, GameManager.BOX_WIDTH, GameManager.BOX_WIDTH);
            }

            set
            {
                base.Position = value;
            }
        }

        public void Apply(Player p)
        {
            switch (Type)
            {
                case PowerUpType.BombCap:
                    p.MaxBombs++;
                    break;
                case PowerUpType.BombPower:
                    p.BombPower++;
                    break;
                case PowerUpType.Speed:
                    p.Speed = Math.Min(5, p.Speed + 1);
                    break;
                case PowerUpType.AutoBomb:
                    p.AutoBomb = true;
                    break;
                case PowerUpType.Pierce:
                    p.Pierce += 3;
                    break;
                case PowerUpType.BombPass:
                    p.BombPass = true;
                    break;
            }
        }
    }
}
