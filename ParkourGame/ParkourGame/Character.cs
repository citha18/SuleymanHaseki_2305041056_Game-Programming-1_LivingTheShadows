using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ParkourGame;

public class Character : MovingSprites
{
    private Vector2 _startPosition;

    public Character(Texture2D texture, int screenWidth, int screenHeight, Vector2? startPosition = null)
        : base(texture, screenWidth, screenHeight, startPosition)
    {
        _startPosition = Vector2.Zero;
    }

    public void SetStartPosition(Vector2 startPosition)
    {
        _startPosition = startPosition;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public void UpdateCollisions(List<Platform> platforms)
    {
        CheckCollisions(platforms);
    }

    public void Reset()
    {
        _position = _startPosition;
        _velocity = Vector2.Zero;
        _isGrounded = true;
        _canJump = true;
    }
}
