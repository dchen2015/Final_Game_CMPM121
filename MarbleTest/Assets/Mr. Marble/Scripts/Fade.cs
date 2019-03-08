/* Copyright (c) 2018 Kuneko. All rights reserved. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

	//----------------------------------------
	// Awake
	//----------------------------------------

	void Awake () {

		StartCoroutine(Functions.FadeIn());

	}

}