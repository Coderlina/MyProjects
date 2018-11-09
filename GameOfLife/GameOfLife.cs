using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Game logic for the Game Of Life.
/// </summary>
public class GameOfLife : MonoBehaviour {
    #region Instance fields and properties

    [Header("Dependencies")]
    [SerializeField] private Timer _timer;

    public Timer Timer
    {
        get
        {
            return _timer;
        }
    }

    [SerializeField] private Grid _grid;
    public Grid Grid
    {
        get
        {
            return _grid;
        }
    }

    //Current generation count
    private int _generations;

    public int Generations
    {
        get { return _generations; }
        set
        {
            _generations = value;
            if (OnGenerationChangedCallback != null)
            {
                OnGenerationChangedCallback(value);
            }
        }
    }
    #endregion

    public event Action<int> OnGenerationChangedCallback;

    private void Start() {
        _grid.CreateGrid();
    }

    /// <summary>
    /// Resets the grid and game variables.
    /// </summary>
    public void ResetGame()
    {
        Generations = 0;
        _timer.IsTimerOn = false;
        _grid.ResetGrid();
    }

    /// <summary>
    /// Called by Timer.cs when generation's lifetime ends.
    /// </summary>
    public void OnGenerationLifetimeEnded() {
        //Check all cells and decide if they should live or die.
        EvaluateCells();

        //Update cell statuses
        SwitchCellGeneration();
    }

    /// <summary>
    /// Cells live or die for the next generation.
    /// </summary>
    private void SwitchCellGeneration()
    {
        for (var x = 0; x < _grid.Columns; x++)
        {
            for (var y = 0; y < _grid.Rows; y++)
            {
                var cell = _grid.GetCell(x, y);
                if (cell.IsAliveInNextGen)
                {
                    cell.BecomeAlive();
                }
                else
                {
                    cell.Die();
                }
            }
        }
        Generations++;
    }

    /// <summary>
    /// Checks every cell's adjacent/neighboring cells and decides the fate of the cell.
    /// </summary>
    private void EvaluateCells()
    {
        for (var x = 0; x < _grid.Columns; x++)
        {
            for (var y = 0; y < _grid.Rows; y++)
            {
                CheckAdjacentCells(_grid.GetCell(x, y));
            }
        }
    }

    /// <summary>
    /// Checks cells around the cell that is in location x, y.
    /// Starts from relative position x = -1, y = -1 which is the diagonal cell in southwest direction.
    /// Ends when diagonal cell on upper right is reached (relative position 1, 1).
    /// Skips the current/middle cell (relative position 0, 0).
    /// Skips any cells that are out of grid bounds.
    /// </summary>
    private void CheckAdjacentCells(Cell cell)
    {
        var adjacentsAlive = 0;
        for (var relativeX = -1; relativeX <= 1; relativeX++)
        {
            for (var relativeY = -1; relativeY <= 1; relativeY++)
            {
                if (relativeX == 0 && relativeY == 0)
                {
                    //We are on the current/middle cell, it doesn't need to be checked.
                    continue;
                }
                //Calculate the coordinates of the cell to be validated.
                var currentX = cell.Coordinates.x + relativeX;
                var currentY = cell.Coordinates.y + relativeY;

                //Check if the coordinates are inside the grid bounds.
                if (!_grid.IsInsideGrid(currentX, currentY))
                {
                    //We are out of bounds, skip this position.
                    continue;
                }

                //Check if the cell is alive.
                var adjacentCell = _grid.GetCell(currentX, currentY);
                if (adjacentCell != null && adjacentCell.IsAlive)
                {
                    adjacentsAlive++;
                }
            }
        }
        DecideFateOfCell(cell, adjacentsAlive);
    }

    /// <summary>
    /// Decides the fate of a cell (stay/become alive or die) based on how many of the neighboring cells are alive.
    /// </summary>
    private void DecideFateOfCell(Cell cell, int adjacentsAlive)
    {
        if (cell.IsAlive)
        {
            //In this cases cell dies.
            if (adjacentsAlive < 2 || adjacentsAlive > 3)
            {
                cell.IsAliveInNextGen = false;
            }
        }
        else
        {
            //Cell becomes alive.
            if (adjacentsAlive == 3)
            {
                cell.IsAliveInNextGen = true;
            }
        }
    }

    /// <summary>
    /// Makes cell in location x, y to become alive immediately.
    /// Used when player places cells/cell patterns on the grid manually.
    /// </summary>
    public void MakeCellAlive(int x, int y)
    {
        if (!_grid.IsInsideGrid(x, y))
        {
            return;
        }
        var cell = _grid.GetCell(x, y);
        cell.BecomeAlive();
    }
}
