using UnityEngine;
using System.Collections.Generic;

public class DamageRange<T> where T: struct {

	private T min;
	private T max;

	public DamageRange(T first, T second) {
		this.min = first;
		this.max = second;
	}

	public void SetNewRange(T first, T second) {
		this.min = first;
		this.max = second;
	}

	public uint GetSize() {
		return 2;
	}

	public T this[uint index] {
		get {
			if (index == 1 || index >= this.GetSize()) return this.max;
			return this.min;
		}
	}

	public override string ToString() {
		return "min: " + this.min.ToString() + ", max: " + this.max.ToString();
	}

}
