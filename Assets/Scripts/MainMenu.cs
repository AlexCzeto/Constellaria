﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MainMenu : MonoBehaviour {
	

	public Slider masterSlider;
	public Slider effectsSlider;
	public Slider ambienceSlider;
	public Slider musicSlider;

	public void MMStartGame ()//TODO should be set via savefile, and default to a level by string name
	{
		SceneManager.LoadScene("Demo3-TutorialLevel");
	}
	public void MMOpenCodex ()
	{
		SceneManager.LoadScene("CodexDisplay");
	}
	public void CDOpenMainMenu ()
	{
		SceneManager.LoadScene(0);
		//print (PlayerPrefsManager.GetMusicVolume());
	}
	public void CDOpenSceneByName (string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
	public void OnMasterSliderChange ()//TODO should be set via savefile
	{
		PlayerPrefsManager.SetMasterVolume (masterSlider.value);
		//print (masterSlider.value);
	}
	public void OnEffectsSliderChange ()//TODO should be set via savefile
	{
		PlayerPrefsManager.SetSoundEffectVolume (effectsSlider.value);
	}
	public void OnAmbienceSliderChange ()//TODO should be set via savefile
	{
		PlayerPrefsManager.SetAmbienceVolume (ambienceSlider.value);
	}
	public void OnMusicSliderChange ()//TODO should be set via savefile
	{
		PlayerPrefsManager.SetMusicVolume (musicSlider.value);
	}

	public void QuitGame ()
	{
		Application.Quit ();
	}
}
