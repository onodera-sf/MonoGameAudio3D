using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Audio3D
{
  /// <summary>
  /// 一箇所に座って犬の鳴き声を鳴らすエンティティクラス。 
  /// これはループ音を使用しますが、それが永遠に続くのを防ぐために明示的に停止する必要があります。
  /// 単発音の使用例については、Catクラスを参照してください。
  /// </summary>
  class Dog : SpriteEntity
  {
    #region フィールド

    /// <summary>サウンドを開始または停止するまでの時間。</summary>
    TimeSpan _timeDelay = TimeSpan.Zero;

    /// <summary>現在再生中のサウンド（ある場合）。</summary>
    SoundEffectInstance _activeSound = null;

    #endregion --フィールド


    /// <summary>
    /// 犬の位置を更新し、音を鳴らします。
    /// </summary>
    public override void Update(GameTime gameTime, AudioManager audioManager)
    {
      // エンティティを固定位置に設定します。
      Position = new Vector3(0, 0, -4000);
      Forward = Vector3.Forward;
      Up = Vector3.Up;
      Velocity = Vector3.Zero;

      // 時間遅延がなくなった場合は、ループ音を開始または停止します。 これは通常は永久に続きますが、6秒の遅延後に停止し、さらに4秒後に再起動します。
      _timeDelay -= gameTime.ElapsedGameTime;

      if (_timeDelay < TimeSpan.Zero)
      {
        if (_activeSound == null)
        {
          // 現在再生中のサウンドがない場合は、トリガーします。
          _activeSound = audioManager.Play3DSound("DogSound", true, this);

          _timeDelay += TimeSpan.FromSeconds(6);
        }
        else
        {
          // それ以外の場合は、現在のサウンドを停止します。
          _activeSound.Stop(false);
          _activeSound = null;

          _timeDelay += TimeSpan.FromSeconds(4);
        }
      }
    }
  }
}
