using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParkourGame;

public class Box : Platform
{
    public Box(Texture2D texture, Vector2 position, float scale = 1.0f) 
        : base(texture, position, scale)
    {
    }

    public override Rectangle BoundingBox
    {
        get
        {
            // Kutunun collision box'ı sadece üst 1/4 kısmı - kenarlardan geçişi sağla
            return new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                _texture.Width,
                _texture.Height / 4
            );
        }
    }
}

