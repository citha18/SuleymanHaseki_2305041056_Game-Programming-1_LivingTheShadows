using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ParkourGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Character _character;
    private List<Platform> _platforms;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _platforms = new List<Platform>();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Character Texture'ı Yükle
        Texture2D characterTexture = Content.Load<Texture2D>("character");
        
        // Platform Texture'larını Yükle
        Texture2D dag2Texture = Content.Load<Texture2D>("dag2");
        Texture2D kutuTexture = Content.Load<Texture2D>("kutu");
        Texture2D kaya3Texture = Content.Load<Texture2D>("kaya3");
        Texture2D kaya4Texture = Content.Load<Texture2D>("kaya4");
        
        int screenWidth = GraphicsDevice.Viewport.Width;
        int screenHeight = GraphicsDevice.Viewport.Height;
        
        // Karakteri sol dağın üstünde başlat
        Vector2 characterStartPos = new Vector2(20, screenHeight - dag2Texture.Height - characterTexture.Height);
        _character = new Character(characterTexture, screenWidth, screenHeight, characterStartPos);
        _character.SetStartPosition(characterStartPos);

        // Sol başlangıç platformu (dag2)
        _platforms.Add(new Mountain(dag2Texture, new Vector2(-50, screenHeight - dag2Texture.Height)));

        // Sağ hedef platformu (dag2)
        _platforms.Add(new Mountain(dag2Texture, new Vector2(screenWidth - dag2Texture.Width + 100, screenHeight - dag2Texture.Height)));

        // Ortaya parkur platformları - sadece kutu, değişen yükseklikler
        _platforms.Add(new Box(kutuTexture, new Vector2(160, screenHeight - 280)));
        _platforms.Add(new Box(kutuTexture, new Vector2(290, screenHeight - 240)));
        _platforms.Add(new Box(kutuTexture, new Vector2(390, screenHeight - 350)));
        _platforms.Add(new Box(kutuTexture, new Vector2(590, screenHeight - 280)));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // 1. Önce karakteri hareket ettir (Gravity uygulanır, pozisyon değişir)
        _character.Update(gameTime);

        // 2. Sonra çarpışmaları kontrol et (Pozisyon hatalıysa düzeltilir ve Y hızı sıfırlanır)
        _character.UpdateCollisions(_platforms);

        // Karakter ekranın altına düştüyse reset et
        if (_character.Position.Y > GraphicsDevice.Viewport.Height - 100)
        {
            _character.Reset();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gray);

        _spriteBatch.Begin();
        
        // Platformları çiz
        foreach (var platform in _platforms)
        {
            platform.Draw(_spriteBatch);
        }
        
        // Character'ı çiz
        _character.Draw(_spriteBatch);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
