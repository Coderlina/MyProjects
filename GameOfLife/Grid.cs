using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    [Header("Cell")]
    [SerializeField] private GameObject _cellPrefab;
    public int Rows = 45;
    public int Columns = 111;
    private Cell[,] _grid;
    private float _cellWidth;


    /// <summary>
    /// Resets the grid of cells.
    /// </summary>
    public void ResetGrid()
    {
        for (var x = 0; x < Columns; x++)
        {
            for (var y = 0; y < Rows; y++)
            {
                GetCell(x, y).Die();
            }
        }
    }
    /// <summary>
    /// Create a 2D cell grid sized rows x columns.
    /// </summary>
    public void CreateGrid()
    {
        var localPosition = Vector2.zero;
        _cellWidth = 0;
        _grid = new Cell[Columns, Rows];
        for (var x = 0; x < Columns; x++)
        {
            for (var y = 0; y < Rows; y++)
            {
                var cell = CreateCell(localPosition, transform, "Cell(" + x + ", " + y + ")");
                //By default, the cell is dead.
                cell.Die();
                cell.Coordinates.x = x;
                cell.Coordinates.y = y;
                _grid[x, y] = cell;

                //Fetch the cell's sprite width so the next cell's position can be calculated.
                if (_cellWidth == 0)
                {
                    _cellWidth = cell.GetSpriteWidth();
                }
                localPosition.y += _cellWidth;
            }
            localPosition.y = 0;
            localPosition.x += _cellWidth;
        }
    }

    /// <summary>
    /// Checks if location x, y is inside the grid bounds.
    /// </summary>
    public bool IsInsideGrid(Cell cell)
    {
        return IsInsideGrid(cell.Coordinates.x, cell.Coordinates.y);
    }

    /// <summary>
    /// Checks if location x, y is inside the grid bounds.
    /// </summary>
    public bool IsInsideGrid(int x, int y)
    {
        return (x >= 0 && x < Columns && y >= 0 && y < Rows);
    }

    /// <summary>
    /// Returns a cell in coordinates x, y
    /// </summary>
    public Cell GetCell(int x, int y)
    {
        return IsInsideGrid(x, y) ? _grid[x, y] : null;
    }

    /// <summary>
    /// Creates a cell game object in localPosition.
    /// Returns Cell component of the created object.
    /// </summary>
    private Cell CreateCell(Vector2 localPosition, Transform parent, string cellName)
    {
        GameObject cellObject = Instantiate(_cellPrefab);
        cellObject.name = cellName;
        cellObject.transform.parent = parent;
        cellObject.transform.localPosition = new Vector3(localPosition.x, localPosition.y, 0f);
        var cell = cellObject.GetComponent<Cell>();
        return cell;
    }
}
