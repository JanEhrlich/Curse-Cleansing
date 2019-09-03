using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //This class holds a static reference to itself to ensure that there will only be
    //one in existence. This is often referred to as a "singleton" design pattern. Other
    //scripts access this one through its public static methods
    static AudioManager current;

    /*
     * The background ambient sound and the background music of the levels
     */

    [Header("Ambient Audio Prison")]
    public AudioClip ambientClipLevel1;		
    public AudioClip musicClipLevel1;
    public int ambientVolumeLevel1;
    public int musicVolumeLevel1;

    [Header("Ambient Audio Escape")]
    public AudioClip ambientClipLevel2;		
    public AudioClip musicClipLevel2;
    public int ambientVolumeLevel2;
    public int musicVolumeLevel2;

    [Header("Ambient Audio Hideout")]
    public AudioClip ambientClipLevel3;		
    public AudioClip musicClipLevel3;
    public int ambientVolumeLevel3;
    public int musicVolumeLevel3;

    [Header("Ambient Audio Way2Tavern")]
    public AudioClip ambientClipLevel4;		
    public AudioClip musicClipLevel4;
    public int ambientVolumeLevel4;
    public int musicVolumeLevel4;

    [Header("Ambient Audio Tavern")]
    //5_1 is the Sound of the Tavern without a fight
    public AudioClip ambientClipLevel5_1;   
    public AudioClip musicClipLevel5_1;
    public int ambientVolumeLevel5_1;
    public int musicVolumeLevel5_1;

    //5_2 is the Sound of the Tavern with a fight
    public AudioClip ambientClipLevel5_2;   
    public AudioClip musicClipLevel5_2;
    public int ambientVolumeLevel5_2;
    public int musicVolumeLevel5_2;

    [Header("Ambient Audio Harbor")]
    public AudioClip ambientClipLevel6;		
    public AudioClip musicClipLevel6;
    public int ambientVolumeLevel6;
    public int musicVolumeLevel6;

    [Header("Ambient Audio Ship")]
    //7_1 is the Sound of the Ship without a fight
    public AudioClip ambientClipLevel7_1;  
    public AudioClip musicClipLevel7_1;
    public int ambientVolumeLevel7_1;
    public int musicVolumeLevel7_1;

    //7_2 is the Sound of the Ship with a fight
    public AudioClip ambientClipLevel7_2;   
    public AudioClip musicClipLevel7_2;
    public int ambientVolumeLevel7_2;
    public int musicVolumeLevel7_2;

    [Header("Sound Effects")]
    public AudioClip[] krakenSquishesClips;     //The effect is played when the kraken is squishing
    public AudioClip[] moveOnWoodSFXClips;      //The effect is played when there is movement on wood
    public AudioClip[] hitFleshSFXClips;        //The effect is played when sword hits flesh
    public AudioClip levelStartSFXClip;         //The effect is played when each level starts
    public AudioClip deathSFXClip;              //The effect is played when the player dies
    public AudioClip doorOpenSFXClip;           //The effect is played when the door opens
    public AudioClip exhaustedSFXClip;          //The effect is played when someone is exhausted
    public AudioClip grabSFXClip;               //The effect is played when grabbing something
    public AudioClip waterBlastSFXClip;         //The effect is played when someone is using a water blast attack

    [Header("Player Audio")]
    public AudioClip[] walkStepClips;           //The footstep sound effects
    public AudioClip[] swordAttackClips;        //The sword attack sound effects
    public AudioClip deathClip;			        //The player death sound effect
    public AudioClip jumpClip;                  //The player jump sound effect

    public AudioClip attackVoiceClip;           //The player is doing an attack voice
    public AudioClip deathVoiceClip;            //The player death voice
    public AudioClip jumpVoiceClip;             //The player jump voice
    public AudioClip hurtVoiceClip;             //The player gets hurt voice

    [Header("Boss Enemy Sound Effects")]
    public AudioClip[] bossTauntClips;          //The boss taunt sounds
    public AudioClip bossAttack1Clip;           //The effect played when the Boss is using Attack 1
    public AudioClip bossAttack2Clip;           //The effect played when the Boss is using Attack 2
    public AudioClip bossAttack3Clip;           //The effect played when the Boss is using Attack 3

    [Header("Mixer Groups")]
    public AudioMixerGroup ambientGroup;        //The ambient mixer group
    public AudioMixerGroup musicGroup;          //The music mixer group
    public AudioMixerGroup soundEffectsGroup;   //The sound effects mixer group
    public AudioMixerGroup playerGroup;         //The player mixer group
    public AudioMixerGroup voiceGroup;          //The voice mixer group
    public AudioMixerGroup enemyGroup;          //The player mixer group
    public AudioMixerGroup bossGroup;           //The voice mixer group

    AudioSource ambientSource;			        //Reference to the generated ambient Audio Source
    AudioSource musicSource;                    //Reference to the generated music Audio Source
    AudioSource soundEffectsSource;             //Reference to the generated sound effects Audio Source
    AudioSource playerSource;                   //Reference to the generated player Audio Source
    AudioSource voiceSource;                    //Reference to the generated voice Audio Source
    AudioSource enemySource;                    //Reference to the generated basic enemy Audio Source
    AudioSource bossSource;                    //Reference to the generated boss Audio Source

    void Awake()
    {
        //If an AudioManager exists and it is not this...
        if (current != null && current != this)
        {
            //...destroy this. There can be only one AudioManager
            Destroy(gameObject);
            return;
        }

        //This is the current AudioManager and it should persist between scene loads
        current = this;
        DontDestroyOnLoad(gameObject);

        //Generate the Audio Source "channels" for our game's audio
        ambientSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        musicSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        soundEffectsSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        playerSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        voiceSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        enemySource = gameObject.AddComponent<AudioSource>() as AudioSource;
        bossSource = gameObject.AddComponent<AudioSource>() as AudioSource;

        //Assign each audio source to its respective mixer group so that it is
        //routed and controlled by the audio mixer
        ambientSource.outputAudioMixerGroup = ambientGroup;
        musicSource.outputAudioMixerGroup = musicGroup;
        soundEffectsSource.outputAudioMixerGroup = soundEffectsGroup;
        playerSource.outputAudioMixerGroup = playerGroup;
        voiceSource.outputAudioMixerGroup = voiceGroup;
        enemySource.outputAudioMixerGroup = enemyGroup;
        bossSource.outputAudioMixerGroup = bossGroup;
    }

    #region StartPlayLevelXYAudio
    public static void StartLevel1Audio()
    {
        //Modify Mixer to make Volume consistent throughout the levels
        setLevelBackgroundVolume(current.musicVolumeLevel1, current.ambientVolumeLevel1);

        //Set the clip for ambient audio, tell it to loop, and then tell it to play
        current.ambientSource.clip = current.ambientClipLevel1;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        //Set the clip for music audio, tell it to loop, and then tell it to play
        current.musicSource.clip = current.musicClipLevel1;
        current.musicSource.loop = true;
        current.musicSource.Play();

        //Play the audio that repeats whenever the level reloads
        PlaySceneRestartSFXAudio();
    }

    public static void StartLevel2Audio()
    {
        //Modify Mixer to make Volume consistent throughout the levels
        setLevelBackgroundVolume(current.musicVolumeLevel2, current.ambientVolumeLevel2);

        //Set the clip for ambient audio, tell it to loop, and then tell it to play
        current.ambientSource.clip = current.ambientClipLevel2;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        //Set the clip for music audio, tell it to loop, and then tell it to play
        current.musicSource.clip = current.musicClipLevel2;
        current.musicSource.loop = true;
        current.musicSource.Play();

        //Play the audio that repeats whenever the level reloads
        PlaySceneRestartSFXAudio();
    }

    public static void StartLevel3Audio()
    {
        //Modify Mixer to make Volume consistent throughout the levels
        setLevelBackgroundVolume(current.musicVolumeLevel3, current.ambientVolumeLevel3);

        //Set the clip for ambient audio, tell it to loop, and then tell it to play
        current.ambientSource.clip = current.ambientClipLevel3;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        //Set the clip for music audio, tell it to loop, and then tell it to play
        current.musicSource.clip = current.musicClipLevel3;
        current.musicSource.loop = true;
        current.musicSource.Play();

        //Play the audio that repeats whenever the level reloads
        PlaySceneRestartSFXAudio();
    }

    public static void StartLevel4Audio()
    {
        //Modify Mixer to make Volume consistent throughout the levels
        setLevelBackgroundVolume(current.musicVolumeLevel4, current.ambientVolumeLevel4);

        //Set the clip for ambient audio, tell it to loop, and then tell it to play
        current.ambientSource.clip = current.ambientClipLevel4;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        //Set the clip for music audio, tell it to loop, and then tell it to play
        current.musicSource.clip = current.musicClipLevel4;
        current.musicSource.loop = true;
        current.musicSource.Play();

        //Play the audio that repeats whenever the level reloads
        PlaySceneRestartSFXAudio();
    }

    public static void StartLevel5_1Audio()
    {
        //Modify Mixer to make Volume consistent throughout the levels
        setLevelBackgroundVolume(current.musicVolumeLevel5_1, current.ambientVolumeLevel5_1);

        //Set the clip for ambient audio, tell it to loop, and then tell it to play
        current.ambientSource.clip = current.ambientClipLevel5_1;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        //Set the clip for music audio, tell it to loop, and then tell it to play
        current.musicSource.clip = current.musicClipLevel5_1;
        current.musicSource.loop = true;
        current.musicSource.Play();

        //Play the audio that repeats whenever the level reloads
        PlaySceneRestartSFXAudio();
    }

    public static void StartLevel5_2Audio()
    {
        //Modify Mixer to make Volume consistent throughout the levels
        setLevelBackgroundVolume(current.musicVolumeLevel5_2, current.ambientVolumeLevel5_2);

        //Set the clip for ambient audio, tell it to loop, and then tell it to play
        current.ambientSource.clip = current.ambientClipLevel5_2;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        //Set the clip for music audio, tell it to loop, and then tell it to play
        current.musicSource.clip = current.musicClipLevel5_2;
        current.musicSource.loop = true;
        current.musicSource.Play();

        //Play the audio that repeats whenever the level reloads
        PlaySceneRestartSFXAudio();
    }

    public static void StartLevel6Audio()
    {
        //Modify Mixer to make Volume consistent throughout the levels
        setLevelBackgroundVolume(current.musicVolumeLevel6, current.ambientVolumeLevel6);

        //Set the clip for ambient audio, tell it to loop, and then tell it to play
        current.ambientSource.clip = current.ambientClipLevel6;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        //Set the clip for music audio, tell it to loop, and then tell it to play
        current.musicSource.clip = current.musicClipLevel6;
        current.musicSource.loop = true;
        current.musicSource.Play();

        //Play the audio that repeats whenever the level reloads
        PlaySceneRestartSFXAudio();
    }

    public static void StartLevel7_1Audio()
    {
        //Set the clip for ambient audio, tell it to loop, and then tell it to play
        current.ambientSource.clip = current.ambientClipLevel7_1;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        //Set the clip for music audio, tell it to loop, and then tell it to play
        current.musicSource.clip = current.musicClipLevel7_1;
        current.musicSource.loop = true;
        current.musicSource.Play();

        //Play the audio that repeats whenever the level reloads
        PlaySceneRestartSFXAudio();
    }

    public static void StartLevel7_2Audio()
    {
        //Set the clip for ambient audio, tell it to loop, and then tell it to play
        current.ambientSource.clip = current.ambientClipLevel7_2;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        //Set the clip for music audio, tell it to loop, and then tell it to play
        current.musicSource.clip = current.musicClipLevel7_2;
        current.musicSource.loop = true;
        current.musicSource.Play();

        //Play the audio that repeats whenever the level reloads
        PlaySceneRestartSFXAudio();
    }
    #endregion

    #region SoundEffects
    public static void PlayKrakenSquishSFXAudio()
    {
        //If there is no current AudioManager or the player source is already playing
        //a clip, exit 
        if (current == null || current.soundEffectsSource.isPlaying)
            return;

        //Pick a random squish sound
        int index = Random.Range(0, current.krakenSquishesClips.Length);

        //Set the squish clip and tell the source to play
        current.soundEffectsSource.clip = current.krakenSquishesClips[index];
        current.soundEffectsSource.Play();
    }

    public static void PlayMoveOnWoodSFXAudio()
    {
        //If there is no current AudioManager or the player source is already playing
        //a clip, exit 
        if (current == null || current.soundEffectsSource.isPlaying)
            return;

        //Pick a random wood sound
        int index = Random.Range(0, current.moveOnWoodSFXClips.Length);

        //Set the wood clip and tell the source to play
        current.soundEffectsSource.clip = current.moveOnWoodSFXClips[index];
        current.soundEffectsSource.Play();
    }

    public static void PlayHitFleshSFXAudio()
    {
        //If there is no current AudioManager or the player source is already playing
        //a clip, exit 
        if (current == null || current.soundEffectsSource.isPlaying)
            return;

        //Pick a random hit sound
        int index = Random.Range(0, current.hitFleshSFXClips.Length);

        //Set the hit clip and tell the source to play
        current.soundEffectsSource.clip = current.hitFleshSFXClips[index];
        current.soundEffectsSource.Play();
    }

    public static void PlaySceneRestartSFXAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the level reload sting clip and tell the source to play
        current.soundEffectsSource.clip = current.levelStartSFXClip;
        current.soundEffectsSource.Play();
    }

    public static void PlayDoorOpenSFXAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the door open sting clip and tell the source to play
        current.soundEffectsSource.clip = current.doorOpenSFXClip;
        current.soundEffectsSource.PlayDelayed(1f);
    }

    public static void PlayDeathSFXAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the door open sting clip and tell the source to play
        current.soundEffectsSource.clip = current.deathSFXClip;
        current.soundEffectsSource.PlayDelayed(1f);
    }

    public static void PlayExhaustedSFXAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the door open sting clip and tell the source to play
        current.soundEffectsSource.clip = current.exhaustedSFXClip;
        current.soundEffectsSource.PlayDelayed(1f);
    }

    public static void PlayGrabSFXAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the door open sting clip and tell the source to play
        current.soundEffectsSource.clip = current.grabSFXClip;
        current.soundEffectsSource.PlayDelayed(1f);
    }

    public static void PlayWaterBlasterSFXAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the door open sting clip and tell the source to play
        current.soundEffectsSource.clip = current.waterBlastSFXClip;
        current.soundEffectsSource.PlayDelayed(1f);
    }
    #endregion

    #region PlayerAudio
    public static void PlayFootstepAudio()
    {
        //If there is no current AudioManager or the player source is already playing
        //a clip, exit 
        if (current == null || current.playerSource.isPlaying)
            return;

        //Pick a random footstep sound
        int index = Random.Range(0, current.walkStepClips.Length);

        //Set the footstep clip and tell the source to play
        current.playerSource.clip = current.walkStepClips[index];
        current.playerSource.Play();
    }

    public static void PlaySwordAttackAudio()
    {
        //If there is no current AudioManager or the player source is already playing
        //a clip, exit 
        if (current == null || current.playerSource.isPlaying)
            return;

        //Pick a random sword attack  sound
        int index = Random.Range(0, current.swordAttackClips.Length);

        //Set the attack clip and tell the source to play
        current.playerSource.clip = current.swordAttackClips[index];
        current.playerSource.Play();

        //Set the jump voice clip and tell the source to play
        current.voiceSource.clip = current.attackVoiceClip;
        current.voiceSource.Play();
    }

    public static void PlayJumpAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the jump SFX clip and tell the source to play
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();

        //Set the jump voice clip and tell the source to play
        current.voiceSource.clip = current.jumpVoiceClip;
        current.voiceSource.Play();
    }

    public static void PlayDeathAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the death SFX clip and tell the source to play
        current.playerSource.clip = current.deathClip;
        current.playerSource.Play();

        //Set the death voice clip and tell the source to play
        current.voiceSource.clip = current.deathVoiceClip;
        current.voiceSource.Play();

        //Set the death sting clip and tell the source to play
        current.soundEffectsSource.clip = current.deathSFXClip;
        current.soundEffectsSource.Play();
    }

    public static void PlayHurtAudio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the death voice clip and tell the source to play
        current.voiceSource.clip = current.hurtVoiceClip;
        current.voiceSource.Play();

        PlayHitFleshSFXAudio();
    }

    #endregion

    #region BossAudio
    public static void PlayBossTauntAudio()
    {
        //If there is no current AudioManager or the player source is already playing
        //a clip, exit 
        if (current == null || current.bossSource.isPlaying)
            return;

        //Pick a random footstep sound
        int index = Random.Range(0, current.bossTauntClips.Length);

        //Set the footstep clip and tell the source to play
        current.bossSource.clip = current.bossTauntClips[index];
        current.bossSource.Play();
    }

    public static void PlayBossAttack1Audio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the jump SFX clip and tell the source to play
        current.bossSource.clip = current.bossAttack1Clip;
        current.bossSource.Play();
    }

    public static void PlayBossAttack2Audio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the jump SFX clip and tell the source to play
        current.bossSource.clip = current.bossAttack2Clip;
        current.bossSource.Play();
    }

    public static void PlayBossAttack3Audio()
    {
        //If there is no current AudioManager, exit
        if (current == null)
            return;

        //Set the jump SFX clip and tell the source to play
        current.bossSource.clip = current.bossAttack3Clip;
        current.bossSource.Play();
    }

    #endregion

    #region ModifyVolumePerLevel
    /*
     * Only Values between -80 and 20 possible
     */
    private static void setLevelBackgroundVolume(int musicVolume, int ambientVolume)
    {
        setMusicVolume(musicVolume);
        setAmbientVolume(ambientVolume);
    }

    /*
     * Only Values between -80 and 20 possible
     */
    private static void setMusicVolume(int value)
    {
        current.musicGroup.audioMixer.SetFloat("musicVol", value);
    }

    /*
     * Only Values between -80 and 20 possible
     */
    private static void setAmbientVolume(int value)
    {
        current.ambientGroup.audioMixer.SetFloat("ambientVol", value);
    }
    #endregion
}
