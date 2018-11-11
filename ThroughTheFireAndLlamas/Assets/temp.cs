using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.E)) {
			this.gameObject.GetComponent<Skill>().Allocate();
			Debug.Log(PlayerStatistics.GetInstance().allocatedSkills["fireball"].ToString());
		}
	}
}
