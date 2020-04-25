using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ms.BehaviorEditor
{
    [CreateAssetMenu(menuName ="Actions/Test/Add Health")]
    public class ChangeHealth : StateAction
    {

        public override void Execute(StateManager state)
        {
            state.health += 10;
        }
    }
}

