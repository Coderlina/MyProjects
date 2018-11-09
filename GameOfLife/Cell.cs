using UnityEngine;
using System;

/// <summary>
/// Holds the functionality of a cell in the Game Of Life.
/// </summary>
public class Cell : MonoBehaviour
{
    #region Static fields

    public static event Action<Cell> OnMouseEnterCallback;
    public static event Action<Cell> OnCellClickedCallback;

    #endregion

    #region Instance fields

    public Vector2Int Coordinates;
    public bool IsAlive { get; private set; }
    public bool IsAliveInNextGen { get; set; }
    private GameOfLife _gameOfLife;
    private SpriteRenderer _renderer;

    #endregion

    #region Instance methods

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _gameOfLife = FindObjectOfType<GameOfLife>();
    }

    /// <summary>
    /// Cell becomes alive.
    /// </summary>
    public void BecomeAlive()
    {
        IsAlive = true;
        IsAliveInNextGen = true;
        _renderer.color = Color.black;
    }

    /// <summary>
    /// Cell dies.
    /// </summary>
    public void Die()
    {
        IsAlive = false;
        IsAliveInNextGen = false;
        _renderer.color = Color.white;
    }

    /// <summary>
    /// Highlights the cell.
    /// </summary>
    /// <param name="highlight"></param>
    public void HighlightCell(bool highlight = true)
    {
        _renderer.color = highlight ? UiManager.HighlightColor : Color.white;
    }

    /// <summary>
    /// Returns the width/height of the square cell sprite.
    /// </summary>
    public float GetSpriteWidth() {
        if (_renderer.sprite) {
            return _renderer.sprite.bounds.size.x;
        }
        return 0;
    }

    #endregion

    private void OnMouseEnter()
    {
        if (_gameOfLife.Timer.IsTimerOn) return;
        if (OnMouseEnterCallback != null)
        {
            OnMouseEnterCallback(this);
        }
    }

    private void OnMouseDown()
    {
        if (_gameOfLife.Timer.IsTimerOn) return;
        if (OnCellClickedCallback != null)
        {
            OnCellClickedCallback(this);
        }
    }

}