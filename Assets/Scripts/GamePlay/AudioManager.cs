using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
/** Author: Sebastián Jiménez Fernández.
* Class for controlling the eaudio in the game.
* */
public class AudioManager : MonoBehaviour
{

	private Scene actualSecene; // NEVER DO THIS.
	public static AudioManager instance;
	public static AudioManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<AudioManager>();
				if (instance == null)
				{
					instance = new GameObject("AudioManager").AddComponent<AudioManager>();
				}
			}
			return instance;
		}
	}

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;


    void Awake()
	{

		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			DontDestroyOnLoad(this.gameObject);
			instance = this;
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}


	//NEVER THIS IN THIS CLASS.
	#region OnSceneLaoded

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	//Cuando carga la escena.
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Game")
		{
			ChangeVolume("Menu", 0f);
			ChangeVolume("Game",0.4F);
		}
		if (scene.name == "Menu")
		{
			Cursor.lockState = CursorLockMode.None;
			PlayM();
			ChangeVolume("Game",0f);
			ChangeVolume("Menu",0.4F);
		}
		actualSecene = scene;
	}
	#endregion




	//Recibe el nombre del sonido y lo reproduce.
	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	//Recibe el nombre del sonido y lo reproduce directamente, pudiendo sonar varias veces(disparos por ejemplo).
	public void PlayOneShot(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.PlayOneShot(s.clip);
	}

	//Recibe el nombre del sonido y lo reproduce con un tiempo de delay.
	public void PlayDelayed(string sound, float time)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.PlayDelayed(time);
	}

	//Recibe el nombre del sonido y lo reproduce solo si no está sonando ya.
	public void PlayOneAtTime(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		if (!s.source.isPlaying)
			s.source.Play();
	}

	//Recibe el sonido y lo pause.
	public void Pause(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if (s.source.isPlaying)
			s.source.Pause();
	}


	//Recibe el sonido y lo reanuda.
	public void UnPause(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if (s.source.isPlaying)
			s.source.UnPause();
	}

	//Recibe el sonido y cambie el volumen.
	public void ChangeVolume(string sound, float volume)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if (s.source.isPlaying)
			s.source.volume = volume;
	}

	//Recibe el el source.
	public AudioSource GetSource(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return null;
		}

		if (s.source != null)
			return s.source;
		else return null;
	}

	//Recibe el sonido y lo para.
	public void Stop(string sound)
    {
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if (s.source.isPlaying)
			s.source.Stop();
	}
	//Para todos los sonidos que estén sonando.
	public void StopAll()
	{
        foreach (var sound in sounds)
		{
			if (sound.source.isPlaying)
				sound.source.Stop();
		}
	}

	#region Methods with volume parameter.

	//Recibe el nombre del sonido y lo reproduce.
	public void Play(string sound, float volume)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	public void PlayOneShot(string sound, float volume)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.PlayOneShot(s.clip);
	}
	//Recibe el nombre del sonido y lo reproduce solo si no está sonando ya, CON VOLUMEN de parámetro.
	public void PlayOneAtTime(string sound, float volume)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		if (!s.source.isPlaying)
			s.source.Play();
	}

	//Recibe el nombre del sonido y lo reproduce con un tiempo de delay.
	public void PlayDelayed(string sound, float time, float volume)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.PlayDelayed(time);
	}
	#endregion



	public void PlayM()
	{
		PlayOneAtTime("V1");
		ChangeVolume("V1", 0F);
		PlayOneAtTime("V2");
		ChangeVolume("V2", 0F);
		PlayOneAtTime("V3");
		ChangeVolume("V3", 0F);
		PlayOneAtTime("V4");
		ChangeVolume("V4", 0F);
		PlayOneAtTime("V5");
		ChangeVolume("V5", 0F);
		PlayOneAtTime("V6");
		ChangeVolume("V6", 0F);
		PlayOneAtTime("DV1");
		ChangeVolume("DV1", 0F);
		PlayOneAtTime("DV2");
		ChangeVolume("DV2", 0F);
		PlayOneAtTime("DV3");
		ChangeVolume("DV3", 0F);
		PlayOneAtTime("DV4");
		ChangeVolume("DV4", 0F);
		PlayOneAtTime("DV5");
		ChangeVolume("DV5", 0F);
		PlayOneAtTime("DV6");
		ChangeVolume("DV6", 0F);
		PlayOneAtTime("E1");
		ChangeVolume("E1", 0F);
		PlayOneAtTime("E2");
		ChangeVolume("E2", 0F);
		PlayOneAtTime("E3");
		ChangeVolume("E3", 0F);
		PlayOneAtTime("E4");
		ChangeVolume("E4", 0F);
		PlayOneAtTime("Game");
		if(SceneManager.GetActiveScene().name != "Menu")
		ChangeVolume("Game", 0.4F);
		else
			ChangeVolume("Game", 0F);
		PlayOneAtTime("Menu");
		ChangeVolume("Menu", 0F);
		PlayOneAtTime("Credits");
		ChangeVolume("Credits", 0F);
	}
}
