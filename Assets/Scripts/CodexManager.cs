﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CodexManager : MonoBehaviour {

	public GameObject selectionMenu;
	public GameObject entry;

	public void LoadEntry()
	{
		selectionMenu.SetActive(false);
		entry.SetActive(true);
	}

	public void LoadSelection(){
		selectionMenu.SetActive(true);
		entry.SetActive(false);
	}

	public void LoadLevel(string scene){
		selectionMenu.SetActive(true);
		entry.SetActive(false);
		SceneManager.LoadScene (scene);
	}
}
