using UnityEngine;

namespace AbiBossAttacks.Functions
{
    [CreateAssetMenu(menuName = "AbiBoss Functions/X Squared")]
    public class XSquared : Function
    {
        public override float Apply(float x)
        {
            return .5f * x * x;
        }
    }
}