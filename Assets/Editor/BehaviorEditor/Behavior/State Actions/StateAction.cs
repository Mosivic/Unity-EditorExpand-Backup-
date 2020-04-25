using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ms.BehaviorEditor
{
    public abstract class StateAction : ScriptableObject
    {
        public abstract void Execute(StateManager state);

    }
}

