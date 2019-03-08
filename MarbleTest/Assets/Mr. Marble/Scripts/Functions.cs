/* Copyright (c) 2018 Kuneko. All rights reserved. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Functions : MonoBehaviour {

	//----------------------------------------
	// Public Static Variables
	//----------------------------------------

	public static AudioSource myAudio;
	public static Transform fadeQuad;
	public static float fadeAlpha;

	//----------------------------------------
	// Play Sound
	//----------------------------------------

	public static void PlaySound (AudioClip sound, float adjust = 0.1f, float volume = 1.0f, AudioSource source = null) {

		if (source) {
			source.pitch = Random.Range(1.0f - adjust, 1.0f + adjust);
			source.PlayOneShot(sound, volume);
		} else {
			if (!myAudio)
				myAudio = Camera.main.GetComponent<AudioSource>();
			myAudio.pitch = Random.Range(1.0f - adjust, 1.0f + adjust);
			myAudio.PlayOneShot(sound, volume);
		}

	}

	//----------------------------------------
	// Smooth Look At
	//----------------------------------------

	public static void SmoothLookAt (Transform t, Vector3 position, float speed, bool clampY = true) {

		t.rotation = Quaternion.RotateTowards(t.rotation, Quaternion.LookRotation(position - t.position), Time.deltaTime * speed);

		if (clampY)
			t.eulerAngles = new Vector3(0.0f, t.eulerAngles.y, 0.0f);

	}

	//----------------------------------------
	// Fade In
	//----------------------------------------

	public static IEnumerator FadeIn (float fadeSpeed = 2.0f) {

		FixAspectRatio();

		if (!fadeQuad) {
			fadeQuad = Camera.main.transform.Find("Fade");
			fadeQuad.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.0f, 0.0f, 0.0f, 1.0f));
		}

		fadeAlpha = fadeQuad.GetComponent<Renderer>().material.color.a;

		while (fadeAlpha > 0.0f) {
			fadeAlpha = Mathf.Clamp01(fadeAlpha - fadeSpeed * Time.deltaTime);
			fadeQuad.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.0f, 0.0f, 0.0f, fadeAlpha));
			yield return null;
		}

	}

	//----------------------------------------
	// Load Level
	//----------------------------------------

	public static IEnumerator LoadLevel (string level, float fadeSpeed = 2.0f) {

		if (!fadeQuad) {
			fadeQuad = Camera.main.transform.Find("Fade");
			fadeQuad.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.0f, 0.0f, 0.0f, 0.0f));
		}

		fadeAlpha = fadeQuad.GetComponent<Renderer>().material.color.a;

		while (fadeAlpha < 1.0f) {
			fadeAlpha = Mathf.Clamp01(fadeAlpha + fadeSpeed * Time.deltaTime);
			fadeQuad.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.0f, 0.0f, 0.0f, fadeAlpha));
			yield return null;
		}

		SceneManager.LoadScene(level);

	}

	//----------------------------------------
	// Fix Aspect Ratio
	//----------------------------------------

	public static void FixAspectRatio () {

		float aspectRatio = 2.165333333f;//19.5:9 = 2.165333333 || 16:9 = 1.777778 || 16:10 = 1.6 || 3:2 = 1.5 || 4:3 = 1.33
		float currentAspectRatio = (float)Screen.width / Screen.height;

		if ((int)(currentAspectRatio * 100) / 100.0f == (int)(aspectRatio * 100) / 100.0f) {
			Camera.main.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
			return;
		}

		if (currentAspectRatio > aspectRatio) {
			float inset = 1.0f - aspectRatio / currentAspectRatio;
			Camera.main.rect = new Rect(inset / 2, 0.0f, 1.0f - inset, 1.0f);
		} else {
			float inset = 1.0f - currentAspectRatio / aspectRatio;
			Camera.main.rect = new Rect(0.0f, inset / 2, 1.0f, 1.0f - inset);
		}

		Camera backgroundCam = new GameObject("BackgroundCamera", typeof(Camera)).GetComponent<Camera>();
		backgroundCam.depth = -10;
		backgroundCam.clearFlags = CameraClearFlags.SolidColor;
		backgroundCam.backgroundColor = Color.black;
		backgroundCam.cullingMask = 0 << 0;

	}

}
