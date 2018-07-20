using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTankController : MonoBehaviour {

	private Tank mTank = null;
	private FSM<Tank> mFSM = null;

	void Awake()
	{
		mTank = GetComponent<Tank>();
		mFSM = new FSM<Tank>(mTank);
		mFSM.GlobalState = new AITankGlobalState();
		mFSM.ChangeState(new AITankMoveState());
	}

	void Start () {

	}


	void Update () {
		mFSM.Update();
	}
}
