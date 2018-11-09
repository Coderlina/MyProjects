using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds methods for creating different cell patterns.
/// When player selects a cell pattern from UI buttons the selection is saved in _selectedPatternType variable.
/// When player clicks, the cells under the highlighted pattern become alive.
/// </summary>
public class CellPattern : MonoBehaviour {
    #region Fields and properties


    private GameOfLife _gameOfLife;
    private Grid _grid;
    public CellPatternType SelectedPatternType {private get; set; }

    #endregion

    private void Start()
    {
        Cell.OnCellClickedCallback += OnCellClicked;
        _gameOfLife = FindObjectOfType<GameOfLife>();
        _grid = _gameOfLife.Grid;
    }

    /// <summary>
    /// Called by UiManager when user selects a new cell pattern.
    /// </summary>
    /// <param name="type"></param>
    public void OnCellPatternSelected(CellPatternType type)
    {
        SelectedPatternType = type;
    }

    /// <summary>
    /// Create a cell group corresponding to the currently selected cell pattern
    /// when player clicks a cell.
    /// </summary>
    private void OnCellClicked(Cell clickedCell)
    {
        PlaceCellPattern(clickedCell, GetPatternCells(SelectedPatternType));
    }

    /// <summary>
    /// Places cell pattern starting from clickedCell's coordinates.
    /// </summary>
    private void PlaceCellPattern(Cell clickedCell, IEnumerable<Cell> pattern)
    {
        foreach (var cell in pattern)
        {
            _gameOfLife.MakeCellAlive(clickedCell.Coordinates.x + cell.Coordinates.x, clickedCell.Coordinates.y + cell.Coordinates.y);
        }
    }

    /// <summary>
    /// Creates a Cell Pattern.
    /// </summary>
    public List<Cell> GetPatternCells(CellPatternType patternType)
    {
        switch (patternType)
        {
            case CellPatternType.Cell:
                return new List<Cell> {_grid.GetCell(0, 0)};
            case CellPatternType.Block:
                return GetCellBlock();
            case CellPatternType.Beacon:
                return GetBeaconCells();
            case CellPatternType.Blinker:
                return GetBlinkerCells();
            case CellPatternType.Toad:
                return GetToadCells();
            case CellPatternType.Glider:
                return GetGliderCells();
            default:
                return null;
        }
    }

    #region Helper methods for pattern creating

    #endregion

    /// <summary>
    /// Get list of cells in a horizontal or vertical row.
    /// </summary>
    private List<Cell> GetCellRow(int length, int startX = 0, int startY = 0, bool horizontal = true)
    {
        var row = new List<Cell>();
        for (var i = 0; i < length; i++)
        {
            row.Add(horizontal ? _grid.GetCell(startX + i, startY) : _grid.GetCell(startX, startY + i));
        }
        return row;
    }

    /// <summary>
    /// Get list of cells in 2 x 2 block.
    /// </summary>
    private List<Cell> GetCellBlock(int startX = 0, int startY = 0)
    {
        var block = new List<Cell>();
        for (var relativeX = 0; relativeX < 2; relativeX++)
        {
            for (var relativeY = 0; relativeY < 2; relativeY++)
            {
                block.Add(_grid.GetCell(startX + relativeX, startY + relativeY));
            }
        }
        return block;
    }

    #region Oscillators

    ///// <summary>
    ///// Get "beacon" cells.
    ///// https://en.wikipedia.org/wiki/File:Game_of_gameOfLife_beacon.gif
    ///// </summary>
    private List<Cell> GetBeaconCells()
    {
        var pattern = GetCellBlock(0, 2);
        pattern.AddRange(GetCellBlock(2));
        return pattern;
    }

    /// <summary>
    /// Get "blinker" cells.
    /// https://upload.wikimedia.org/wikipedia/commons/9/95/Game_of_gameOfLife_blinker.gif
    /// </summary>
    private List<Cell> GetBlinkerCells() {
        return GetCellRow(3);
    }

    ///// <summary>
    ///// Get "toad" cells.
    ///// https://upload.wikimedia.org/wikipedia/commons/1/12/Game_of_gameOfLife_toad.gif
    ///// </summary>
    private List<Cell> GetToadCells()
    {
        var pattern = GetCellRow(3); //Row length = 3, start x = 0, start y = 0
        pattern.AddRange(GetCellRow(3, 1, 1)); //Row length = 3, start x = 1, start y = 1
        return pattern;
    }

    #endregion

    #region Spaceships
    ///// <summary>
    ///// Get "glider" cells.
    ///// https://en.wikipedia.org/wiki/Conway%27s_Game_of_gameOfLife#/media/File:Game_of_gameOfLife_animated_glider.gif
    ///// </summary>
    private List<Cell> GetGliderCells()
    {
        var pattern = GetCellRow(3); //length = 3, x = 0, y = 0
        pattern.Add(_grid.GetCell(1, 2));
        pattern.AddRange(GetCellRow(2, 2, 0, false)); //length = 2, x = 2, y = 0, vertical row);
        return pattern;
    }
    #endregion


}
