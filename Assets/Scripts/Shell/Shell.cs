using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

	private Rigidbody mRb = null;
	private Tank mOwner = null;
	private TrailRenderer mTrailRenderer = null;
    private Tank mTarget = null;

	void Awake()
	{
		mRb = GetComponent<Rigidbody> ();
		mOwner = null;
		mTrailRenderer = GetComponent<TrailRenderer> ();
	}

	void Start () 
	{
		
	}

	public void Shoot(Vector3 position, Vector3 force, Tank owner, Tank target)
	{
		mOwner = owner;
        mTarget = target;

        mRb.gameObject.transform.position = position;
		mRb.gameObject.transform.rotation = Quaternion.LookRotation (force.normalized);
		mRb.velocity = Vector3.zero;
		mRb.angularVelocity = Vector3.zero;
		mRb.isKinematic = false;
		mRb.AddForce (force, ForceMode.Impulse);
		mTrailRenderer = GetComponent<TrailRenderer> ();
		mTrailRenderer.Clear ();
		mTrailRenderer.enabled = true;
	}

	void OnTriggerEnter(Collider other)
	{
		Destroy (this.gameObject);
        MessageBus.Instance.TankAttackFinishing(mOwner, (other.ClosestPointOnBounds(transform.position) - mTarget.transform.position).sqrMagnitude);
        MessageBus.Instance.TankAttackFinished(mOwner);

    }
}
