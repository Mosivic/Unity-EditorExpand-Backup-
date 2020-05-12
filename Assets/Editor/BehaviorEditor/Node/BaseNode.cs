using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ms.BehaviorEditor
{
    public abstract class BaseNode : ScriptableObject
    {
        public float key;
        public Rect windowRect;
        public string windowTitle;

        public abstract void DrawWindow();

    }
}

