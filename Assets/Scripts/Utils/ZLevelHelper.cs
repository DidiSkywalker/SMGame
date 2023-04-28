
using UnityEngine;

namespace Utils
{
    public static class ZLevelHelper
    {
        public const float Background = 10;

        public const float Ground = -.5f;
        
        public const float Player = -1f;
        
        public const float Foreground = -10;

        public static float Between(float back, float front)
        {
            return (back - front) / 2;
        }
        
        public static Vector3 Between(Vector3 position, float back, float front)
        {
            return position + Vector3.forward * (back - front) / 2;
        }
    }
}