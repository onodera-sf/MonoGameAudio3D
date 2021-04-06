using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Audio3D
{
  public class Audio3DGame : Game
  {
    #region フィールド

    readonly GraphicsDeviceManager _graphics;
    readonly AudioManager _audioManager;
    readonly SpriteEntity _cat;
    readonly SpriteEntity _dog;

    /// <summary>地面の描画に使用するテクスチャーです。</summary>
    Texture2D _checkerTexture;

    QuadDrawer _quadDrawer;

    Vector3 _cameraPosition = new Vector3(0, 512, 0);
    Vector3 _cameraForward = Vector3.Forward;
    Vector3 _cameraUp = Vector3.Up;
    Vector3 _cameraVelocity = Vector3.Zero;

    KeyboardState _currentKeyboardState;
    GamePadState _currentGamePadState;

    #endregion --フィールド


    public Audio3DGame()
    {
      Content.RootDirectory = "Content";

      _graphics = new GraphicsDeviceManager(this);

      _audioManager = new AudioManager(this);

      // AudioManager を Components に追加して自動的に Update メソッドが呼ばれるようにします。
      Components.Add(_audioManager);

      _cat = new Cat();
      _dog = new Dog();
    }

    /// <summary>
    /// グラフィックコンテンツをロードします。
    /// </summary>
    protected override void LoadContent()
    {
      _cat.Texture = Content.Load<Texture2D>("CatTexture");
      _dog.Texture = Content.Load<Texture2D>("DogTexture");

      _checkerTexture = Content.Load<Texture2D>("checker");

      // 四角形ポリゴンを描画するためのクラス
      _quadDrawer = new QuadDrawer(_graphics.GraphicsDevice);
    }

    /// <summary>
    /// ゲームがロジックを実行できるようにします。
    /// </summary>
    protected override void Update(GameTime gameTime)
    {
      HandleInput();

      UpdateCamera();

      // 新しいカメラの位置について AudioManager に伝えます。
      _audioManager.Listener.Position = _cameraPosition;
      _audioManager.Listener.Forward = _cameraForward;
      _audioManager.Listener.Up = _cameraUp;
      _audioManager.Listener.Velocity = _cameraVelocity;

      // ゲームエンティティに動き回って音を鳴らすように伝えます。
      _cat.Update(gameTime, _audioManager);
      _dog.Update(gameTime, _audioManager);

      base.Update(gameTime);
    }


    /// <summary>
    /// ゲームが描画する必要があるときに呼び出されます。
    /// </summary>
    protected override void Draw(GameTime gameTime)
    {
      var device = _graphics.GraphicsDevice;

      device.Clear(Color.CornflowerBlue);

      device.BlendState = BlendState.AlphaBlend;

      // カメラ行列を計算します。
      var view = Matrix.CreateLookAt(_cameraPosition, _cameraPosition + _cameraForward, _cameraUp);
      var projection = Matrix.CreatePerspectiveFieldOfView(1, device.Viewport.AspectRatio, 1, 100000);

      // チェッカーグラウンドポリゴンを描画します。
      var groundTransform = Matrix.CreateScale(20000) * Matrix.CreateRotationX(MathHelper.PiOver2);

      _quadDrawer.DrawQuad(_checkerTexture, 32, groundTransform, view, projection);

      // ゲームエンティティを描画します。
      _cat.Draw(_quadDrawer, _cameraPosition, view, projection);
      _dog.Draw(_quadDrawer, _cameraPosition, view, projection);

      base.Draw(gameTime);
    }

    /// <summary>
    /// ゲームを終了するための入力を処理します。
    /// </summary>
    void HandleInput()
    {
      _currentKeyboardState = Keyboard.GetState();
      _currentGamePadState = GamePad.GetState(PlayerIndex.One);

      // 終了を確認します。
      if (_currentKeyboardState.IsKeyDown(Keys.Escape) ||
          _currentGamePadState.Buttons.Back == ButtonState.Pressed)
      {
        Exit();
      }
    }

    /// <summary>
    /// カメラを動かすための入力を処理します。
    /// </summary>
    void UpdateCamera()
    {
      const float turnSpeed = 0.05f;
      const float accelerationSpeed = 4;
      const float frictionAmount = 0.98f;

      // 左または右に曲がります。
      float turn = -_currentGamePadState.ThumbSticks.Left.X * turnSpeed;

      if (_currentKeyboardState.IsKeyDown(Keys.Left)) turn += turnSpeed;
      if (_currentKeyboardState.IsKeyDown(Keys.Right)) turn -= turnSpeed;

      _cameraForward = Vector3.TransformNormal(_cameraForward, Matrix.CreateRotationY(turn));

      // 前方または後方に加速します。
      float accel = _currentGamePadState.ThumbSticks.Left.Y * accelerationSpeed;

      if (_currentKeyboardState.IsKeyDown(Keys.Up)) accel += accelerationSpeed;
      if (_currentKeyboardState.IsKeyDown(Keys.Down)) accel -= accelerationSpeed;

      _cameraVelocity += _cameraForward * accel;

      // 現在の位置に速度を追加します。
      _cameraPosition += _cameraVelocity;

      // 摩擦力を加えます。
      _cameraVelocity *= frictionAmount;
    }
  }
}
