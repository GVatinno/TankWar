

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


    private IState<T> mGlobalState = null;
    public IState<T> GlobalState
    {
        get { return mGlobalState; }
        set { mGlobalState = value; }
    }

    public FSM(T agent)
    {
        owner = agent;
    }

    // This is called by the Agent whenever the Game invokes the Agent's Update() method
    public void Update()
    {
        if (mGlobalState != null)
        {
            mGlobalState.Execute(owner);
        }
        if (mCurrentState != null)
        {
            mCurrentState.Execute(owner);
        }
    }

//    public bool HandleMessage(Telegram telegram)
//    {
//        if (globalState != null)
//        {
//            if (globalState.OnMesssage(owner, telegram))
//            {
//                return true;
//            }
//
//        }
//        if (currentState != null)
//        {
//            if (currentState.OnMesssage(owner, telegram))
//            {
//                return true;
//            }
//        }
//        return false;
//    }


    public void ChangeState(IState<T> newState)
    {
        mPreviousState = mCurrentState;
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
