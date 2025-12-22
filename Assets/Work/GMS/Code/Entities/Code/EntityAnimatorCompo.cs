using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Work.Entities
{
    public class EntityAnimatorCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] protected Animator animator;
        public Entity Owner { get; protected set; }

        public virtual void InitCompo(Entity entity)
        {
            Owner = entity;
        }

        public void SetParam(int hash, bool value) => animator.SetBool(hash, value);
        public void SetParam(int hash, int value) => animator.SetInteger(hash, value);
        public void SetParam(int hash, float value) => animator.SetFloat(hash, value);
        public void SetParam(int hash) => animator.SetTrigger(hash);
    }
}
