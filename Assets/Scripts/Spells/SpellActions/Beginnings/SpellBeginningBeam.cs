//author - Jordan -> Nov 08, 2026

using UnityEngine;

//shoots out from the player. I have no god damn idea what to actually name this file. If you can find a better wording for "summons from the player", be my guest
//currently used for lazer (hence why its beam for now)
public class SpellBeginningBeam : SpellActionBeginning
{ 
    public override void OnPerformAction()
    {
        explosion.StartExplosion(castingData.casterGrid, castingData);
        EndSpell();
    }
}
