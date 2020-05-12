using Ms.BehaviorEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Ms.BehaviorEditor
{
    [CreateAssetMenu(menuName = "Data/Graph")]
    public class BehaviorGraph : ScriptableObject
    {
        public string test;

        [SerializeField]
        public List<BaseSaveNode> savedNodeList;

        [SerializeField]
        public List<Vector2> connectIndexList;

        public void Init(List<Vector2> connectIndexList, List<BaseSaveNode> savedNodeList)
        {
           // if (savedNodeList == null) savedNodeList = new List<BaseSaveNode>();
           // if (connectIndexList == null) connectIndexList = new List<Vector2>();
            this.savedNodeList.Clear();
           this.connectIndexList.Clear();
            foreach (var r in connectIndexList)
            {
                this.connectIndexList.Add(r);
            }
            foreach (var r in savedNodeList)
            {
                this.savedNodeList.Add(r);
            }

        }



    }
    






}

