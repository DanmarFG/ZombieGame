using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }
        public override NodeState Evaluate()
        {
            bool anyChildRunning = false;


            foreach(Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FALIURE:
                        state = NodeState.FALIURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildRunning = true;
                        continue;
                        default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }

            state = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;    
            return state;
        }
    }
}


