

using System;


public class FSM<T>
{
    private T owner;

    // This holds the current state for the state machine
    private IState<T> mCurrentState = null;
    public IState<T> CurrentState
    {
        get { return mCurrentState; }
        set { mCurrentState = value; }
    }

    private IState<T> mPreviousState = null;
    public IState<T> PreviousState
    {
        get { return mPreviousState; }
        set { mPreviousState = value; }
    }

    public FSM(T agent)
    {
        owner = agent;
    }

    // This is called by the Agent whenever the Game invokes the Agent's Update() method
    public void Update()
    {
        if (mCurrentState != null)
        {
            mCurrentState.Execute(owner);
        }
    }


    public void ChangeState(IState<T> newState)
    {
        mPreviousState = mCurrentState;
		if ( mCurrentState != null )
			mCurrentState.Exit(owner);
        mCurrentState = newState;
        mCurrentState.Enter(owner);
    }

    // Invoked when a state blip is finished
    public void RevertToPreviousState()
    {
        ChangeState(mPreviousState);
    }

    // Checks whether the machine is in a given state
    public Boolean IsInState(IState<T> state)
    {
        return state.Equals(mCurrentState);
    }	
}
