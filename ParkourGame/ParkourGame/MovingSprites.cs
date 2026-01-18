using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ParkourGame;

public class MovingSprites : Sprite
{
    // Konum ve Hareket
    protected Vector2 _velocity;
    protected float _speed = 200f;
    protected float _jumpPower = 480f;
    protected float _gravity = 1000f;

    // Durum
    protected bool _isGrounded;
    protected bool _canJump = true;
    
    // Ekran Sınırları
    protected int _screenWidth;
    protected int _screenHeight;
    protected float _groundLevel;

    public bool IsGrounded => _isGrounded;
    public Vector2 Velocity => _velocity;

    public MovingSprites(Texture2D texture, int screenWidth, int screenHeight, Vector2? startPosition = null) 
        : base(texture, startPosition ?? Vector2.Zero)
    {
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
        _groundLevel = screenHeight - texture.Height - 50;

        _position = startPosition ?? new Vector2(screenWidth / 2f, _groundLevel);
        _velocity = Vector2.Zero;
        _isGrounded = true;
    }

    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        HandleInput();
        ApplyGravity(deltaTime);
        UpdatePosition(deltaTime);
        
        // ÖNEMLİ: Update içinde CheckCollisions çağrılmıyorsa
        // Game1.cs içinde Update'den hemen sonra çağrılmalıdır.
        // Eğer Game1 içinde çağırıyorsan buraya eklemene gerek yok.
        // Ancak logic bütünlüğü için zemin kontrolünü buraya alıyoruz:
        CheckGlobalGround(); 
    }

    protected virtual void HandleInput()
    {
        KeyboardState keyboardState = Keyboard.GetState();
        
        // Yatay Hız Sıfırlama (Tuşa basılmazsa dursun)
        _velocity.X = 0;

        if (keyboardState.IsKeyDown(Keys.A))
        {
            _velocity.X = -_speed;
        }
        else if (keyboardState.IsKeyDown(Keys.D))
        {
            _velocity.X = _speed;
        }

        // Zıplama
        if (keyboardState.IsKeyDown(Keys.W) && _isGrounded)
        {
            _velocity.Y = -_jumpPower;
            _isGrounded = false;
        }
    }

    private void ApplyGravity(float deltaTime)
    {
        // Yerçekimini her zaman uygula. Eğer yerdeysek Collision bunu sıfırlayacak.
        // Bu sayede "Grounded miyim?" kararsızlığından kurtuluruz.
        _velocity.Y += _gravity * deltaTime;
    }

    private void UpdatePosition(float deltaTime)
    {
        _position += _velocity * deltaTime;

        // Ekranın sağına soluna gitmeyi engelle
        if (_position.X < 0) _position.X = 0;
        if (_position.X + _texture.Width > _screenWidth) _position.X = _screenWidth - _texture.Width;
    }

    // Bu metodu Game1.cs içerisindeki Update döngüsünde çağırmalısın:
    // sprite.Update(gameTime);
    // sprite.CheckCollisions(platforms);
    public void CheckCollisions(List<Platform> platforms)
    {
        // Varsayılan olarak havada olduğumuzu varsayalım
        // Sadece bir şeye çarparsak true yapacağız.
        bool onSomething = false;

        foreach (var platform in platforms)
        {
            if (BoundingBox.Intersects(platform.BoundingBox))
            {
                Rectangle overlap = Rectangle.Intersect(BoundingBox, platform.BoundingBox);

                // Çarpışma var mı?
                if (overlap.Width > 0 && overlap.Height > 0)
                {
                    // Sadece ÜSTTEN çarpışmaları algıla
                    // Karakterin ayakları, platformun merkezinden yukarıdaysa
                    // Ve karakter aşağı doğru düşüyorsa (_velocity.Y > 0)
                    bool isFalling = _velocity.Y > 0;
                    bool isAbovePlatform = (_position.Y + _texture.Height) - _velocity.Y < platform.Position.Y + (platform.BoundingBox.Height / 2);

                    if (isFalling)
                    {
                        // Düzeltme yap (Snap)
                        _position.Y = platform.Position.Y - _texture.Height;
                        _velocity.Y = 0; // Düşmeyi durdur
                        _isGrounded = true;
                        onSomething = true;
                    }
                }
            }
        }
        
        // Platformlara basmıyorsak ve Global zemin kontrolü (ekran altı)
        // CheckGlobalGround() metodunu Update'de çağırdığımız için burada isGrounded'ı
        // hemen false yapmamalıyız, yoksa zeminle çakışır.
        
        // Eğer platforma basmıyorsak, yer durumu false olabilir
        // (Ama CheckGlobalGround bunu tekrar true yapabilir, o yüzden sorun yok)
        if (!onSomething)
        {
            // Eğer global zeminde değilsek havada demektir.
            if (_position.Y < _groundLevel)
            {
                _isGrounded = false;
            }
        }
    }

    private void CheckGlobalGround()
    {
        if (_position.Y >= _groundLevel)
        {
            _position.Y = _groundLevel;
            if (_velocity.Y > 0) 
            {
                _velocity.Y = 0;
            }
            _isGrounded = true;
        }
    }
}