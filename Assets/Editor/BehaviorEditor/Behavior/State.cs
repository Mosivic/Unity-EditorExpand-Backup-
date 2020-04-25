using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ms.BehaviorEditor
{
    [CreateAssetMenu(fileName ="State",menuName ="State/NormalState")]
    public class State :ScriptableObject
    {
        public StateAction[] OnActions;
        public List<Connect> transitions = new List<Connect>();

    }
}

