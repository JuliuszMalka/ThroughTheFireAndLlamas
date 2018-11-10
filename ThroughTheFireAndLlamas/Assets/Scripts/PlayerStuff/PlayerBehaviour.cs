using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerBehaviour : NetworkBehaviour {

	public Vector3 lookDirection = new Vector3(0f, 0f, 0f);
	public Vector3 moveDirection = new Vector3(0f, 0f, 0f);
	private Rigidbody rbody = null;

	void Awake() {
		rbody = gameObject.GetComponent<Rigidbody>();
	}

	void Update () {

        if (isLocalPlayer)
        {
            float x = 0f, z = 0f;


            x = Input.GetAxis("Horizontal") * PlayerStatistics.GetInstance().playerStats.Speed * 5f;
            z = Input.GetAxis("Vertical") * PlayerStatistics.GetInstance().playerStats.Speed * 5f;

            if (x != 0f || z != 0f)
            {
                moveDirection.y = 0f;
                moveDirection.x = x;
                moveDirection.z = z;
                rbody.velocity = moveDirection;
            }
            else
            {
                rbody.velocity = Vector3.zero;
            }

            lookDirection.x = x;
            lookDirection.z = z;
            lookDirection.y = 0f;
            lookDirection = lookDirection.normalized;

            if (PlayerStatistics.GetInstance().timers["hit"] > 0f)
            {
                PlayerStatistics.GetInstance().timers["hit"] -= Time.deltaTime;
            }
            else
            {
                PlayerStatistics.GetInstance().ToggleFlag("isHit", false);
            }
        }
    }

    void Attack() {
		Vector3 attackDirection = lookDirection.normalized;
		Debug.Log("attack direction: " + attackDirection.ToString());
		RaycastHit[] hits = Physics.RaycastAll(
			this.transform.position,
			attackDirection,
			0f//tymczasowe zero bo możliwe że zjebałem//PlayerStatistics.GetInstance().equippedWeapon != null ? PlayerStatistics.GetInstance().equippedWeapon.data.range : 2f
		); 
	}
}
