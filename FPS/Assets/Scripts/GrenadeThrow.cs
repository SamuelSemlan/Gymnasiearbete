using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour {

	public float throwForce = 300f;
	public GameObject grenadePrefab;

	void Start () {
		
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.G)){
			ThrowGrenade();
		}
	}


	void ThrowGrenade(){
		GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
		Rigidbody rb = grenade.GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * throwForce * Time.deltaTime, ForceMode.VelocityChange);
	}
}
