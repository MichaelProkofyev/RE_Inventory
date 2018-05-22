using UnityEngine;

public static class VectorExtensions
{

    public static Vector2 xy(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }
    public static Vector2 xz(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }
    public static Vector2 yx(this Vector3 v3)
    {
        return new Vector2(v3.y, v3.x);
    }
    public static Vector2 yz(this Vector3 v3)
    {
        return new Vector2(v3.y, v3.z);
    }
    public static Vector2 zx(this Vector3 v3)
    {
        return new Vector2(v3.z, v3.x);
    }
    public static Vector2 zy(this Vector3 v3)
    {
        return new Vector2(v3.z, v3.y);
    }

    public static Vector3 xy_(this Vector2 v2, float a = 0)
    {
        return new Vector3(v2.x, v2.y, a);
    }
    public static Vector3 x_y(this Vector2 v2, float a = 0)
    {
        return new Vector3(v2.x, a, v2.y);
    }
    public static Vector3 _xy(this Vector2 v2, float a = 0)
    {
        return new Vector3(a, v2.x, v2.y);
    }
    public static Vector3 yx_(this Vector2 v2, float a = 0)
    {
        return new Vector3(v2.y, v2.x, a);
    }
    public static Vector3 y_x(this Vector2 v2, float a = 0)
    {
        return new Vector3(v2.y, a, v2.x);
    }
    public static Vector3 _yx(this Vector2 v2, float a = 0)
    {
        return new Vector3(a, v2.y, v2.x);
    }

    public static Vector2 Rotated(this Vector2 v2, float angle)
    {
        float cs = Mathf.Cos(angle);
        float sn = Mathf.Sin(angle);
        float x = v2.x * cs - v2.y * sn; // now x is something different than original vector x
        float y = v2.x * sn + v2.y * cs;
        v2.x = x;
        v2.y = y;
        return v2;
    }

    public static Vector2Int Int(this Vector2 v2)
    {
        return new Vector2Int((int)v2.x, (int)v2.y);
    }

}