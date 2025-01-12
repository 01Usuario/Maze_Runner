using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfigureOptions : MonoBehaviour
{
    public Dropdown dimensionDropdown; // Menú desplegable para la dimensión del laberinto
    public Dropdown playersDropdown; // Menú desplegable para la cantidad de jugadores
    public Dropdown piecesDropdown; // Menú desplegable para la cantidad de piezas
    public Button playButton; // Botón para comenzar el juego

    public static int mazeDimension; // Dimensión seleccionada del laberinto
    public static int playerCount; // Cantidad de jugadores seleccionados
    public static int pieceCount; // Cantidad de piezas seleccionadas

    void Start()
    {
        // Agregar listeners a los menús desplegables
        dimensionDropdown.onValueChanged.AddListener(delegate { OnDimensionChanged(dimensionDropdown); });
        playersDropdown.onValueChanged.AddListener(delegate { OnPlayersChanged(playersDropdown); });
        piecesDropdown.onValueChanged.AddListener(delegate { OnPiecesChanged(piecesDropdown); });

        // Agregar listener al botón de play
        playButton.onClick.AddListener(OnPlayButtonClicked);

        // Inicializar con opciones predeterminadas
        OnDimensionChanged(dimensionDropdown);
        OnPlayersChanged(playersDropdown);
        OnPiecesChanged(piecesDropdown);
    }

    // Actualizar la dimensión del laberinto
    public void OnDimensionChanged(Dropdown dropdown)
    {
        mazeDimension = int.Parse(dropdown.options[dropdown.value].text);
    }

    // Actualizar la cantidad de jugadores
    public void OnPlayersChanged(Dropdown dropdown)
    {
        playerCount = int.Parse(dropdown.options[dropdown.value].text);
    }

    // Actualizar la cantidad de piezas
    public void OnPiecesChanged(Dropdown dropdown)
    {
        pieceCount = int.Parse(dropdown.options[dropdown.value].text);
    }

    // Cargar la siguiente escena
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
}
