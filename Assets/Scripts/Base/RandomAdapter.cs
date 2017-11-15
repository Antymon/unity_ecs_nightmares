using Entitas;
using System;
using UnityEngine;

public interface IRandom
{
    float value { get; }
    Vector2 insideUnitCircle { get; }
}

public class SystemRandomAdapter : IRandom
{
    System.Random random;

    public SystemRandomAdapter(int seed)
    {
        random = new System.Random(seed);
    }

    public float value
    {
        get { return (float) random.NextDouble(); }
    }

    public Vector2 insideUnitCircle
    {
        get {
            var angle = random.NextDouble();
            var radius = random.NextDouble();
            var x = (float) (Math.Sin(angle) * radius);
            var y = (float) (Math.Cos(angle) * radius);
            return new Vector2(x,y);
        }
    }
}

