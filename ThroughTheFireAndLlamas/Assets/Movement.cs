using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Movement : NetworkBehaviour {

	public float speed = 0f;
	public float knockbackForce = 0f;
	public float jumpForce = 0f;
	public bool isGrounded = false;
    public GameObject BarrelPrefab;
    public Transform BarrelSpawn;
    public float timer = 0f;

	// Use this for initialization
	void Start () {
        speed = 10f;
        BarrelSpawn.Rotate(new Vector3(0f, 0f, 1f) * 90f);
    }
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer)
        {

            if (timer > 0) timer -= Time.deltaTime;

            float y, z;
            y = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");

            transform.Translate(new Vector3(y, 0f, z) * 1.5f *speed * Time.deltaTime);
            //transform.Rotate(new Vector3(0f, y, 0f) * 17f * speed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space) && timer <= 0) CmdShoot();
        }
	}

    [Command]
    void CmdShoot()
    {
        if(timer <= 0f) timer = 1.2f;
        //Create object to shoot
        GameObject bullet = (GameObject)Instantiate(BarrelPrefab, BarrelSpawn.position, BarrelSpawn.rotation);

        //Add velocity
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 45f;

        //Spawning bullet on the Clients
        NetworkServer.Spawn(bullet);

        //Destruction
        Destroy(bullet, 1.5f);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
