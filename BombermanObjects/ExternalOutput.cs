using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace BombermanObjects
{
    public class ExternalOutput
    {
        public NetClient client;
        public int playerID;

        public ExternalOutput(NetClient client, int playerID)
        {
            this.client = client;
            this.playerID = playerID;
        }

        public void sendMove(Logical.Player.Direction dir, int dist)
        {
            NetOutgoingMessage outmsg = client.CreateMessage();
            outmsg.WriteVariableInt32(playerID);
            outmsg.Write((byte)PacketTypeEnums.PacketType.EVENT);
            outmsg.Write((byte)PacketTypeEnums.EventType.EVENT_MOVE);
            outmsg.WriteVariableInt32((int)dir);
            outmsg.WriteVariableInt32(dist);
            client.SendMessage(outmsg, NetDeliveryMethod.UnreliableSequenced);
        }

        public void sendBombPlacement(GameTime gameTime)
        {
            NetOutgoingMessage outmsg = client.CreateMessage();
            outmsg.WriteVariableInt32(playerID);
            outmsg.Write((byte)PacketTypeEnums.PacketType.EVENT);
            outmsg.Write((byte)PacketTypeEnums.EventType.EVENT_BOMB_PLACEMENT);
            outmsg.WriteAllProperties(gameTime);
            client.SendMessage(outmsg, NetDeliveryMethod.UnreliableSequenced);
        }
    }
}
