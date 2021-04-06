using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Audio3D
{
  /// <summary>
  /// AudioManager は、再生中の3Dサウンドを追跡し、カメラとエンティティがワールドを移動するときに設定を更新し、
  /// 再生が終了するとサウンドエフェクトインスタンスを自動的に破棄します。
  /// </summary>
  public class AudioManager : GameComponent
  {
    #region フィールド

    // このマネージャーにロードされるすべてのサウンドエフェクトのリスト。
    static string[] _soundNames =
      {
        "CatSound0",
        "CatSound1",
        "CatSound2",
        "DogSound",
      };

    /// <summary>音を聞くリスナーの情報です。これは通常カメラに一致するように設定されます。</summary>
    public AudioListener Listener { get; } = new AudioListener();

    /// <summary>AudioEmitter は、3Dサウンドを生成しているエンティティを表します。</summary>
    readonly AudioEmitter _emitter = new AudioEmitter();

    /// <summary>再生可能なすべての効果音を保存します。</summary>
    readonly Dictionary<string, SoundEffect> _soundEffects = new Dictionary<string, SoundEffect>();

    /// <summary>現在再生中のすべての3Dサウンドを追跡します。また、再生が終わったインスタンスの破棄にも使用します。</summary>
    readonly List<ActiveSound> _activeSounds = new List<ActiveSound>();

    #endregion --フィールド


    public AudioManager(Game game) : base(game) { }

    /// <summary>
    /// オーディオマネージャを初期化します。
    /// </summary>
    public override void Initialize()
    {
      // ゲームの世界のスケールと一致するように、3Dオーディオのスケールを設定します。
      // DistanceScale は、離れるにつれて音量が変化する音の量を制御します。
      // DopplerScale は、サウンドを通過するときにピッチが変化するサウンドの量を制御します。
      SoundEffect.DistanceScale = 2000;
      SoundEffect.DopplerScale = 0.1f;

      // すべての効果音をロードします。
      foreach (string soundName in _soundNames)
      {
        _soundEffects.Add(soundName, Game.Content.Load<SoundEffect>(soundName));
      }

      base.Initialize();
    }


    /// <summary>
    /// 効果音データをアンロードします。
    /// GameComponent として登録すればゲーム終了時に自動的に呼ばれます。
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
    /// 3Dオーディオシステムの状態を更新します。
    /// </summary>
    public override void Update(GameTime gameTime)
    {
      // 現在再生中のすべての3Dサウンドをループします。
      int index = 0;

      while (index < _activeSounds.Count)
      {
        ActiveSound activeSound = _activeSounds[index];

        if (activeSound.Instance.State == SoundState.Stopped)
        {
          // 音の再生が終了した場合は廃棄してください。
          activeSound.Instance.Dispose();

          // アクティブリストから削除します。
          _activeSounds.RemoveAt(index);
        }
        else
        {
          // サウンドがまだ再生されている場合は、3D設定を更新します。
          Apply3D(activeSound);

          index++;
        }
      }

      base.Update(gameTime);
    }


    /// <summary>
    /// 新しい3Dサウンドを設定し再生します。
    /// </summary>
    public SoundEffectInstance Play3DSound(string soundName, bool isLooped, IAudioEmitter emitter)
    {
      ActiveSound activeSound = new ActiveSound();

      // インスタンスを生成し、インスタンス、エミッターを設定します。
      activeSound.Instance = _soundEffects[soundName].CreateInstance();
      activeSound.Instance.IsLooped = isLooped;

      activeSound.Emitter = emitter;

      // 3D 位置を設定します。
      Apply3D(activeSound);

      activeSound.Instance.Play();

      // このサウンドがアクティブであることを保存します。
      _activeSounds.Add(activeSound);

      return activeSound.Instance;
    }


    /// <summary>
    /// 3Dサウンドの位置と速度の設定を更新します。
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
    /// アクティブな3Dサウンドを追跡し、アタッチされているエミッターオブジェクトを記憶するための内部ヘルパークラス。
    /// </summary>
    private class ActiveSound
    {
      public SoundEffectInstance Instance;
      public IAudioEmitter Emitter;
    }
  }
}
