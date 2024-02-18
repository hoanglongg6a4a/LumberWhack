using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;

namespace GameSystem
{
    /// <summary>
    /// Describes an InteractableObject that can be picked up and grants a specific item when interacted with.
    ///
    /// It will also play a small animation (object going in an arc from spawn point to a random point around) when the
    /// object is actually "spawned", and the object becomes interactable only when that animation is finished.
    ///
    /// Finally it will notify the LootUI that a new loot is available in the world so the UI displays the name.
    /// </summary>
    public class Loot : InteractableObject
    {
        static float AnimationTime = 0.5f;

        public DropItem DropItem;

        public override bool IsInteractable => m_AnimationTimer >= AnimationTime;

        Vector3 m_OriginalPosition;
        Vector3 m_TargetPoint;
        float m_AnimationTimer = 0.0f;

    
        void Awake()
        {
            m_OriginalPosition = transform.position;
            m_TargetPoint = transform.position;
            m_AnimationTimer = AnimationTime - 0.1f;
        }

        protected override void Start()
        {
            base.Start();
        }

        void Update()
        {
            if (m_AnimationTimer < AnimationTime)
            {
                m_AnimationTimer += Time.deltaTime;

                float ratio = Mathf.Clamp01(m_AnimationTimer / AnimationTime);

                Vector3 currentPos = Vector3.Lerp(m_OriginalPosition, m_TargetPoint, ratio);
                currentPos.y = currentPos.y + Mathf.Sin(ratio * Mathf.PI) * 2.0f;
            
                transform.position = currentPos;

                if (m_AnimationTimer >= AnimationTime)
                {
                    //LootUI.Instance.NewLoot(this);
                }
            }
        
            Debug.DrawLine(m_TargetPoint, m_TargetPoint + Vector3.up, Color.magenta);
        }

        public override void InteractWith(CharacterData target)
        {
            //TODO
            //target.Inventory.AddItem(Item);
            //SFXManager.PlaySound(SFXManager.Use.Sound2D, new SFXManager.PlayData(){Clip = SFXManager.PickupSound});
        
            //UISystem.Instance.InventoryWindow.Load(target);
            //Destroy(gameObject);
        }

        /// <summary>
        /// This is called when the loot become available. It will setup to play the small spawn animation.
        /// This is rarely called manually, and mostly called by the LootSpawner class.
        /// </summary>
        /// <param name="position"></param>
        public void Spawn(Vector3 position)
        {
            m_OriginalPosition = position;
            transform.position = position;
        
            Vector3 targetPos = RandomPoint(transform.position, 0.1f);

            m_TargetPoint = targetPos;
            m_AnimationTimer = 0.0f;

            gameObject.layer = LayerMask.NameToLayer(Constants.ItemMask);
        }
    
        Vector2 RandomPoint(Vector2 center, float range)
        {
            return new Vector2(Random.Range(center.x - range, center.x + range),center.y);
        }
    }
}