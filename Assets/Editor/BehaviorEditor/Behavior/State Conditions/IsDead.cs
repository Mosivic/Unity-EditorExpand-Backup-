using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ms.BehaviorEditor
{
    [CreateAssetMenu(menuName ="Condition/IsDead",fileName ="IsDead")]
    public class IsDead : Condition
    {
        public override bool CheckCondition(StateManager manager)
        {
            return manager.health <= 0;
        }
    }

}
