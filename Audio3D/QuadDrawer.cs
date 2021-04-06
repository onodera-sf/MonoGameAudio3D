using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Audio3D
{
  /// <summary>
  /// 3D�l�ӌ`��`�悷�邽�߂̃w���p�[�B ����́A�L�ƌ��̃r���{�[�h�X�v���C�g�A����юs���͗l�̒n�ʂ̃|���S����`�悷�邽�߂Ɏg�p����܂��B
  /// </summary>
  class QuadDrawer
  {
		#region �t�B�[���h

		readonly GraphicsDevice _graphicsDevice;
		readonly AlphaTestEffect _effect;
		readonly VertexPositionTexture[] _vertices;

    #endregion


    /// <summary>
    /// �V�����l�ӌ`�̕`�惏�[�J�[���쐬���܂��B
    /// </summary>
    public QuadDrawer(GraphicsDevice device)
    {
      _graphicsDevice = device;

      _effect = new AlphaTestEffect(device);

      _effect.AlphaFunction = CompareFunction.Greater;
      _effect.ReferenceAlpha = 128;

      // 4�̒��_�̔z������O�Ɋ��蓖�Ă܂��B
      _vertices = new VertexPositionTexture[4];

      _vertices[0].Position = new Vector3(1, 1, 0);
      _vertices[1].Position = new Vector3(-1, 1, 0);
      _vertices[2].Position = new Vector3(1, -1, 0);
      _vertices[3].Position = new Vector3(-1, -1, 0);
    }


    /// <summary>
    /// 3D���[���h�̈ꕔ�Ƃ��Ďl�p�`��`�悵�܂��B
    /// </summary>
    public void DrawQuad(Texture2D texture, float textureRepeats, Matrix world, Matrix view, Matrix projection)
    {
      // �w�肳�ꂽ�e�N�X�`���ƃJ�����}�g���b�N�X���g�p����悤�ɃG�t�F�N�g��ݒ肵�܂��B
      _effect.Texture = texture;

      _effect.World = world;
      _effect.View = view;
      _effect.Projection = projection;

      // �w�肳�ꂽ���̃e�N�X�`���̌J��Ԃ����g�p����悤�ɒ��_�z����X�V���܂��B
      _vertices[0].TextureCoordinate = new Vector2(0, 0);
      _vertices[1].TextureCoordinate = new Vector2(textureRepeats, 0);
      _vertices[2].TextureCoordinate = new Vector2(0, textureRepeats);
      _vertices[3].TextureCoordinate = new Vector2(textureRepeats, textureRepeats);

      // ��`��`�悵�܂��B
      _effect.CurrentTechnique.Passes[0].Apply();

      _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _vertices, 0, 2);
    }
  }
}
