﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	RaycastHit hit;

	[SerializeField] Camera fpsCam;
	[SerializeField] ParticleSystem muzzleFlash;
	[SerializeField] GameObject impactEffect;

	[SerializeField] float fireRate = 15f;
	[SerializeField] float damage;
	[SerializeField] float range;

	private float nextTimeToFire = 0f;
	

	void Update () {
		
		if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire){
			nextTimeToFire = Time.time + 1f/fireRate;
			Shoot();
		}
	}

	void Shoot(){

//		muzzleFlash.Stop();
//		muzzleFlash.Play();

		if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){

			Target target = hit.transform.GetComponent<Target>();
			if(target != null){
				target.TakeDamage(damage);
			}

			if(hit.rigidbody != null){
				hit.rigidbody.AddForce(-hit.normal * 100);
			}

			GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
			Destroy(impactGo, 1f);
		}
	}
}