// from https://answers.unity.com/questions/1117712/master-audio-source-volume-control-with-slider.html
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerPrefsManager : MonoBehaviour {

	const string MASTER_VOLUME_KEY = "master_volume";
	const string MUSIC_VOLUME_KEY = "music_volume";
	const string SE_VOLUME_KEY = "sound_effect_volume";

	void Start(){
		SetMasterVolume (0.2f);
		SetMusicVolume (0.2f);
		SetSoundEffectVolume (0.2f);
	}


	public static void SetMasterVolume (float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat ("master_volume", volume);
		} else {
			Debug.LogError ("Master Volume out of range");
		}
	}


	public static float GetMasterVolume () {
		return PlayerPrefs.GetFloat (MASTER_VOLUME_KEY);
	}
		
	public static void SetMusicVolume (float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat ("music_volume", volume);
		} else {
			Debug.LogError ("Music Volume out of range");
		}
	}


	public static float GetMusicVolume () {
		return PlayerPrefs.GetFloat (MUSIC_VOLUME_KEY);
	}

	public static void SetSoundEffectVolume (float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat ("sound_effect_volume", volume);
		} else {
			Debug.LogError ("Sound Effect Volume out of range");
		}
	}


	public static float GetSoundEffectVolume () {
		return PlayerPrefs.GetFloat (SE_VOLUME_KEY);
	}

}