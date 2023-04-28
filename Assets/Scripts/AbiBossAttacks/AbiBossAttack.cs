using UnityEngine;

namespace AbiBossAttacks
{
    public abstract class AbiBossAttack : ScriptableObject
    {
        public void Use(GameObject gameObject)
        {
            UseImpl(gameObject);
        }
        protected abstract void UseImpl(GameObject boss);
        public abstract void Update();
    }
}
