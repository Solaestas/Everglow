using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.IIIDModule.Utils
{
    public class IIIDUtils
    {
        public static Matrix CreateLookAtFront(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
        {
            CreateLookAtFront(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out var result);
            return result;
        }

        public static void CreateLookAtFront(ref Vector3 cameraPosition, ref Vector3 cameraTarget, ref Vector3 cameraUpVector, out Matrix result)
        {
            Vector3 vector = Vector3.Normalize(cameraPosition - cameraTarget);
            Vector3 vector2 = Vector3.Normalize(Vector3.Cross(cameraUpVector, vector));
            Vector3 vector3 = Vector3.Cross(vector, vector2);
            result.M11 = vector2.X;
            result.M12 = vector3.X;
            result.M13 = vector.X;
            result.M14 = 0f;
            result.M21 = vector2.Y;
            result.M22 = vector3.Y;
            result.M23 = vector.Y;
            result.M24 = 0f;
            result.M31 = vector2.Z;
            result.M32 = vector3.Z;
            result.M33 = vector.Z;
            result.M34 = 0f;
            result.M41 = -Vector3.Dot(vector2, cameraPosition);
            result.M42 = -Vector3.Dot(vector3, cameraPosition);
            result.M43 = -Vector3.Dot(vector, cameraPosition);
            result.M44 = 1f;
        }
        public static Matrix CreateLookAtBehind(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
        {
            CreateLookAtBehind(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out var result);
            return result;
        }

        public static void CreateLookAtBehind(ref Vector3 cameraPosition, ref Vector3 cameraTarget, ref Vector3 cameraUpVector, out Matrix result)
        {
            Vector3 vector = Vector3.Normalize(cameraPosition - cameraTarget);
            Vector3 vector2 = Vector3.Normalize(Vector3.Cross(cameraUpVector, vector));
            Vector3 vector3 = Vector3.Cross(vector, vector2);
            result.M11 = -vector2.X;
            result.M12 = -vector3.X;
            result.M13 = -vector.X;
            result.M14 = 0f;
            result.M21 = vector2.Y;
            result.M22 = vector3.Y;
            result.M23 = vector.Y;
            result.M24 = 0f;
            result.M31 = vector2.Z;
            result.M32 = vector3.Z;
            result.M33 = vector.Z;
            result.M34 = 0f;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1f;
        }

    }
}
