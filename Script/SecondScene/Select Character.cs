using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    public List<Facciones> facciones = new List<Facciones>();
    public Image imageFaccion;
    public Transform personajesPanel; // El panel donde aparecerán los botones de personajes
    public List<Button> personajesButtons; // Lista de botones de personajes
    public Button siguienteFaccionButton; // Botón para cambiar a la siguiente facción
    public Button anteriorFaccionButton; // Botón para cambiar a la facción anterior
    private int index = 0;
    private int cantidadEscoger;
    private int escogidos = 0;

    void Start()
    {
        cantidadEscoger = ConfigurarOpciones.cantidadFichas;
        MostrarFaccion();
    }

    void MostrarFaccion()
    {
        // Actualizar la imagen de la facción
        if (index >= 0 && index < facciones.Count)
        {
            imageFaccion.sprite = facciones[index].FaccionImage;

            // Asignar los personajes de la facción a los botones
            for (int i = 0; i < personajesButtons.Count; i++)
            {
                if (i < facciones[index].faccion.Count)
                {
                    var localIndex = i; // Capturar el índice localmente
                    var imageComponent = personajesButtons[localIndex].GetComponent<Image>();
                    if (imageComponent != null)
                    {
                        imageComponent.sprite = facciones[index].faccion[localIndex].CharacterImage;
                        Personajes personaje = facciones[index].faccion[localIndex];
                        personajesButtons[localIndex].onClick.RemoveAllListeners();
                        personajesButtons[localIndex].onClick.AddListener(() => SeleccionarPersonaje(personaje, personajesButtons[localIndex]));
                        personajesButtons[localIndex].gameObject.SetActive(true);
                    }
                   
                }
                else
                {
                    personajesButtons[i].gameObject.SetActive(false);
                }
            }
        }
        
    }

    public void SiguienteFaccion()
    {
        if (escogidos == 0) // Permite cambiar de facción solo si no se ha seleccionado ningún personaje
        {
            index = (index + 1) % facciones.Count;
            MostrarFaccion();
        }
    }

    public void AnteriorFaccion()
    {
        if (escogidos == 0) // Permite cambiar de facción solo si no se ha seleccionado ningún personaje
        {
            index = (index - 1 + facciones.Count) % facciones.Count;
            MostrarFaccion();
        }
    }

    void SeleccionarPersonaje(Personajes personaje, Button button)
    {

        if (escogidos < cantidadEscoger && !GameManager.instance.personajesSeleccionados.Contains(personaje))
        {
            GameManager.instance.personajesSeleccionados.Add(personaje);
            button.gameObject.SetActive(false); // Desaparecer el botón del personaje
            escogidos++;


            // Desactivar los botones de cambio de facción
            siguienteFaccionButton.interactable = false;
            anteriorFaccionButton.interactable = false;

            if (escogidos == cantidadEscoger)
            {
                // Cambiar a la siguiente escena
                SceneManager.LoadScene(2);
            }
        }
        
    }
}
