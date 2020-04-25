using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ms.BehaviorEditor
{
    public abstract class BaseNode : ScriptableObject
    {
        public Rect windowRect;
        public string windowTitle;
        public List<Connect> selfConnects = new List<Connect>();

        public abstract void DrawWindow();



     

    }
}

