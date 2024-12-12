using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfigurarOpciones : MonoBehaviour
{
     public Dropdown dimensionDropdown;
    public Dropdown jugadoresDropdown;
    public Dropdown fichasDropdown;
    public Button playButton;
    public static int dimensionLaberinto;
    public static int cantidadJugadores;
    public static int cantidadFichas;

    void Start()
    {
        // Configurar listeners para los dropdowns
        dimensionDropdown.onValueChanged.AddListener(delegate {
            OnDimensionChanged(dimensionDropdown);
        });

        jugadoresDropdown.onValueChanged.AddListener(delegate {
            OnJugadoresChanged(jugadoresDropdown);
        });
        fichasDropdown.onValueChanged.AddListener(delegate {
            OnFichasChanged(fichasDropdown);
        });

        // Configurar listener para el botón Play
        playButton.onClick.AddListener(OnPlayButtonClicked);

        // Inicializar valores con las opciones seleccionadas por defecto
        OnDimensionChanged(dimensionDropdown);
        OnJugadoresChanged(jugadoresDropdown);
        OnFichasChanged(fichasDropdown);
    }

    public void OnDimensionChanged(Dropdown dropdown)
    {
        dimensionLaberinto = int.Parse(dropdown.options[dropdown.value].text);
    }

    public void OnJugadoresChanged(Dropdown dropdown)
    {
        cantidadJugadores = int.Parse(dropdown.options[dropdown.value].text);
    }
    public void OnFichasChanged(Dropdown dropdown){
        cantidadFichas= int.Parse(dropdown.options[dropdown.value].text);
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
}

