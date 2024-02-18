using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameSystem
{
    public abstract class DropItem : MonoBehaviour
    {
        public string ItemName;
        public Sprite ItemSprite;
        public int Quantity;
        public string Description;
        public GameObject WorldObjectPrefab;
 
        public virtual string GetDescription()
        {
            return Description;
        }
    }
}
