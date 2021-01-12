using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Maths
{
    public static class Vector2fExtension
    {
        public static float Len2(this Vector2f obj)
        {
            return obj.X * obj.X + obj.Y * obj.Y;
        }

        public static float Len(this Vector2f obj)
        {
            return (float) Math.Sqrt(obj.Len2());
        }

        public static float Angle(this Vector2f obj)
        {
            double len = obj.Len();
            double acos = Math.Acos(obj.X / len);

            float result = 0;
            if (obj.Y >= 0)
            {
                result = (float) acos;
            }
            else
            {
                result = (float) ((Math.PI - acos) + Math.PI);
            }

            return (float) (result * 180 / Math.PI);
        }

    }
}
