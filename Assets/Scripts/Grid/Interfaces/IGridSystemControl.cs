// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

public interface IGridSystemControl
{
    void SingleColControlUpdate(bool friendly = true);

    void AddGridColumn();

    void AddGridRow();

    GridCell GetTileAtPosition(Vector2 position);
}
