using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParkourGame;

public abstract class Sprite
{
    protected Texture2D _texture;
    protected Vector2 _position;

    public Vector2 Position => _position;
    public Texture2D Texture => _texture;

    public virtual Rectangle BoundingBox => new Rectangle(
        (int)_position.X,
        (int)_position.Y,
        _texture.Width,
        _texture.Height
    );

    protected Sprite(Texture2D texture, Vector2 position)
    {
        _texture = texture;
        _position = position;
    }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, Color.White);
    }
}
