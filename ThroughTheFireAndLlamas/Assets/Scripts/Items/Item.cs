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
	
	// Update is called once per frame
	public virtual void Update () {
		if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(player.position, this.transform.position) <= 0.3f) {
			PickUp();
		}
	}

	public virtual void OnPickUp() {

	}

	public void PickUp() {

	}
}
