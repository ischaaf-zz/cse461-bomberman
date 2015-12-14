using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects
{
    public class PacketTypeEnums
    {
        public enum PacketType : byte
        {
            EVENT,
            GAME_STATE,
            GAME_STATE_FULL, 
            SEND_PLAYER_ID,
            NEW_PLAYER_ID, 
            GAME_START,
            PLAYER_DISCONNECTION,
            CLIENT_IS_ALIVE
        };

        public enum EventType : byte
        {
            EVENT_MOVE,
            EVENT_BOMB_PLACEMENT
        };
    }
}
