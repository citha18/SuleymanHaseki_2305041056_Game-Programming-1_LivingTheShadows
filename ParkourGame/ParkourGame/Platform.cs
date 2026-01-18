using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParkourGame;

public class Platform : ResizableSprite
{
    public Platform(Texture2D texture, Vector2 position, float scale = 1.0f) 
        : base(texture, position, scale)
    {
    }
}
