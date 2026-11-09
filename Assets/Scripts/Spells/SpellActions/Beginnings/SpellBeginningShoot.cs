//author - Jordan -> Nov 08, 2026

using Entities.Abstract;
using Entities.Projectile.ProjectileTypes;
using UnityEngine;

//beginning action
//for shooting a pea out from the player, basically only for the basicShot
//rename this later, i chose a trash ass name
public class SpellBeginningShoot : SpellActionBeginning
{
    public override void OnPerformAction()
    {
        MakePea();
        EndSpell(); //set the finish flag
    }

    //make Pea
    private void MakePea()
    {
        Entity entity = EntityManager.GetInstance().GetEntity(EntityType.ProjectilePea, new Vector3(castingData.casterGrid.x, castingData.casterGrid.y, -1), castingData.spellCaster.GetIsFriendly());

        PeaBullet pea = entity.GetComponent<PeaBullet>();
        pea.SetMaxLifeSpan(2f);
    }
}
