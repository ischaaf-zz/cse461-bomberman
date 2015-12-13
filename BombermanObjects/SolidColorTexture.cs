using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects
{
    // Creates a 1x1 pixel texture of a solid color
    public class SolidColorTexture : Texture2D
    {
        private Color _color;
        // Gets or sets the color used to create the texture
        public Color Color
        {
            get { return _color; }
            set
            {
                if (value != _color)
                {
                    _color = value;
                    SetData(new Color[] { _color });
                }
            }
        }


        public SolidColorTexture(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, 1, 1)
        {
            //default constructor
        }
        public SolidColorTexture(GraphicsDevice graphicsDevice, Color color)
            : base(graphicsDevice, 1, 1)
        {
            Color = color;
        }

    }

}
