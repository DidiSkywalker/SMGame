using UnityEngine;

namespace AbiBossAttacks.Functions
{
    [CreateAssetMenu(menuName = "AbiBoss Functions/SinX")]
    public class SinX : Function
    {
        public override float Apply(float x)
        {
            return Mathf.Sin(x) + 1;
        }
    }
}