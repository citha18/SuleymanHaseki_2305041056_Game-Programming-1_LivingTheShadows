using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParkourGame;

public class ResizableSprite : Sprite
{
    private float _scale;

    public float Scale
    {
        get => _scale;
        set => _scale = value;
    }

    public int Width => (int)(_texture.Width * _scale);
    public int Height => (int)(_texture.Height * _scale);

    public override Rectangle BoundingBox => new Rectangle(
        (int)_position.X,
        (int)_position.Y,
        Width,
        Height
    );

    public ResizableSprite(Texture2D texture, Vector2 position, float scale = 1.0f) : base(texture, position)
    {
        _scale = scale;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (_scale != 1.0f)
        {
            spriteBatch.Draw(
                _texture,
                _position,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                _scale,
                SpriteEffects.None,
                0f
            );
        }
        else
        {
            base.Draw(spriteBatch);
        }
    }
}
