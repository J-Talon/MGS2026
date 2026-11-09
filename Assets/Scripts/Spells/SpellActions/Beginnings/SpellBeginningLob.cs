//author - Jordan -> Nov 08, 2026

using Entities.Abstract;
using Entities.Projectile.ProjectileTypes;
using UnityEngine;

//lobbing, so when the player needs to lob a projectile. Kinda hard coded for bomb for now, whoops
public class SpellBeginningLob : SpellActionBeginning
{
    private float travelTime; //time taken for bomb to travel to its destination
    private float travelTimer; //timer to help with teh above
    private float directDamage; //the lob itself dealing damage when it lands on an enemy (not the explosion, think of extra damage from the bomb itself bonking on top your head)
    
    //sets the info needed (see ISpellBeginningSettings.cs and LobSettings.cs)
    public SpellBeginningLob(LobSettings lobSettings)
    {
        travelTime = lobSettings.travelTime;
        directDamage = lobSettings.directDamage;
    }

    //overrides the func from SpellAction.cs. Sets the timer and makes the bomb projectile
    public override void OnPerformAction()
    {
        MakeBomb();
        travelTimer = travelTime;
    }

    //override OnTick() from SpellAction.cs. Timer stuff from travelTime before exploding
    public override void OnTick()
    {
        travelTimer = travelTimer - Time.deltaTime;

        if (travelTimer < 0 )
        {
            explosion.StartExplosion(castingData.targetGrid, castingData);
            EndSpell();
        } 
    }

    //make bomb
    private void MakeBomb()
    {
        Entity entity = EntityManager.GetInstance().GetEntity(EntityType.ProjectileBomb, new Vector3(castingData.casterGrid.x, castingData.casterGrid.y, -1), castingData.spellCaster.GetIsFriendly());
        
        Bomb bomb = entity.GetComponent<Bomb>();
        bomb.SetIsFriendly(castingData.spellCaster.GetIsFriendly());
        bomb.SetMaxLifeSpan(travelTime);
        bomb.SetDamage(directDamage);
        bomb.start = castingData.casterGrid;
        bomb.target = castingData.targetGrid;
    }
}
