using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent(typeof(Tank))]
public class PlayerTankController : MonoBehaviour
{


	private Tank mTank = null;
	private FSM<Tank> mFSM = null;
	
	void Awake()
	{
		mTank = GetComponent<Tank>();
		mFSM = new FSM<Tank>(mTank);
		mFSM.GlobalState = new PlayerGlobalState();
		mFSM.CurrentState = new PlayerMoveState();
	}

	void Start () {
		
	}
	

	void Update () {
		mFSM.Update();
	}
}
