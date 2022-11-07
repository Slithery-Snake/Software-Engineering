using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GenericBT
{
    public enum NodeState {RUNNING, SUCCESS, FAILURE }
    public abstract class BehaviourTree : MonoBehaviour
    {
        
        private Node root;
        protected virtual void Start()
        {
            root = SetUpTree();   
        }
        protected virtual void Update()
        {
            root.Evaluate();
        }
        protected abstract Node SetUpTree();
    }

    public class Node
    {
        protected NodeState state;
        public Node parent;
        protected List<Node> children;
       public Node () { children = null; }
        public Node(List<Node> children)
        {

            this.children = new List<Node>();
            foreach(Node child in children)
            {
                AttatchNode(child);
            }
        }
       void AttatchNode(Node node)
       {
            node.parent = this;
            children.Add(node);
       }
        public virtual NodeState Evaluate()
        => NodeState.FAILURE;
       
    }
    public class Sequence : Node
    {
        public Sequence(List<Node> children) : base(children)
        {
        }
        public override NodeState Evaluate()
        {
            bool childrenRunning = false;
            foreach(Node node in children)
            {
               switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.RUNNING:
                        childrenRunning = true;

                        continue;
                    case NodeState.SUCCESS:
                            continue;
                    default: state = NodeState.SUCCESS;
                        return state;

                }
            }
            state = childrenRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return NodeState.SUCCESS;
        }
    }
    public class Selector : Node
    {
        public Selector(List<Node> children) : base(children)
        {
        }
        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING; 
                        return state;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    default:
                        continue;

                }
            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}


