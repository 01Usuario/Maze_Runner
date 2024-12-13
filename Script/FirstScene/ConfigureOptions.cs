using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfigureOptions : MonoBehaviour
{
    public Dropdown dimensionDropdown;
    public Dropdown playersDropdown;
    public Dropdown piecesDropdown;
    public Button playButton;

    public static int mazeDimension;
    public static int playerCount;
    public static int pieceCount;

    void Start()
    {
        // Add listeners for dropdowns
        dimensionDropdown.onValueChanged.AddListener(delegate { OnDimensionChanged(dimensionDropdown); });
        playersDropdown.onValueChanged.AddListener(delegate { OnPlayersChanged(playersDropdown); });
        piecesDropdown.onValueChanged.AddListener(delegate { OnPiecesChanged(piecesDropdown); });

        // Add listener for the play button
        playButton.onClick.AddListener(OnPlayButtonClicked);

        // Initialize with default options
        OnDimensionChanged(dimensionDropdown);
        OnPlayersChanged(playersDropdown);
        OnPiecesChanged(piecesDropdown);
    }

        public void OnDimensionChanged(Dropdown dropdown)
    {
        mazeDimension = int.Parse(dropdown.options[dropdown.value].text);
    }

    public void OnPlayersChanged(Dropdown dropdown)
    {
        playerCount = int.Parse(dropdown.options[dropdown.value].text);
    }

    public void OnPiecesChanged(Dropdown dropdown)
    {
        pieceCount = int.Parse(dropdown.options[dropdown.value].text);
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
}