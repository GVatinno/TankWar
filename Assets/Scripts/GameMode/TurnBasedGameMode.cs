using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedGameMode : MonoBehaviour {

	[SerializeField]
	private TurnBasedGameModeData mData = null;
	[HideInInspector]
	private FSM<TurnBasedGameMode> mFSM = null;
	
	public TurnBasedGameModeData data
	{
		get { return mData; }
	}
	
	void Awake()
	{
		mFSM = new FSM<TurnBasedGameMode>(this);
		ChangeState(new TurnBasedGameModeOpeningState());
	}
	
	public void ChangeState(IState<TurnBasedGameMode> state)
	{
		mFSM.ChangeState (state);
	}

	void Update () {
		mFSM.Update ();
	}
}
