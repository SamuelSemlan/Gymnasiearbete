using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

	public float delay = 3f;
	public float radius = 5f;
	public float force = 700f;
	public float explosionDamage = 5f;

	public GameObject explosionEffect;

	float countDown;
	bool hasExploded = false;

	void Start () {
		countDown = delay;
	}
	
	void Update () {
		countDown -= Time.deltaTime;
		if(countDown <= 0f && !hasExploded){
			Explode();
			hasExploded = true;
		}
	}

	void Explode(){
		GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);

		Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, radius);

		foreach(Collider nearbyObject in collidersToDestroy){

			Destructible dest = nearbyObject.GetComponent<Destructible>();
			if(dest != null){
				dest.Destroy();
			}
			Enemy enemy = nearbyObject.GetComponent<Enemy>();
			if(enemy != null){
				enemy.TakeDamage(explosionDamage);
			}
		}

		Collider[] collidersToShatter = Physics.OverlapSphere(transform.position, radius);

		foreach(Collider nearbyObject in collidersToShatter){
			Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
			if(rb != null){
				rb.AddExplosionForce(force, transform.position, radius);
			}
		}
		Destroy(gameObject);
		Destroy(effect.gameObject, 2.5f);
	}
}
