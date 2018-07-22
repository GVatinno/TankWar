

using System;
using System.Collections.Generic;
using UnityEngine;

public class MessageBus : Singleton<MessageBus>
{
    public Action<Tank> TankReachedPosition = delegate {  };
	public Action StartPositioning = delegate {  };
	public Action<Tank> StartTankAttack = delegate {  };
    public Action<Tank> TankAttackFinished = delegate { };
    public Action<Tank> TankDestroyed = delegate { };
    public Action<Tank, float> TankAttackFinishing = delegate { };
}
