using System;
using Unity.Entities;
using Unity.Collections;

public static class ElementProrityTable
{
    //Water Fire
    //Fire Water
    //public readonly static float[,] ElementPriorities = new float[,]
    //{
    //   { 0, 2.0f },
    //   { 0.5f, 0 }
    //};
    private readonly static float[] ElementPriorities = new float[]
    {
        0.0f, 0.5f, 2.0f, 0
    };

    public static float GetElementPriority(int x, int y)
    {
        return ElementPriorities[x + y * 2];
    }

}
