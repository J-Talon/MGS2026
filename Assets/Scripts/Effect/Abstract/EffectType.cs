namespace Effect.Abstract
{
    
    /*
     * @author Talon J
     * NAMING CONVENTION
     * effects that affect entities are prepended with ENTITY
     * effects for grid cells are prepended with GRID 
     *
     */
    public enum EffectType {
        
        //shutup rider I've done enums before
        
        GRID_BURNING,
        GRID_SNARE_TRAP,  //the grid has a snare viney thing
        
        ENTITY_BURNING, //this is fine
        ENTITY_MOVEMENT_LOCK, //entity cannot move
    }
}