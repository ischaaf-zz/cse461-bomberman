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
        public Dictionary<string, Texture2D> textures;

        public GraphicalGameManager(int players, Dictionary<string, Texture2D> textureMappings) : base(players)
        {
            textures = textureMappings;
        }

        public override Wall CreateBackground()
        {
            return new DrawableWall(this, textures["background"], new Rectangle(0, 0, BOX_WIDTH * GAME_SIZE, BOX_WIDTH * GAME_SIZE), null);
        }

        public override Wall CreateWall(int x, int y, int dim, Rectangle? textureRect)
        {
            return new DrawableWall(this, textures["wall"], new Rectangle(x, y, dim, dim), textureRect);
        }

        public override Player CreatePlayer(Rectangle pos, Color? c)
        {
            return new DrawablePlayer(this, pos, textures["player"], c.Value);
        }

        public override Box CreateBox(int x, int y, PowerUp p)
        {
            return new DrawableBox(this, x, y, p, textures["box"]);
        }

        public override PowerUp CreatePowerUp(PowerUp.PowerUpType type, int x, int y)
        {
            return new DrawablePowerUp(this, type, x, y, textures["powerups"]);
        }

        public override Explosion CreateExplosion(int x, int y, int dim, TimeSpan startedAt)
        {
            return new DrawableExplosion(this, x, y, dim, startedAt, textures["explosion"]);
        }

        public override Bomb CreateBomb(int x, int y, TimeSpan placed, int ttd, Player placedBy, int dim, bool super)
        {
            return new DrawableBomb(this, x, y, placed, ttd, placedBy, dim, textures["bomb"], super);
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            (background as Drawable.IDrawable).Draw(spritebatch, gameTime);

            foreach (var item in statics)
            {
                (item as Drawable.IDrawable).Draw(spritebatch, gameTime);
            }
            foreach (var item in powerUps)
            {
                (item as Drawable.IDrawable).Draw(spritebatch, gameTime);
            }
            foreach (var item in bombs)
            {
                (item as Drawable.IDrawable).Draw(spritebatch, gameTime);
            }
            foreach (var item in explosions)
            {
                (item as Drawable.IDrawable).Draw(spritebatch, gameTime);
            }
            foreach (var p in players)
            {
                (p as Drawable.IDrawable)?.Draw(spritebatch, gameTime);
            }
        }

        public override void UpdateBomb(GameTime gametime, Bomb b)
        {
            base.UpdateBomb(gametime, b);
        }

        public override bool PlaceExplosion(int x, int y, GameTime gametime, bool super)
        {
            Point p = new Point(x, y);
            if (bombs.IsItemAtPoint(p))
            {
                Bomb b = bombs.GetAtPoint(p) as Bomb;
                ExplodeBomb(gametime, b);
            } else if (statics.IsItemAtPoint(p))
            {
                var item = statics.GetAtPoint(p);
                if (item is Box)
                {
                    // blow the box up
                    explosions.Add(new DrawableExplosion(this, x, y, BOX_WIDTH, gametime.TotalGameTime, textures["explosion"]));
                    return super;
                }
                return false;
            }
            explosions.Add(new DrawableExplosion(this, x, y, BOX_WIDTH, gametime.TotalGameTime, textures["explosion"]));
            return true;
        }
    }
}
