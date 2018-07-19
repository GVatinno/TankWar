using System;
using System.Configuration;
using UnityEngine;
using UnityEngine.AI;

public class PlayerGlobalState : IState<Tank>
{
    public override void Enter(Tank agent)
    {
        // respond to events
    }

    public override void Execute(Tank agent)
    {
    }


    public override void Exit(Tank agent)
    {
    }
}


public class PlayerIdleState : IState<Tank>
{
    public override void Enter(Tank agent)
    {
    }

    public override void Execute(Tank agent)
    {
    }

    public override void Exit(Tank agent)
    {
    }
}

public class PlayerMoveState : IState<Tank>
{
    public override void Enter(Tank agent)
    {
    }

    public override void Execute(Tank agent)
    {
        if ( Input.GetMouseButtonDown(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit, float.MaxValue ))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    NavMeshHit navHit;
                    if ( NavMesh.SamplePosition(hit.point, out navHit, float.MaxValue, NavMesh.AllAreas) )
                    {
                        agent.MoveTo(hit.point);
                    }
                    
                }
            }
        }
    }

    public override void Exit(Tank agent)
    {
    }
}