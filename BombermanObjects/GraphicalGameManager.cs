using BombermanObjects.Drawable;
using BombermanObjects.Logical;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects
{
    public class GraphicalGameManager : GameManager
    {

        protected Dictionary<string, Texture2D> textures;

        public GraphicalGameManager(int players, Dictionary<string, Texture2D> textureMappings) : base(players)
        {
            textures = textureMappings;
        }

        public override void Initialize()
        {
            IGameObject background = new DrawableWall(textures["background"], new Rectangle(0, 0, BOX_WIDTH * GAME_SIZE, BOX_WIDTH * GAME_SIZE), null);
            statics.Add(background);

            for (int i = 0; i < GAME_SIZE; i++)
            {
                for (int j = 0; j < GAME_SIZE; j++)
                {
                    if (i == 0 || j == 0 || i == GAME_SIZE - 1 || j == GAME_SIZE - 1 || (i % 2 == 0 && j % 2 == 0))
                    {
                        IGameObject wall = new DrawableWall(textures["wall"], new Rectangle(i * BOX_WIDTH, j * BOX_WIDTH, BOX_WIDTH, BOX_WIDTH), null);
                        statics.Add(wall);
                        collider.RegisterStatic(wall);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            var backgrounds = statics.GetAllInRegion(new Rectangle(0, 0, GAME_SIZE * BOX_WIDTH, GAME_SIZE * BOX_WIDTH));
            foreach (var item in backgrounds)
            {
                (item as Drawable.IDrawable).Draw(spritebatch);
            }
            var bombs = dynamics.GetAllInRegion(new Rectangle(0, 0, GAME_SIZE * BOX_WIDTH, GAME_SIZE * BOX_WIDTH));
            foreach (var item in bombs)
            {
                (item as Drawable.IDrawable).Draw(spritebatch);
            }
            foreach (var p in players)
            {
                (p as Drawable.IDrawable).Draw(spritebatch);
            }
        }
    }
}
