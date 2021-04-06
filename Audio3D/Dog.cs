using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Audio3D
{
  /// <summary>
  /// ��ӏ��ɍ����Č��̖�����炷�G���e�B�e�B�N���X�B 
  /// ����̓��[�v�����g�p���܂����A���ꂪ�i���ɑ����̂�h�����߂ɖ����I�ɒ�~����K�v������܂��B
  /// �P�����̎g�p��ɂ��ẮACat�N���X���Q�Ƃ��Ă��������B
  /// </summary>
  class Dog : SpriteEntity
  {
    #region �t�B�[���h

    /// <summary>�T�E���h���J�n�܂��͒�~����܂ł̎��ԁB</summary>
    TimeSpan _timeDelay = TimeSpan.Zero;

    /// <summary>���ݍĐ����̃T�E���h�i����ꍇ�j�B</summary>
    SoundEffectInstance _activeSound = null;

    #endregion --�t�B�[���h


    /// <summary>
    /// ���̈ʒu���X�V���A����炵�܂��B
    /// </summary>
    public override void Update(GameTime gameTime, AudioManager audioManager)
    {
      // �G���e�B�e�B���Œ�ʒu�ɐݒ肵�܂��B
      Position = new Vector3(0, 0, -4000);
      Forward = Vector3.Forward;
      Up = Vector3.Up;
      Velocity = Vector3.Zero;

      // ���Ԓx�����Ȃ��Ȃ����ꍇ�́A���[�v�����J�n�܂��͒�~���܂��B ����͒ʏ�͉i�v�ɑ����܂����A6�b�̒x����ɒ�~���A�����4�b��ɍċN�����܂��B
      _timeDelay -= gameTime.ElapsedGameTime;

      if (_timeDelay < TimeSpan.Zero)
      {
        if (_activeSound == null)
        {
          // ���ݍĐ����̃T�E���h���Ȃ��ꍇ�́A�g���K�[���܂��B
          _activeSound = audioManager.Play3DSound("DogSound", true, this);

          _timeDelay += TimeSpan.FromSeconds(6);
        }
        else
        {
          // ����ȊO�̏ꍇ�́A���݂̃T�E���h���~���܂��B
          _activeSound.Stop(false);
          _activeSound = null;

          _timeDelay += TimeSpan.FromSeconds(4);
        }
      }
    }
  }
}
