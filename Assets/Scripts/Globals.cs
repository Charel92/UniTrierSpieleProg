using UnityEngine;

public static class Globals
{
    public const string ip = "localhost";
    public const int port = 4321;
    public const string name = "Superjhemp";
    public static int clientCount = 0;

    public static string[] SplitPositionString(string position)
    {
        return position.Split(',');
    }

    public static Vector3 SplitPositionStringToVector(string position)
    {
        string[] pos = SplitPositionString(position);
        return new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
    }

    public static string PositionToString(Vector3 pos)
    {
        return pos.x + "," + pos.y + "," + pos.z;
    }

    public static string PositionToString(float x,float y, float z)
    {
        return x + "," + y + "," + z;
    }

}
