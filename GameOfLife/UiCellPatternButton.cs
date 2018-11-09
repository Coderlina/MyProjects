using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that is attached to each cell pattern button in the UI.
/// </summary>

public class UiCellPatternButton : MonoBehaviour {
    [SerializeField] private CellPatternType _cellPatternType = CellPatternType.None;
    public CellPatternType CellPatternType {
        get { return _cellPatternType; }
    }

    private UiManager _uiManager;
    private Image _buttonImage;
    private Color _buttonSelectedColor;

    /// <summary>
    /// Gets components required by this script.
    /// </summary>
    private void Start()
    {
        _uiManager = FindObjectOfType<UiManager>();
        _buttonImage = GetComponent<Image>();
        _buttonSelectedColor = UiManager.HighlightColor;

        //Select at start
        if (CellPatternType == CellPatternType.Cell)
        {
            OnSelect();
        }
    }

    /// <summary>
    /// Makes the button green to indicate that this button is selected.
    /// </summary>
    public void OnSelect() {
        if (_cellPatternType != CellPatternType.None) {
            _uiManager.OnCellPatternSelected(this);
        }
        _buttonImage.color = _buttonSelectedColor;
    }

    /// <summary>
    /// Makes the button color normal.
    /// </summary>
    public void Deselect() {
       _buttonImage.color  = Color.white;
    }

}
