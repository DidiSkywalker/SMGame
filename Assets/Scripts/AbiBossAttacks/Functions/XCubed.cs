using UnityEngine;

namespace AbiBossAttacks.Functions
{
    [CreateAssetMenu(menuName = "AbiBoss Functions/X Cubed")]
    public class XCubed : Function
    {
        public override float Apply(float x)
        {
            return factor * x * x * x + additive;
        }
    }
}