//author - Jordan -> Nov 27, 2026

using Entities.Abstract;
using UnityEngine;

//beginning action
//for placing a turret in front of the player, basically only for the turret, go figure
public class SpellActionPlaceCloneTurret : SpellAction
{
    public override void OnPerformAction()
    {
        MakeTurret();
        EndAction(); //set the finish flag
    }

    //make Turret
    private void MakeTurret()
    {
        IGridSystemView gridView = GridManager.instance;
        Vector2Int frontOfPlayer = Vector2Int.RoundToInt(castingData.spellCaster.transform.position) + Vector2Int.right;
        GridCell targetTile = gridView.GetTileAtPosition(frontOfPlayer);
        if (targetTile != null)
        {
            if (targetTile.isOccupied == false && targetTile.isFriendly == true)
            {
                Entity cloneTurret = EntityManager.GetInstance().GetEntity(EntityType.UnitTestCloneTurret, new Vector3(frontOfPlayer.x, frontOfPlayer.y, -1), true, 0f);

                //set the owner of the cloneTurret
                CloneTurretSpellCaster cloneTurretSpellCaster = cloneTurret.GetComponent<CloneTurretSpellCaster>();
                SpellCaster owner = castingData.spellCaster.GetComponent<SpellCaster>();
                cloneTurretSpellCaster.SetOwner(owner);
            }
        }
    }
}
