using Ms.BehaviorEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ms.BehaviorEditor
{
    public class Connect
    {
        public BaseNode LeftNode { get; set; }
        public BaseNode RightNode { get; set; }

        public Connect(BaseNode leftNode,BaseNode rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
        }
    }
}

