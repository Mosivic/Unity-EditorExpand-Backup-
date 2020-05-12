using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ms.BehaviorEditor
{
    [CreateAssetMenu(fileName ="State",menuName ="State/NormalState")]
    public class State :ScriptableObject
    {
        public StateAction[] PreActions;
        public StateAction[] OnActions;
        public StateAction[] AftActions;
        
        public void ExcutePreActions(StateManager manager)
        {
            if (PreActions == null) return;
            foreach(var action in PreActions)
            {
                action.Execute(manager);
            }
        }

        public void ExcuteOnActions(StateManager manager)
        {
            if (PreActions == null) return;
            foreach (var action in OnActions)
            {
                action.Execute(manager);
            }
        }

        public void ExcuteAftActions(StateManager manager)
        {
            if (PreActions == null) return;
            foreach (var action in AftActions)
            {
                action.Execute(manager);
            }
        }
    }
}

