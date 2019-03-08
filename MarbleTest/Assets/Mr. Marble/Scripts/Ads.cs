/* Copyright (c) 2018 Kuneko. All rights reserved. */

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class Ads : MonoBehaviour {

	//----------------------------------------
	// Start
	//----------------------------------------

	void Start () {

		#if UNITY_ADS
		if (Advertisement.IsReady("video"))
			Advertisement.Show("video", new ShowOptions{resultCallback = HandleShowResult});
		else
			Finished();
		#else
		Finished();
		#endif

	}

	//----------------------------------------
	// Handle Show Result
	//----------------------------------------

	#if UNITY_ADS
	void HandleShowResult (ShowResult result) {

		switch (result) {
		case ShowResult.Finished:
			Debug.Log("AD SHOWN");
			break;
		case ShowResult.Skipped:
			Debug.Log("AD SKIPPED");
			break;
		case ShowResult.Failed:
			Debug.LogError("AD FAILED");
			break;
		}

		Finished();

	}
	#endif

	//----------------------------------------
	// Finished
	//----------------------------------------

	void Finished () {

		SceneManager.LoadScene("Main");

	}

}
