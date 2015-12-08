using BombermanObjects.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects
{
    public class GameState
    {
        Player[] players;
        List<Bomb> bombList;
        List<Box> destroyedBoxList;
        List<PowerUp> powerupList;

        public GameState(Player[] players, List<Bomb> bombList, List<Box> destroyedBoxList, List<PowerUp> powerupList)
        {

        }
    }
}
