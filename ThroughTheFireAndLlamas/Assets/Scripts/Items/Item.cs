using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public Transform player = null;
	public string _name = "";

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	public virtual void OnPickUp() {
		Debug.Log("cosiestao");
	}

	public void PickUp() {
		UIManager.instance.AddItem(this.gameObject);
		this.OnPickUp();
		this.gameObject.SetActive(false);
	}

	public void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Player")) {
			PickUp();
		}
	}
}
