using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Audio3D
{
  /// <summary>
  /// AudioManager �́A�Đ�����3D�T�E���h��ǐՂ��A�J�����ƃG���e�B�e�B�����[���h���ړ�����Ƃ��ɐݒ���X�V���A
  /// �Đ����I������ƃT�E���h�G�t�F�N�g�C���X�^���X�������I�ɔj�����܂��B
  /// </summary>
  public class AudioManager : GameComponent
  {
    #region �t�B�[���h

    // ���̃}�l�[�W���[�Ƀ��[�h����邷�ׂẴT�E���h�G�t�F�N�g�̃��X�g�B
    static string[] _soundNames =
      {
        "CatSound0",
        "CatSound1",
        "CatSound2",
        "DogSound",
      };

    /// <summary>���𕷂����X�i�[�̏��ł��B����͒ʏ�J�����Ɉ�v����悤�ɐݒ肳��܂��B</summary>
    public AudioListener Listener { get; } = new AudioListener();

    /// <summary>AudioEmitter �́A3D�T�E���h�𐶐����Ă���G���e�B�e�B��\���܂��B</summary>
    readonly AudioEmitter _emitter = new AudioEmitter();

    /// <summary>�Đ��\�Ȃ��ׂĂ̌��ʉ���ۑ����܂��B</summary>
    readonly Dictionary<string, SoundEffect> _soundEffects = new Dictionary<string, SoundEffect>();

    /// <summary>���ݍĐ����̂��ׂĂ�3D�T�E���h��ǐՂ��܂��B�܂��A�Đ����I������C���X�^���X�̔j���ɂ��g�p���܂��B</summary>
    readonly List<ActiveSound> _activeSounds = new List<ActiveSound>();

    #endregion --�t�B�[���h


    public AudioManager(Game game) : base(game) { }

    /// <summary>
    /// �I�[�f�B�I�}�l�[�W�������������܂��B
    /// </summary>
    public override void Initialize()
    {
      // �Q�[���̐��E�̃X�P�[���ƈ�v����悤�ɁA3D�I�[�f�B�I�̃X�P�[����ݒ肵�܂��B
      // DistanceScale �́A�����ɂ�ĉ��ʂ��ω����鉹�̗ʂ𐧌䂵�܂��B
      // DopplerScale �́A�T�E���h��ʉ߂���Ƃ��Ƀs�b�`���ω�����T�E���h�̗ʂ𐧌䂵�܂��B
      SoundEffect.DistanceScale = 2000;
      SoundEffect.DopplerScale = 0.1f;

      // ���ׂĂ̌��ʉ������[�h���܂��B
      foreach (string soundName in _soundNames)
      {
        _soundEffects.Add(soundName, Game.Content.Load<SoundEffect>(soundName));
      }

      base.Initialize();
    }


    /// <summary>
    /// ���ʉ��f�[�^���A�����[�h���܂��B
    /// GameComponent �Ƃ��ēo�^����΃Q�[���I�����Ɏ����I�ɌĂ΂�܂��B
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (disposing)
        {
          foreach (SoundEffect soundEffect in _soundEffects.Values)
          {
            soundEffect.Dispose();
          }

          _soundEffects.Clear();
        }
      }
      finally
      {
        base.Dispose(disposing);
      }
    }


    /// <summary>
    /// 3D�I�[�f�B�I�V�X�e���̏�Ԃ��X�V���܂��B
    /// </summary>
    public override void Update(GameTime gameTime)
    {
      // ���ݍĐ����̂��ׂĂ�3D�T�E���h�����[�v���܂��B
      int index = 0;

      while (index < _activeSounds.Count)
      {
        ActiveSound activeSound = _activeSounds[index];

        if (activeSound.Instance.State == SoundState.Stopped)
        {
          // ���̍Đ����I�������ꍇ�͔p�����Ă��������B
          activeSound.Instance.Dispose();

          // �A�N�e�B�u���X�g����폜���܂��B
          _activeSounds.RemoveAt(index);
        }
        else
        {
          // �T�E���h���܂��Đ�����Ă���ꍇ�́A3D�ݒ���X�V���܂��B
          Apply3D(activeSound);

          index++;
        }
      }

      base.Update(gameTime);
    }


    /// <summary>
    /// �V����3D�T�E���h��ݒ肵�Đ����܂��B
    /// </summary>
    public SoundEffectInstance Play3DSound(string soundName, bool isLooped, IAudioEmitter emitter)
    {
      ActiveSound activeSound = new ActiveSound();

      // �C���X�^���X�𐶐����A�C���X�^���X�A�G�~�b�^�[��ݒ肵�܂��B
      activeSound.Instance = _soundEffects[soundName].CreateInstance();
      activeSound.Instance.IsLooped = isLooped;

      activeSound.Emitter = emitter;

      // 3D �ʒu��ݒ肵�܂��B
      Apply3D(activeSound);

      activeSound.Instance.Play();

      // ���̃T�E���h���A�N�e�B�u�ł��邱�Ƃ�ۑ����܂��B
      _activeSounds.Add(activeSound);

      return activeSound.Instance;
    }


    /// <summary>
    /// 3D�T�E���h�̈ʒu�Ƒ��x�̐ݒ���X�V���܂��B
    /// </summary>
    private void Apply3D(ActiveSound activeSound)
    {
      _emitter.Position = activeSound.Emitter.Position;
      _emitter.Forward = activeSound.Emitter.Forward;
      _emitter.Up = activeSound.Emitter.Up;
      _emitter.Velocity = activeSound.Emitter.Velocity;

      activeSound.Instance.Apply3D(Listener, _emitter);
    }


    /// <summary>
    /// �A�N�e�B�u��3D�T�E���h��ǐՂ��A�A�^�b�`����Ă���G�~�b�^�[�I�u�W�F�N�g���L�����邽�߂̓����w���p�[�N���X�B
    /// </summary>
    private class ActiveSound
    {
      public SoundEffectInstance Instance;
      public IAudioEmitter Emitter;
    }
  }
}
