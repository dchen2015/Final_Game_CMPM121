/* Copyright (c) 2018 Kuneko. All rights reserved. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

	//----------------------------------------
	// Public Static Variables
	//----------------------------------------

	public static int turn;
	public static int scoreCPU;
	public static int scoreP1;

	//----------------------------------------
	// Public Variables
	//----------------------------------------

	public Transform pivot;
	public Transform shooter;
	public TextMesh scoreCPUText;
	public TextMesh scoreP1Text;
	public AudioClip loseSound;
	public AudioClip throwSound;
	public AudioClip winSound;

	//----------------------------------------
	// Private Variables
	//----------------------------------------

	private Vector3 startPos;
	private Vector3 delta;
	private Vector3 lastPos;
	private Vector2 speed;
	private float rotX;
	private int turnCPU;
	private int turnDir;
	private bool gameOver;
	private bool hasControl = true;

	//----------------------------------------
	// Start
	//----------------------------------------

	void Start () {

		turn = 1;
		scoreP1 = 0;
		scoreCPU = 0;
		startPos = shooter.localPosition;

	}

	//----------------------------------------
	// Update
	//----------------------------------------

	void Update () {

		scoreP1Text.text = scoreP1.ToString();
		scoreCPUText.text = scoreCPU.ToString();

		if (!hasControl || gameOver)
			return;

		if (turn == 1) {
			if (Input.GetMouseButtonDown(0)) {
				delta = Vector3.zero;
				lastPos = Input.mousePosition;
			} else if (Input.GetMouseButton(0)) {
				delta = Input.mousePosition - lastPos;
				lastPos = Input.mousePosition;
			} else if (Input.GetMouseButtonUp(0)) {
				delta = Vector3.zero;
				lastPos = Input.mousePosition;
				StartCoroutine(Throw());
			}
			if (Mathf.Abs(delta.x) > 1.0f)
				speed.x = Mathf.Clamp(speed.x -= delta.x * 0.8f, -100.0f, 100.0f);
			else
				speed.x *= 0.95f;
			if (Mathf.Abs(delta.y) > 1.0f)
				speed.y = Mathf.Clamp(speed.y += delta.y * 0.015f, -100.0f, 100.0f);
			else
				speed.y *= 0.95f;
		} else {
			if (turnCPU > 0) {
				turnCPU--;
				if (turnDir == 0)
					speed.x = Mathf.Lerp(speed.x, -100, Time.deltaTime * 5.0f);
				else
					speed.x = Mathf.Lerp(speed.x, 100, Time.deltaTime * 5.0f);
			} else {
				if (speed.x > 5.0f)
					speed.x *= 0.95f;
				else
					StartCoroutine(Throw());
			}
		}

		rotX = Mathf.Clamp(rotX + speed.y, -15.0f, -8.0f);
		pivot.Rotate(new Vector3(0.0f, speed.x * Time.deltaTime, 0.0f));
		pivot.eulerAngles = new Vector3(rotX, pivot.eulerAngles.y, 0.0f);

	}

	//----------------------------------------
	// Throw
	//----------------------------------------

	IEnumerator Throw () {

		hasControl = false;
		Functions.PlaySound(throwSound);
		Vector3 pos = (shooter.position - Camera.main.transform.position) * (Mathf.Clamp(-(rotX * 0.5f), 1.0f, 5.0f) * 0.4f);
		shooter.parent = null;
		shooter.GetComponent<Rigidbody>().isKinematic = false;
		shooter.GetComponent<Rigidbody>().AddForce(pos.x * 700.0f, 20.0f, pos.z * 700.0f);

		yield return new WaitForSeconds(3.0f);

		shooter.GetComponent<Rigidbody>().isKinematic = true;
		shooter.parent = Camera.main.transform;
		shooter.localPosition = startPos;
		turn = -turn;
		speed = Vector2.zero;

		if (turn == -1) {
			turnCPU = Random.Range(5, 20);
			turnDir = Random.Range(0, 2);
			yield return new WaitForSeconds(1.0f);
		}

		hasControl = true;

	}

	//----------------------------------------
	// Game Over
	//----------------------------------------

	IEnumerator GameOver () {

		gameOver = true;

		yield return new WaitForSeconds(1.0f);

		transform.Find("GameOver").gameObject.SetActive(true);
		transform.Find("UI").gameObject.SetActive(false);

		yield return new WaitForSeconds(1.0f);

		if (scoreP1 > scoreCPU) {
			Functions.PlaySound(winSound);
			transform.Find("GameOver/TextGameOver").GetComponent<TextMesh>().text = "YOU WIN";
		} else {
			Functions.PlaySound(loseSound);
			transform.Find("GameOver/TextGameOver").GetComponent<TextMesh>().text = "YOU LOSE";
		}

		yield return new WaitForSeconds(4.0f);

		StartCoroutine(Functions.LoadLevel("Title"));

	}

}
