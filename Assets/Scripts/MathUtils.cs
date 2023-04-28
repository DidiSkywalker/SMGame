using UnityEngine;

namespace DefaultNamespace
{
    public class MathUtils
    {
        public static bool CompareAlmostEqual(float x, float y, float epsilon)
        {
            // Based upon implementation:
            // http://floating-point-gui.de/errors/comparison/

            if (x == y)
                return true;

            float absX = Mathf.Abs(x);
            float absY = Mathf.Abs(y);
            float diff = Mathf.Abs(x - y);

            if (x * y == 0)
                return diff < (epsilon * epsilon);
            else if (absX + absY == diff) // [1]
                return diff < epsilon;
            else
                return diff / (absX + absY) < epsilon;
        }
    }
}