using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedGameMode : MonoBehaviour {

	[SerializeField]
	public TurnBasedGameModeData mData;
	[HideInInspector]
	public FSM<TurnBasedGameMode> mFSM = null;

	void Awake()
	{
		mFSM = new FSM<TurnBasedGameMode>(this);
		mFSM.GlobalState = new TurnBasedGameModeGlobalState();
		mFSM.ChangeState(new TurnBasedGameModeOpeningState());
	}

	void Start () {
		
	}
	

	void Update () {
		mFSM.Update ();
	}
}
