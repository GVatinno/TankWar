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
        MessageBus.Instance.TankAttackFinishing(mOwner, ComputeSignedDistance(other.ClosestPointOnBounds(transform.position)));
        if ( other.gameObject.layer == Utils.LayerTank)
        {
            MessageBus.Instance.TankDestroyed(other.GetComponent<Tank>());
            PlayExplosionSound();
        }
        MessageBus.Instance.TankAttackFinished(mOwner);
        PoolManager.Instance.ReturnPoolElement(PoolManager.PoolType.SHELL, this.gameObject);
    }

    void PlayExplosionSound()
    {
        GameObject explosion = PoolManager.Instance.GetPoolElement(PoolManager.PoolType.EXPLOSION);
        explosion.SetActive(true);
        explosion.transform.position = this.transform.position;
        explosion.GetComponent<PoolSound>().Play();
    }

    
    float ComputeSignedDistance(Vector3 collisionPoint)
    {
        // negative if before the thank, positive if after ( from the other tank perspective )
        collisionPoint.y = mTarget.transform.position.y;
        Vector3 planeNormal = (mTarget.transform.position - mOwner.transform.position).normalized;
        float signedDistance =  Vector3.Dot(collisionPoint - mTarget.transform.position, planeNormal);
        // TODO TEST IF IT'S EQUAL SQRDISTANCE SO NO NORMALIZATION
        return signedDistance;
    }

}
