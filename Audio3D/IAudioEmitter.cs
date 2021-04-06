using Microsoft.Xna.Framework;

namespace Audio3D
{
  /// <summary>
  /// 3D サウンドを再生するエンティティの位置と速度を検索するために AudioManager が使用するインターフェイス。
  /// </summary>
  public interface IAudioEmitter
  {
    Vector3 Position { get; }
    Vector3 Forward { get; }
    Vector3 Up { get; }
    Vector3 Velocity { get; }
  }
}
