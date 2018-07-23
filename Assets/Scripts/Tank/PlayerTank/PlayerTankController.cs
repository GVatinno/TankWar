using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent(typeof(Tank))]
public class PlayerTankController : MonoBehaviour
{

	private Tank mTank = null;
    private FSM<PlayerTankController> mFSM = null;

	public Tank tank
	{
		get { return mTank; }
	}

	void Awake()
	{
		mTank = GetComponent<Tank>();
		mFSM = new FSM<PlayerTankController>(this);
		ChangeState(new PlayerIdleState ());
	}

	public void ChangeState(IState<PlayerTankController> state)
	{
		mFSM.ChangeState (state);
	}


	void Update () {
		mFSM.Update();
	}
}
