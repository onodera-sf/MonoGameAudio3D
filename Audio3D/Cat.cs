using System;
using Microsoft.Xna.Framework;

namespace Audio3D
{
  /// <summary>
  /// �~��`���Ĉړ����A�L�̖�����炷�G���e�B�e�B�N���X�B
  /// ����͒P�������g�p���A�Đ����I������Ǝ����I�ɒ�~���܂��B
  /// ���[�v���̎g�p��ɂ��ẮADog �N���X���Q�Ƃ��Ă��������B
  /// </summary>
  class Cat : SpriteEntity
  {
    #region Fields

    /// <summary>���̉���炷�܂ł̎��ԁB</summary>
    TimeSpan _timeDelay = TimeSpan.Zero;

    /// <summary>�T�E���h�o���G�[�V��������I�����邽�߂̗����W�F�l���[�^�B</summary>
    static readonly Random _random = new Random();

    #endregion


    /// <summary>
    /// �L�̈ʒu���X�V���A����炵�܂��B
    /// </summary>
    public override void Update(GameTime gameTime, AudioManager audioManager)
    {
      // �L��傫�ȉ~�œ������܂��B
      double time = gameTime.TotalGameTime.TotalSeconds;

      float dx = (float)-Math.Cos(time);
      float dz = (float)-Math.Sin(time);

      Vector3 newPosition = new Vector3(dx, 0, dz) * 6000;

      // �G���e�B�e�B�̈ʒu�Ƒ��x���X�V���܂��B
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

      // ���Ԓx�����Ȃ��Ȃ����ꍇ�́A�ʂ̒P�������g���K�[���܂��B
      _timeDelay -= gameTime.ElapsedGameTime;

      if (_timeDelay < TimeSpan.Zero)
      {
        // �قȂ�3�̃T�E���h�o���G�[�V�����iCatSound0�ACatSound1�ACatSound2�j���烉���_���ɑI�����܂��B
        string soundName = "CatSound" + _random.Next(3);

        audioManager.Play3DSound(soundName, false, this);

        _timeDelay += TimeSpan.FromSeconds(1.25f);
      }
    }
  }
}
