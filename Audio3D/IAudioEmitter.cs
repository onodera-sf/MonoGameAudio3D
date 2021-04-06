using Microsoft.Xna.Framework;

namespace Audio3D
{
  /// <summary>
  /// 3D �T�E���h���Đ�����G���e�B�e�B�̈ʒu�Ƒ��x���������邽�߂� AudioManager ���g�p����C���^�[�t�F�C�X�B
  /// </summary>
  public interface IAudioEmitter
  {
    Vector3 Position { get; }
    Vector3 Forward { get; }
    Vector3 Up { get; }
    Vector3 Velocity { get; }
  }
}
