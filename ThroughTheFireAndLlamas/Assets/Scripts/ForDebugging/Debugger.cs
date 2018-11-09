using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger {

	public static void DebugMeSomething(params object[] objects) {
		foreach (object temp in objects) {
			Debug.Log(temp.ToString());
		}
	}

	public static void DebugMeVector(Vector3 origin, Vector3 direction, float range, Color color) {
		Debug.DrawRay(origin, direction * range, color, 5f);
	}

	public static void RandomDebugStatement(string message) {
		Debug.Log(message);
	}
}
