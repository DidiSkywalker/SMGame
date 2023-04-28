using UnityEngine;

namespace AbiBossAttacks.Functions
{
    public abstract class Function : ScriptableObject
    {
        public string textRepresentation;
        public abstract float Apply(float x);
    }
}