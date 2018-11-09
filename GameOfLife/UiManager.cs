using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI code for Game Of Life.
/// </summary>
public class UiManager : MonoBehaviour
{

    public static Color HighlightColor = new Color32(0x8E, 0xFF, 0x8F, 0xFF);

    #region Instance fields and properties

    [Header("Texts")]
    [SerializeField] private Text _generationsText;

    [Header("Buttons")]
    [SerializeField] private GameObject _startTimerButton;
    [SerializeField] private GameObject _stopTimerButton;

    private UiCellPatternButton _selectedButton;
    private GameOfLife _gameOfLife;
    private Grid _grid;
    private CellPattern _cellPattern;
    private List<Cell> _cellsToHighlight;
    private List<Cell> _selectedCellPattern;
    #endregion

    #region Instance methods
    /// <summary>
    /// Sets callback functions. Sets initial active/inactive state of buttons.
    /// </summary>
    private void Start()
    {
        _gameOfLife = FindObjectOfType<GameOfLife>();
        _gameOfLife.OnGenerationChangedCallback += OnGenerationCountChanged;
        _grid = FindObjectOfType<Grid>();
        _cellPattern = FindObjectOfType<CellPattern>();
        _cellsToHighlight = new List<Cell>();
        _startTimerButton.SetActive(true);
        _stopTimerButton.SetActive(false);
        Cell.OnMouseEnterCallback += OnMouseEnteredCell;
    }

    /// <summary>
    /// Gets called when generations count changes in GameOfLife script.
    /// Updates UI's "Generations" text.
    /// </summary>
    private void OnGenerationCountChanged(int generations) {
        _generationsText.text = generations.ToString();
    }

    /// <summary>
    /// Start/stop the advancing of the in-game time.
    /// </summary>
    public void OnClickTimerButton() {
        _gameOfLife.Timer.ToggleTimer();
        _startTimerButton.SetActive(!_startTimerButton.activeSelf);
        _stopTimerButton.SetActive(!_stopTimerButton.activeSelf);
    }

    /// <summary>
    /// Reset the game to it's initial state. Clears the grid.
    /// </summary>
    public void OnClickResetButton() {
        _gameOfLife.ResetGame();
        _startTimerButton.SetActive(true);
        _stopTimerButton.SetActive(false);
    }

    /// <summary>
    /// Called by UiCellPatternButton.cs when a cell pattern button is clicked (= cell pattern is selected by the player).
    /// </summary>
    public void OnCellPatternSelected(UiCellPatternButton patternButton) {
        //Inform the cell pattern creator script which pattern is selected.
        _cellPattern.OnCellPatternSelected(patternButton.CellPatternType);
        _selectedCellPattern = _cellPattern.GetPatternCells(patternButton.CellPatternType);

        //When a button is selected, deselect previously selected button.
        if (_selectedButton != null) {
            _selectedButton.Deselect();
        }
        _selectedButton = patternButton;
    }

    /// <summary>
    /// Move the highlight pattern when mouse moves on the grid.
    /// </summary>
    private void OnMouseEnteredCell(Cell hoveredCell)
    {
        if (_cellsToHighlight.Count != 0)
        {
            HighlightCells(_cellsToHighlight, false);
            _cellsToHighlight.Clear();
        }
        foreach (var cell in _selectedCellPattern)
        {
            _cellsToHighlight.Add(_grid.GetCell(hoveredCell.Coordinates.x + cell.Coordinates.x, hoveredCell.Coordinates.y + cell.Coordinates.y));
        }
        HighlightCells(_cellsToHighlight);
    }

    /// <summary>
    /// Highlight all the cells in the list.
    /// </summary>
    private void HighlightCells(List<Cell> cellsToHighlight, bool highlight = true)
    {
        foreach (var cell in cellsToHighlight)
        {
            //Skip these coordinates if it's out of grid bounds OR
            //if the cell in the coordinates is alive it doesn't need to be highlighted.
            if (cell == null || !_grid.IsInsideGrid(cell) || cell.IsAlive)
                continue;
            cell.HighlightCell(highlight);
        }
    }

    #endregion
}
