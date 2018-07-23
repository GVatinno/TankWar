using System;
using System.Collections.Generic;


abstract public class IState<T>
{
    // This will be executed when the state is entered
    public virtual void Enter(T agent) {}

    // This is called by the Agent's update function each update step
    public virtual void Execute(T agent) {}

    // This will be executed when the state is exited
    public virtual void Exit(T agent) {}

}