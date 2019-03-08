/* Copyright (c) 2018 Kuneko. All rights reserved. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour {

	//----------------------------------------
	// Public Variables
	//----------------------------------------

	public AudioClip impactSound;

	//----------------------------------------
	// Private Variables
	//----------------------------------------

	private Rigidbody body;

	//----------------------------------------
	// Start
	//----------------------------------------

	IEnumerator Start () {

		body = GetComponent<Rigidbody>();

		yield return new WaitForSeconds(0.5f);

		body.isKinematic = false;

	}

	//----------------------------------------
	// Update
	//----------------------------------------

	void Update () {

		transform.Rotate(new Vector3(body.velocity.z * 10.0f, 0.0f, -body.velocity.x * 10.0f), Space.World);

	}

	//----------------------------------------
	// On Collision Enter
	//----------------------------------------

	void OnCollisionEnter () {

		Functions.PlaySound(impactSound, 0.1f, 1.0f, GetComponent<AudioSource>());

	}

	//----------------------------------------
	// On Trigger Exit
	//----------------------------------------

	void OnTriggerExit () {

		if (GameLogic.turn == 1)
			GameLogic.scoreP1++;
		else
			GameLogic.scoreCPU++;

		if (GameLogic.scoreP1 + GameLogic.scoreCPU >= 11)
			GameObject.Find("GameLogic").SendMessage("GameOver");

	}

}
