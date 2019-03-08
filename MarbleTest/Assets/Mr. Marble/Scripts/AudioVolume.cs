/* Copyright (c) 2018 Kuneko. All rights reserved. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolume : MonoBehaviour {

	//----------------------------------------
	// Public Variables
	//----------------------------------------

	public float maxVolume;

	//----------------------------------------
	// Private Variables
	//----------------------------------------

	private AudioSource source;

	//----------------------------------------
	// Awake
	//----------------------------------------

	void Awake () {

		source = GetComponent<AudioSource>();
		source.volume = 0.0f;

	}

	//----------------------------------------
	// Update
	//----------------------------------------

	void Update () {

		source.volume = (1.0f - Functions.fadeAlpha) * maxVolume;

	}

}