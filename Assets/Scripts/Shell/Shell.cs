using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

	private Rigidbody mRb = null;
	private Tank mOwner = null;
	private TrailRenderer mTrailRenderer = null;

	void Awake()
	{
		mRb = GetComponent<Rigidbody> ();
		mOwner = null;
		mTrailRenderer = GetComponent<TrailRenderer> ();
	}

	void Start () 
	{
		
	}

	public void Shoot(Vector3 position, Vector3 force, Tank owner)
	{
		mOwner = owner;
		mRb.gameObject.transform.position = position;
		mRb.gameObject.transform.rotation = Quaternion.LookRotation (force.normalized);
		mRb.position = position;
		mRb.rotation = Quaternion.LookRotation (force.normalized);
		mRb.isKinematic = false;
		mRb.AddForce (force, ForceMode.Impulse);
		mTrailRenderer = GetComponent<TrailRenderer> ();
		mTrailRenderer.Clear ();
		mTrailRenderer.enabled = true;
	}

	void OnTriggerEnter(Collider other)
	{
		Destroy (this.gameObject);
		// put the pool here
	}
}
