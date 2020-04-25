using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ms.BehaviorEditor
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool CheckCondition(StateManager manager);
    }
}
