using System;
using System.Configuration;
using UnityEngine;
using UnityEngine.AI;




public class PlayerMoveState : IState<PlayerTankController>
{
    PlayerTankController mTankController = null;

    public override void Enter(PlayerTankController agent)
    {
        mTankController = agent;
        MessageBus.Instance.TankReachedPosition += OnTankReachedPosition;
    }

    public override void Execute(PlayerTankController agent)
    {
        if ( Input.GetMouseButtonDown(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit, float.MaxValue ))
            {
                if (hit.collider.CompareTag(Utils.Ground))
                {
                    NavMeshHit navHit;
                    if ( NavMesh.SamplePosition(hit.point, out navHit, float.MaxValue, NavMesh.AllAreas) )
                    {
                        agent.tank.MoveTo(hit.point);
                    }
                    
                }
            }
        }
    }

    public override void Exit(PlayerTankController agent)
    {
        MessageBus.Instance.TankReachedPosition -= OnTankReachedPosition;
    }

    private void OnTankReachedPosition( Tank tank )
    {
        if (tank == mTankController.tank)
        {
            mTankController.ChangeState (new PlayerIdleState ());
        }
    }
}
