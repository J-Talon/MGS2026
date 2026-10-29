// Author: Jeremy
using UnityEngine;
using UnityEngine.InputSystem;
using Particle.Base;

/// <summary>
/// Test script for creating explosions in the game. This script allows you to specify the source of the explosion, its position, type, size, and damage.
/// Click on a GridCell to spawn the explosion at that location. The explosion will be created based on the specified parameters.
/// </summary>
public class TestExplosion : MonoBehaviour
{
    private GridCell selectedCell;

    void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Camera camera = Camera.main;
        if (camera == null)
            return;

        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        selectedCell = null;
        float nearestDistance = float.MaxValue;

        // Use rendered bounds instead of a physics raycast so cells do not need colliders.
        foreach (GridCell cell in Object.FindObjectsByType<GridCell>(FindObjectsSortMode.None))
        {
            foreach (Renderer cellRenderer in cell.GetComponentsInChildren<Renderer>())
            {
                if (cellRenderer.bounds.IntersectRay(ray, out float distance) && distance < nearestDistance)
                {
                    nearestDistance = distance;
                    selectedCell = cell;
                }
            }
        }

        Debug.Log(selectedCell != null ? $"Selected cell: {selectedCell.name}" : "No cell selected");
        if (selectedCell)
        {
            ExplosionVFX testVFX = new ExplosionVFX()
                    .Add(ParticleType.SPARK, ExplosionVFXStep.PreMutate)
                    .Add(ParticleType.SPARK, ExplosionVFXStep.PostMutate, 1f)
                    .Add(ParticleType.SPARK, ExplosionVFXStep.PostMutate, 1f);

            // Cross 1,1
            // new Explosion(
            //     new ExplosionPatternCross(1, 1),
            //     selectedCell.GetCellPosition(),
            //     testVFX,
            //     new MutatorDamageScale(2f),
            //     1f,
            //     null
            // ).Detonate();

            // Cross 2, 2
            // new Explosion(
            //     new ExplosionPatternCross(2, 2),
            //     selectedCell.GetCellPosition(),
            //     testVFX,
            //     new MutatorDamageScale(2f),
            //     1f,
            //     null
            // ).Detonate();

            // Diagonal 2
            // new Explosion(
            //     new ExplosionPatternDiagonal(2),
            //     selectedCell.GetCellPosition(),
            //     testVFX,
            //     new MutatorDamageScale(2f),
            //     1f,
            //     null
            // ).Detonate();

            // Laser down 2
            // new Explosion(
            //     new ExplosionPatternLaser(Vector2Int.down, 2),
            //     selectedCell.GetCellPosition(),
            //     testVFX,
            //     new MutatorDamageScale(2f),
            //     1f,
            //     null
            // ).Detonate();

            // Quad 2, 2
            new Explosion(
                new ExplosionPatternQuad(2, 2),
                selectedCell.GetCellPosition(),
                testVFX,
                new MutatorDamageScale(2f),
                1f,
                null
            ).Detonate();

            selectedCell = null;
        }
    }
}
