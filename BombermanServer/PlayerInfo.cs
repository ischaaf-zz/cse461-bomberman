using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace BombermanServer
{
    public class PlayerInfo : IComparable
    {
        public NetConnection playerConnection { get; set; }
        public int playerID { get; set; }
        public float latency { get; set; }
        public Boolean alive;

        public PlayerInfo(NetConnection playerConnection, int playerID, float latency)
        {
            this.playerConnection = playerConnection;
            this.playerID = playerID;
            this.latency = latency;
            alive = true;
        }

        public int CompareTo(object obj)
        {
            PlayerInfo other = (PlayerInfo)obj;
            // this probably won't work
            if (!this.alive && !other.alive)
            {
                return 0;
            } else if (!this.alive)
            {
                return Int32.MaxValue;
            } else if (!other.alive)
            {
                return Int32.MinValue;
            } else
            {
                return (int) (this.latency - other.latency);
            }
            // throw new NotImplementedException();
        }
    }
}
