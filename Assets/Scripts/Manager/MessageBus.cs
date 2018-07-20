

using System;
using System.Collections.Generic;

public class MessageBus : Singleton<MessageBus>
{
    public Action<Tank> TankReachedPosition = delegate {  };
}
