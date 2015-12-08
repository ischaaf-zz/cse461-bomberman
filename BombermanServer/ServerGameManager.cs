using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using BombermanObjects;
using Microsoft.Xna.Framework;

namespace BombermanServer
{
    public class ServerGameManager : GameManager
    {
        public static readonly int BROADCAST_INTERVAL = 60; // in ticks
        public NetServer server;
        PlayerInfo[] playerInfoArr;

        public ServerGameManager (NetServer server, PlayerInfo[] playerInfoArr, int players) : base(players)
        {
            this.server = server;
            this.playerInfoArr = playerInfoArr;
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
            // broadcast gamestate
            if (gametime.ElapsedGameTime.Ticks % BROADCAST_INTERVAL == 0)
            {
                // send game state
                NetOutgoingMessage outmsg = GetPackagedGameState();
                for (int i = 0; i < playerInfoArr.Length; i++)
                {
                    NetConnection currConnection = playerInfoArr[i].playerConnection;
                    server.SendMessage(outmsg, currConnection, NetDeliveryMethod.UnreliableSequenced, 0);
                }
            }
        }

        public NetOutgoingMessage GetPackagedGameState()
        {
            NetOutgoingMessage outmsg = server.CreateMessage();

            return outmsg;
        }

        public NetOutgoingMessage GetFullGameState()
        {
            NetOutgoingMessage outmsg = server.CreateMessage();
            outmsg.WriteVariableInt32(0);
            outmsg.Write((byte)PacketTypeEnums.PacketType.GAME_STATE_FULL);
            outmsg.WriteAllProperties((GameManager)this);
            return outmsg;

        }
    }
}
