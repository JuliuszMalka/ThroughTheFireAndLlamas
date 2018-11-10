using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ReflectionTest : MonoBehaviour {

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Log(PlayerStatistics.GetInstance().weaponSkills[PlayerStatistics.GetInstance().typeOfWeapon]);
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			MethodInfo temp = PlayerStatistics.GetInstance().weaponSkills[PlayerStatistics.GetInstance().typeOfWeapon];
			temp.Invoke(null, new object[] {new Vector3(Random.Range(0, 1.5f), Random.Range(0, 3f), Random.Range(0, 5f))});
		}

		if (Input.GetKeyDown(KeyCode.S)) {
			PlayerStatistics.GetInstance().IncreaseAttribute("Strength");
			//Debug.Log("After the increment: " + PlayerStatistics.GetInstance().playerAttributes.Strength.ToString())
		}
	}
}
