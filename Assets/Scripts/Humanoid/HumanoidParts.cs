using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidParts : MonoBehaviour
{
     static Vector3 leftHandDefault = new Vector3(-0.7f, 0.79f, 0.74f);
     static Vector3 rightHandDefault = new Vector3(0.7f, 0.79f, 0.74f);
    [System.Serializable]
    public struct Parts {
        public Transform head;
        public Transform body;
        public Collider rHandCol;
        public Collider lHandCol;
        public Transform hands;
        public Animator lHandAnim;
        public Animator rHandAnim;
        public Transform rHand;
        public Transform lHand;
       

    }
    
    [SerializeField] protected Parts parts;

    public Parts Parts1 { get => parts;  }
    public static Vector3 LeftHandDefault { get => leftHandDefault;}
    public static Vector3 RightHandDefault { get => rightHandDefault;  }
}
