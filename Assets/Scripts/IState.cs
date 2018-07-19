using System;
using System.Collections.Generic;


abstract public class IState<T>
{
    // This will be executed when the state is entered
    public abstract void Enter(T agent);

    // This is called by the Agent's update function each update step
    public abstract void Execute(T agent);

    // This will be executed when the state is exited
    public abstract void Exit(T agent);

//    // This will be executed when the agent receives a message
//    abstract public bool OnMesssage(T agent, Telegram telegram);
}