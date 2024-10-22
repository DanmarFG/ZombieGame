using System.Collections.Generic;

namespace BehaviourTree
{
    public enum NodeState { RUNNING, SUCCESS, FALIURE }

    public class Node
    {
        public NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        private Dictionary<string, object> data = new Dictionary<string, object>();

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node node in children)
            {
                Attach(node);
            }
        }

            private void Attach(Node node)
            {
                node.parent = this;
                children.Add(node);
            }

        public virtual NodeState Evaluate() => NodeState.FALIURE;

        public void SetData(string key, object value)
        {
            data[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;
            if (data.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = parent;
            while(node != null)
            {
                value = node.GetData(key);
                if(value != null)
                    return value;
                node = node.parent;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if (data.ContainsKey(key))
            {
                data.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null) 
            {
                bool cleared = node.ClearData(key);
                if(cleared)
                    return true;
                node = node.parent;
            }

            return false;
        }
    }
}

