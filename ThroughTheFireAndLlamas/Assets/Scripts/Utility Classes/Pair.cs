using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair<T, U> {
	
	private readonly T first;
	private readonly U second;

	Pair() {
		first = default(T);
		second = default(U);
	}

	Pair(T first, U second) {
		this.first = first;
		this.second = second;
	}

	public T GetFirstValue() {
		return this.first;
	}

	public U GetSecondValue() {
		return this.second;
	}

}
