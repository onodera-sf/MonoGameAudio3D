using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Audio3D
{
  /// <summary>
  /// 3D四辺形を描画するためのヘルパー。 これは、猫と犬のビルボードスプライト、および市松模様の地面のポリゴンを描画するために使用されます。
  /// </summary>
  class QuadDrawer
  {
		#region フィールド

		readonly GraphicsDevice _graphicsDevice;
		readonly AlphaTestEffect _effect;
		readonly VertexPositionTexture[] _vertices;

    #endregion


    /// <summary>
    /// 新しい四辺形の描画ワーカーを作成します。
    /// </summary>
    public QuadDrawer(GraphicsDevice device)
    {
      _graphicsDevice = device;

      _effect = new AlphaTestEffect(device);

      _effect.AlphaFunction = CompareFunction.Greater;
      _effect.ReferenceAlpha = 128;

      // 4つの頂点の配列を事前に割り当てます。
      _vertices = new VertexPositionTexture[4];

      _vertices[0].Position = new Vector3(1, 1, 0);
      _vertices[1].Position = new Vector3(-1, 1, 0);
      _vertices[2].Position = new Vector3(1, -1, 0);
      _vertices[3].Position = new Vector3(-1, -1, 0);
    }


    /// <summary>
    /// 3Dワールドの一部として四角形を描画します。
    /// </summary>
    public void DrawQuad(Texture2D texture, float textureRepeats, Matrix world, Matrix view, Matrix projection)
    {
      // 指定されたテクスチャとカメラマトリックスを使用するようにエフェクトを設定します。
      _effect.Texture = texture;

      _effect.World = world;
      _effect.View = view;
      _effect.Projection = projection;

      // 指定された数のテクスチャの繰り返しを使用するように頂点配列を更新します。
      _vertices[0].TextureCoordinate = new Vector2(0, 0);
      _vertices[1].TextureCoordinate = new Vector2(textureRepeats, 0);
      _vertices[2].TextureCoordinate = new Vector2(0, textureRepeats);
      _vertices[3].TextureCoordinate = new Vector2(textureRepeats, textureRepeats);

      // 矩形を描画します。
      _effect.CurrentTechnique.Passes[0].Apply();

      _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _vertices, 0, 2);
    }
  }
}
