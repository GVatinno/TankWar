using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent(typeof(Tank))]
public class PlayerTankController : MonoBehaviour
{

    [HideInInspector]
	public Tank mTank = null;
    [HideInInspector]
    public FSM<PlayerTankController> mFSM = null;
	
	void Awake()
	{
		mTank = GetComponent<Tank>();
		mFSM = new FSM<PlayerTankController>(this);
		mFSM.GlobalState = new PlayerGlobalState();
		mFSM.ChangeState (new PlayerIdleState ());
	}

	void Start () {
		
	}
	

	void Update () {
		mFSM.Update();
	}
}
