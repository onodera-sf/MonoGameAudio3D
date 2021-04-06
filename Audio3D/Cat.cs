using System;
using Microsoft.Xna.Framework;

namespace Audio3D
{
  /// <summary>
  /// 円を描いて移動し、猫の鳴き声を鳴らすエンティティクラス。
  /// これは単発音を使用し、再生が終了すると自動的に停止します。
  /// ループ音の使用例については、Dog クラスを参照してください。
  /// </summary>
  class Cat : SpriteEntity
  {
    #region Fields

    /// <summary>次の音を鳴らすまでの時間。</summary>
    TimeSpan _timeDelay = TimeSpan.Zero;

    /// <summary>サウンドバリエーションから選択するための乱数ジェネレータ。</summary>
    static readonly Random _random = new Random();

    #endregion


    /// <summary>
    /// 猫の位置を更新し、音を鳴らします。
    /// </summary>
    public override void Update(GameTime gameTime, AudioManager audioManager)
    {
      // 猫を大きな円で動かします。
      double time = gameTime.TotalGameTime.TotalSeconds;

      float dx = (float)-Math.Cos(time);
      float dz = (float)-Math.Sin(time);

      Vector3 newPosition = new Vector3(dx, 0, dz) * 6000;

      // エンティティの位置と速度を更新します。
      Velocity = newPosition - Position;
      Position = newPosition;
      if (Velocity == Vector3.Zero)
      {
        Forward = Vector3.Forward;
      }
      else
      {
        Forward = Vector3.Normalize(Velocity);
      }

      Up = Vector3.Up;

      // 時間遅延がなくなった場合は、別の単発音をトリガーします。
      _timeDelay -= gameTime.ElapsedGameTime;

      if (_timeDelay < TimeSpan.Zero)
      {
        // 異なる3つのサウンドバリエーション（CatSound0、CatSound1、CatSound2）からランダムに選択します。
        string soundName = "CatSound" + _random.Next(3);

        audioManager.Play3DSound(soundName, false, this);

        _timeDelay += TimeSpan.FromSeconds(1.25f);
      }
    }
  }
}
