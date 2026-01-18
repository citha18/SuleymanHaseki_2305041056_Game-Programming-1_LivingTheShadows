using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParkourGame;

public class Mountain : Platform
{
    public Mountain(Texture2D texture, Vector2 position) 
        : base(texture, position, 1.0f)
    {
    }

    public override Rectangle BoundingBox
    {
        get
        {
            // Dağın collision box'ı ÜST 1/3 kısmı - oyuncu buraya üstten çarpışabilir
            return new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                _texture.Width,
                _texture.Height / 3
            );
        }
    }
}

