//author - Jordan -> Nov 20, 2026

using Entities.Unit;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyShooter : MonoBehaviour
{
    //a temp file to actually make the enemies shoot stuff if you attach this script to them.
    //its just simply pressing the [1, 2, 3] keys. They will do a spell. Thats it, will be removed in the future

    private EnemySpellCaster spellCaster;
    private EntityUnit unit;
    private Vector2Int targetGrid;

    private void Start()
    {
        spellCaster = GetComponent<EnemySpellCaster>();
        unit = GetComponent<EntityUnit>();
    }

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasReleasedThisFrame)
        {
            spellCaster.CastDeckSpell(0);
        }

        if (Keyboard.current.digit2Key.wasReleasedThisFrame)
        {
            spellCaster.CastDeckSpell(2);
        }

        if (Keyboard.current.digit3Key.wasReleasedThisFrame)
        {
            spellCaster.CastDeckSpell(3);
        }
    }
}
