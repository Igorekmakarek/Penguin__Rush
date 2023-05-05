using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

	public AudioSource[] soundSources;
	public AudioSource musicSourse;
	public AudioSource backgroundSource;
	public static SoundManager instance = null;

	public float lowPitchRange = 0.95f;
	public float highPitchRange = 1.05f;

	private float standartVolume;


	void Awake()
	{

		standartVolume = AudioListener.volume;

		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

	}

	public void PlaySingle(AudioClip clip)
	{
		for (int i = 0; i < soundSources.Length; i++)
        {
			if (!soundSources[i].isPlaying)
			{
				soundSources[i].clip = clip;
				soundSources[i].Play();
				return;
			}
		}
	}

	//public void RandomizeSFX(params AudioClip[] clips)
	//{
	//	int RandomIndex = Random.Range(0, clips.Length);
	//	float randomPitch = Random.Range(lowPitchRange, highPitchRange);

	//	soundSources.pitch = randomPitch;
	//	efxSourse.clip = clips[RandomIndex];
	//	efxSourse.Play();

	//}

	public void Mute()
    {
		
		AudioListener.volume = 0;
	}

	public void UnMute()
    {
		AudioListener.volume = standartVolume;
	}

	public void StopAudio()
    {
		for (int i = 0; i < transform.childCount; i++)
        {
			transform.GetChild(i).GetComponent<AudioSource>().Pause();
        }
    }

	public void ResumeAudio()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).GetComponent<AudioSource>().Play();
		}
	}
}

