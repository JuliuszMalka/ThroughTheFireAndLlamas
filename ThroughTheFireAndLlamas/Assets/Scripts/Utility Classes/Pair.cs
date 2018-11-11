using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair<T, U> {
	
	private readonly T first;
	private readonly U second;

	public Pair() {
		first = default(T);
		second = default(U);
	}

	public Pair(T first, U second) {
		this.first = first;
		this.second = second;
	}

	public T GetFirstValue() {
		return this.first;
	}

	public U GetSecondValue() {
		return this.second;
	}

	public override string ToString() {
		return this.first.ToString() + ", " + this.second.ToString();
	}

}
