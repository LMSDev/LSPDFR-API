using System;

using Rage;

public static class Vector3Extension
{
    public static Vector3 Around(this Vector3 start, float radius)
    {
        // Random direction.
        Vector3 direction = Vector3Extension.RandomXY();
        Vector3 around = start + (direction * radius);
        return around;
    }

    public static float DistanceTo(this Vector3 start, Vector3 end)
    {
        return (end - start).Length();
    }

    public static Vector3 RandomXY()
    {
        Random random = new Random(Environment.TickCount);

        Vector3 vector3 = new Vector3();
        vector3.X = (float)(random.NextDouble() - 0.5);
        vector3.Y = (float)(random.NextDouble() - 0.5);
        vector3.Z = 0.0f;
        vector3.Normalize();
        return vector3;
    }
}