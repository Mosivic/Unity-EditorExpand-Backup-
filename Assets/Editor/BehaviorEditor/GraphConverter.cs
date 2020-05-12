using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ms.BehaviorEditor
{
    public static class GraphConverter
    {

        static public List<Vector2> connectKeysList;
        static public List<BaseSaveNode> saveNodeList;

        public static void ConverterToData(List<BaseNode> nodes, List<Vector2> connects, BehaviorGraph graph)
        {
            if (connectKeysList == null) connectKeysList = new List<Vector2>();
            if (saveNodeList == null) saveNodeList = new List<BaseSaveNode>();
            connectKeysList.Clear();
            saveNodeList.Clear();

            graph.test = "sssss";

            foreach (var node in nodes)
            {
                if (node is StateNode)
                {
                    SavedStateNode sn = new SavedStateNode()
                    {
                        key = node.key,
                        Position = new Vector2(node.windowRect.x, node.windowRect.y),
                        State = (node as StateNode).state

                    };
                    saveNodeList.Add(sn);


                }
                else if (node is ConditionNode)
                {
                    SaveConditionNode sn = new SaveConditionNode()
                    {
                        key = node.key,
                        Position = new Vector2(node.windowRect.x, node.windowRect.y),
                        Condition = (node as ConditionNode).condition,
                    };
                    saveNodeList.Add(sn);

                }
       
            }
            
            graph.Init(connects, saveNodeList);
               
        }




        public static void ConverterToGraph(BehaviorGraph graph, out List<BaseNode> nodes, out List<Vector2> connects,out List<float> index)
        {
                    
            nodes = new List<BaseNode>();
            connects = graph.connectIndexList;
            index = new List<float>();
            if (graph.savedNodeList == null) return;


            foreach (var r in graph.savedNodeList)
            {
                if(r is SavedStateNode)
                {
                    StateNode node = new StateNode()
                    {
                        key = r.key,
                        state = (r as SavedStateNode).State,
                        windowRect = new Rect(r.Position, new Vector2(200, 100))
                    };
                    nodes.Add(node);
                    index.Add(r.key);
                }
                else if(r is SaveConditionNode)
                {
                    ConditionNode node = new ConditionNode()
                    {
                        key = r.key,
                        condition = (r as SaveConditionNode).Condition,
                        windowRect = new Rect(r.Position, new Vector2(200, 100))
                    };
                    nodes.Add(node);
                    index.Add(r.key);
                }
                
            }

       


        }



    }


    [System.Serializable]
    public class BaseSaveNode
    {
        public float key;
        public Vector2 Position;
    }

    [System.Serializable]
    public class SavedStateNode : BaseSaveNode
    {
        public State State;
    }

    [System.Serializable]
    public class SaveConditionNode : BaseSaveNode
    {
        public Condition Condition;
    }

}


