using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public float speed = 0f;
	public Color sphereColor = Color.blue;
	public float knockbackForce = 0f;
	public float jumpForce = 0f;
	public bool isGrounded = false;
	public float clipLength = 0f;
	public float soundTimer = 0f;

	// Use this for initialization
	void Start () {
		clipLength = gameObject.GetComponent<AudioSource>().clip.length;
	}
	
	// Update is called once per frame
	void Update () {
		float x, z;
		x = Input.GetAxis("Horizontal");
		z = Input.GetAxis("Vertical");
		transform.Translate(new Vector3(x, 0f, z) * speed * Time.deltaTime);
		if (Input.GetKeyDown(KeyCode.Space)) {
			gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}
		if (Input.GetKeyDown(KeyCode.C) && soundTimer <= 0f) {
			gameObject.GetComponent<AudioSource>().Play();
			soundTimer = clipLength;
		}

		if (soundTimer > 0f) {
			soundTimer -= Time.deltaTime;
		}
	}

	// void OnCollisionEnter(Collision other) {
	// 	if (other.gameObject.CompareTag("Enemy")) {
	// 		Destroy(other.gameObject);
	// 	}
	// }

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Enemy")) {
			Rigidbody temp = other.gameObject.GetComponent<Rigidbody>();
			temp.AddForce(-Vector3.forward * knockbackForce, ForceMode.Impulse);
			gameObject.GetComponent<AudioSource>().Play();
			soundTimer = clipLength;
		}
	}
}
