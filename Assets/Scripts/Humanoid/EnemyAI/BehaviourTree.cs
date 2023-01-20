using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
namespace GenericBT
   
{
    public enum NodeState { RUNNING, SUCCESS, FAILURE }
    public abstract class BehaviourTree : MonoBehaviour
    {
        private bool isEvaluating = true;

        private Node root;
        protected Coroutine evaluation;
        protected  Func<NodeState> Eval;
        protected bool IsEvaluating { get => isEvaluating;}

        protected virtual void Start()
        {
           
            root = SetUpTree();
            Eval = root.Evaluate;

            // StartEvaluation();
        }

        protected void Pause()
        {
            Eval = Running;
        }
      protected  void StartEval()
        {
            Eval = root.Evaluate;
        }
      protected  NodeState Running()
        {
            return NodeState.RUNNING;
        }
        protected void Update()
        {
            Eval();

        }
        /*
        IEnumerator Evaluate()
        {
            while (true)
            {
                root.Evaluate();
                yield return new WaitForEndOfFrame();
            }
        }
        
        public void StartEvaluation()
        {
            evaluation = StartCoroutine(Evaluate());
        }
        public void PauseEval()
        {
            if(evaluation!=null)
            {
                StopCoroutine(evaluation);
            }
        }*/
      protected abstract Node SetUpTree();

    }
        public class Node
        {
            protected NodeState state;
            public Node parent;
            protected List<Node> children;
            public Node() { children = null; }
            public Node(List<Node> children)
            {

                this.children = new List<Node>();
                foreach (Node child in children)
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
                foreach (Node node in children)
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


