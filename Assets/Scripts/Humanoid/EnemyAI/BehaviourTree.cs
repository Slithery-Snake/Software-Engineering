using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GenericBT
{
    public enum NodeState {RUNNING, SUCCESS, FAILURE }
    public abstract class BehaviourTree : MonoBehaviour
    {

        private Node root;
        protected void Start()
        {
            root = SetUpTree();   
        }
        protected void FixedUpdate()
        {
            root.Evaluate();
        }
        protected abstract Node SetUpTree();
    }
    public class Sequence  : Node
    {
        bool childrenRunning;
        public Sequence () : base ()
        {
           
        }
        public Sequence(List<Node> children): base(children) { }
        public override NodeState Evaluate()
        {
            foreach(Node child in children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                      return state;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        continue;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        childrenRunning = true;
                        continue ;
                    default:
                        state = NodeState.SUCCESS;
                            return state;
                }
            }
            state = childrenRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
    public class Selector : Node
    {
        bool childrenRunning;
        public Selector() : base()
        {

        }
        public Selector(List<Node> children) : base(children) { }
        public override NodeState Evaluate()
        {
            foreach (Node child in children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        childrenRunning = true;
                        return state;
                    default:
                        continue;
                }

            }
            state = NodeState.FAILURE;
            return state;
        }
    }
    public class Node
    {
        protected NodeState state;
        public Node parent;
        protected List<Node> children = new List<Node>();
        Dictionary<string, object> sharedData = new Dictionary<string, object>();
       public Node () { children = null; }
        public Node(List<Node> children)
        {
            foreach(Node child in children)
            {
                AttatchNode(child);
            }
        }
       void AttatchNode(Node node)
       {
            node.parent = this;
            children.Add(this);
       }
        public virtual NodeState Evaluate()
        => NodeState.FAILURE;
        public void SetData (string k, object v)
        {
            sharedData[k] = v;
        }
        public object GetData(string k)
        {
            object result = null;
            if(sharedData.TryGetValue(k, out result))
            {
                return result;
            }
            Node node = parent;
            while (node !=null)
            {
                result = node.GetData(k);
                if (result != null)
                {
                    return result;
                }
                node = node.parent;
            }
            return null;
        }
        public bool ClearData(string k)
        {
            if(sharedData.ContainsKey(k))
            {
                sharedData.Remove(k);
                return true;
            }
            Node node = parent;
            while(node !=null)
            {
                bool clear = node.ClearData(k);
                if(clear) { return true; }
                node = node.parent;
            }
            return false;
        }
    }

}


