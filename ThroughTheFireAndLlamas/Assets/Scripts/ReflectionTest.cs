using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ReflectionTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.E)) {
			MethodInfo temp = PlayerStatistics.GetInstance().weaponSkills[(int)PlayerStatistics.GetInstance().typeOfWeapon];
			temp.Invoke(null, new object[] {Vector3.zero});
		}
	}
}
