using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTankController : MonoBehaviour {

	[HideInInspector]
	public Tank mTank = null;
	[HideInInspector]
	public FSM<AiTankController> mFSM = null;
    public AiTankStrategy mStrategy = null;

    void Awake()
	{
		mTank = GetComponent<Tank>();
		mFSM = new FSM<AiTankController>(this);
		mFSM.GlobalState = new AITankGlobalState();
		mFSM.ChangeState(new AITankIdleState());
        mStrategy = new AiTankStrategy(mTank.mData);

    }

	void Start () {

	}


	void Update () {
		mFSM.Update();
	}
}
