/* Copyright (c) 2018 Kuneko. All rights reserved. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	//----------------------------------------
	// Public Variables
	//----------------------------------------

	public AudioClip clickSound;

	//----------------------------------------
	// Private Variables
	//----------------------------------------

	private float scale;
	private float startScale;

	//----------------------------------------
	// Start
	//----------------------------------------

	void Start () {

		startScale = transform.localScale.x;
		scale = startScale;

	}

	//----------------------------------------
	// Late Update
	//----------------------------------------

	void LateUpdate () {

		scale = Mathf.Lerp(scale, startScale, Time.deltaTime * 3.0f);
		transform.localScale = Vector3.one * scale;

	}

	//----------------------------------------
	// On Mouse Down
	//----------------------------------------

	void OnMouseDown () {

		switch (name) {
			case "ButtonPlay" :
				#if UNITY_ADS
				StartCoroutine(Functions.LoadLevel("Ads"));
				#else
				StartCoroutine(Functions.LoadLevel("Main"));
				#endif
				break;
		}

		Functions.PlaySound(clickSound);
		scale = startScale * 1.2f;

	}

}
