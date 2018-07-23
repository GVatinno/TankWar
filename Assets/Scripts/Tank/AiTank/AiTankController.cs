using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTankController : MonoBehaviour {


	private Tank mTank = null;
	private FSM<AiTankController> mFSM = null;
    private AiTankStrategy mStrategy = null;

	public Tank tank
	{
		get { return mTank; }
	}
	
	public AiTankStrategy aiStrategy
	{
		get { return mStrategy; }
	}
	
	void Awake()
	{
		mTank = GetComponent<Tank>();
		mFSM = new FSM<AiTankController>(this);
        mStrategy = new AiTankStrategy(mTank.tankData);
		ChangeState(new AITankIdleState());
    }
	
	public void ChangeState(IState<AiTankController> state)
	{
		mFSM.ChangeState (state);
	}

	void Update () 
	{
		mFSM.Update();
	}
}
