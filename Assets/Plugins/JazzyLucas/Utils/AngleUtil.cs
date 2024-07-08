using System;
using System.Collections.Generic;
using UnityEngine;

namespace JazzyLucas.Utils
{
    /// <summary>
    /// Utilities for angles.
    /// </summary>
    public static class AngleUtil
    {
        private const int DEFAULT_CLAMP_MAX = 89;
        private const int DEFAULT_CLAMP_MIN = -89;
        
        public static Angle CustomClampAngle(Angle angle, float min = DEFAULT_CLAMP_MIN, float max = DEFAULT_CLAMP_MAX)
        {
            var ang = (float) angle;
            min = 360 + min;
            if ((ang > min && ang > 180) || (ang > 0 && ang < max))
            {
                return ang;
            }
            if (ang > max && ang < 180)
            {
                return max;
            }
            if (ang > 180 && ang < min)
            {
                return min;
            }
            return ang;
        }
    }
}