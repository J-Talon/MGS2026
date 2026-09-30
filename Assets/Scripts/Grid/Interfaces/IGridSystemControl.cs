using UnityEngine;

public interface IGridSystemControl
{
    void SingleColControlUpdate(bool freindly = true);

    void AddGridColumn();

    void AddGridRow();

    GridCell GetTileAtPosition(Vector2 position);
}
