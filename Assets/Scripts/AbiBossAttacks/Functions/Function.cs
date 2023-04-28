using UnityEngine;

namespace AbiBossAttacks.Functions
{
    public abstract class Function : ScriptableObject
    {
        public string textRepresentation;
        public float factor = 1;
        public float additive = 0;
        public abstract float Apply(float x);
    }
}