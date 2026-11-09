//author - Jordan -> Nov 08, 2026

using UnityEngine;

//has the explosion shapes to be called and used
//this file kinda sucks no lie, probs a better way to set this up

public delegate ExplosionPattern SpellShape(Vector2Int direction, int size);

//check ExplosionPattern.cs for more details
public static class SpellShapes
{
    public static ExplosionPattern Cross(Vector2Int direction, int size)
    {
        return new ExplosionPatternCross(size, size);
    }

    public static ExplosionPattern Diagonal(Vector2Int direction, int size)
    {
        return new ExplosionPatternDiagonal(size);
    }

    public static ExplosionPattern Quad(Vector2Int direction, int size)
    {
        return new ExplosionPatternQuad(size, size);
    }

    public static ExplosionPattern Lazer(Vector2Int direction, int size)
    {
        return new ExplosionPatternLazer(direction, 10);
    }
}