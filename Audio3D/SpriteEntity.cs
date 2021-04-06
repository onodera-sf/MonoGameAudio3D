using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Audio3D
{
  /// <summary>
  /// ビルボードスプライトとして表示され、3Dサウンドを発することができるゲームエンティティの基本クラス。
  /// Cat クラスと Dog クラスはどちらもこれから派生しています。
  /// </summary>
  abstract class SpriteEntity : IAudioEmitter
  {
    #region プロパティ

    /// <summary>エンティティの3D位置を取得または設定します。</summary>
    public Vector3 Position { get; set; }

    /// <summary>エンティティが向いている方向を取得または設定します。</summary>
    public Vector3 Forward { get; set; }

    /// <summary>このエンティティの上方向を取得または設定します。</summary>
    public Vector3 Up { get; set; }

    /// <summary>このエンティティの移動速度を取得または設定します。</summary>
    public Vector3 Velocity { get; protected set; }

    /// <summary>このエンティティの表示に使用されるテクスチャを取得または設定します。</summary>
    public Texture2D Texture { get; set; }


    #endregion


    /// <summary>
    /// エンティティの位置を更新し、サウンドを再生できるようにします。
    /// </summary>
    public abstract void Update(GameTime gameTime, AudioManager audioManager);


    /// <summary>
    /// エンティティをビルボードスプライトとして描画します。
    /// </summary>
    public void Draw(QuadDrawer quadDrawer, Vector3 cameraPosition, Matrix view, Matrix projection)
    {
      Matrix world = Matrix.CreateTranslation(0, 1, 0) *
                     Matrix.CreateScale(800) *
                     Matrix.CreateConstrainedBillboard(Position, cameraPosition, Up, null, null);

      quadDrawer.DrawQuad(Texture, 1, world, view, projection);
    }
  }
}
