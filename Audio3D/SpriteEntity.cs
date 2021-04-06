using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Audio3D
{
  /// <summary>
  /// �r���{�[�h�X�v���C�g�Ƃ��ĕ\������A3D�T�E���h�𔭂��邱�Ƃ��ł���Q�[���G���e�B�e�B�̊�{�N���X�B
  /// Cat �N���X�� Dog �N���X�͂ǂ�������ꂩ��h�����Ă��܂��B
  /// </summary>
  abstract class SpriteEntity : IAudioEmitter
  {
    #region �v���p�e�B

    /// <summary>�G���e�B�e�B��3D�ʒu���擾�܂��͐ݒ肵�܂��B</summary>
    public Vector3 Position { get; set; }

    /// <summary>�G���e�B�e�B�������Ă���������擾�܂��͐ݒ肵�܂��B</summary>
    public Vector3 Forward { get; set; }

    /// <summary>���̃G���e�B�e�B�̏�������擾�܂��͐ݒ肵�܂��B</summary>
    public Vector3 Up { get; set; }

    /// <summary>���̃G���e�B�e�B�̈ړ����x���擾�܂��͐ݒ肵�܂��B</summary>
    public Vector3 Velocity { get; protected set; }

    /// <summary>���̃G���e�B�e�B�̕\���Ɏg�p�����e�N�X�`�����擾�܂��͐ݒ肵�܂��B</summary>
    public Texture2D Texture { get; set; }


    #endregion


    /// <summary>
    /// �G���e�B�e�B�̈ʒu���X�V���A�T�E���h���Đ��ł���悤�ɂ��܂��B
    /// </summary>
    public abstract void Update(GameTime gameTime, AudioManager audioManager);


    /// <summary>
    /// �G���e�B�e�B���r���{�[�h�X�v���C�g�Ƃ��ĕ`�悵�܂��B
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
