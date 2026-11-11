//author - Jordan -> Nov 08, 2026
//credits to Jacky for their ported over Turret code

using Entities.Abstract;
using UnityEngine;

//beginning action
//for placing a turret in front of the player, basically only for the turret, go figure
public class SpellBeginningPlaceTurret : SpellActionBeginning
{
    public override void OnPerformAction()
    {
        MakeTurret();
        EndAction(); //set the finish flag
    }

    //make Turret
    //Jacky's code ported over
    private void MakeTurret()
    {
        IGridSystemView gridView = GridManager.instance;
        Vector2Int frontOfPlayer = Vector2Int.RoundToInt(castingData.spellCaster.transform.position) + Vector2Int.right;
        GridCell targetTile = gridView.GetTileAtPosition(frontOfPlayer);
        if (targetTile != null)
        {
            if (targetTile.isOccupied == false && targetTile.isFriendly == true)
            {
                EntityManager.GetInstance().GetEntity(EntityType.UnitTestTurret, new Vector3(frontOfPlayer.x, frontOfPlayer.y, -1), true, 0f);
            }
        }
    }
}