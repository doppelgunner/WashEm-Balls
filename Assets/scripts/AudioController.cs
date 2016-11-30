using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

    [SerializeField]
    private AudioClip buttonFillClip;
    [SerializeField]
    private AudioClip bgMusicClip;
    [SerializeField]
    private AudioClip placeClip;

    private static AudioSource _soundSource;
    private static AudioSource _bgMusicSource;
    private static AudioClip _buttonFillClip;
    private static AudioClip _bgMusicClip;
    private static AudioClip _placeClip;

	void Start () {
        _soundSource = gameObject.AddComponent<AudioSource>();
        _bgMusicSource = gameObject.AddComponent<AudioSource>();

        _buttonFillClip = buttonFillClip;
        _bgMusicClip = bgMusicClip;
        _placeClip = placeClip;

        _bgMusicSource.loop = true;
	}
	
	
	public static void PlayButtonFillSound(float f) {
        _soundSource.clip = _buttonFillClip;
        PlaySound(f);
    }

    public static void PlayBGMusic(float f) {
        _bgMusicSource.volume = f;
        _bgMusicSource.clip = _bgMusicClip;
        _bgMusicSource.Play();
    }

    public static void PlayPlaceSound(float f) {
        _soundSource.clip = _placeClip;
        PlaySound(f);
    }

    private static void PlaySound(float f) {
        _soundSource.volume = f;
        _soundSource.Play();
    }
}
