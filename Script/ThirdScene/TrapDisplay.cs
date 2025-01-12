using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class TrapDisplay : MonoBehaviour
{
    public Trap trap;
    public Image image;
    private TrapManager trapManager;

    void Start()
    {
        trapManager = FindObjectOfType<TrapManager>();
        if(trap != null && image != null)
        {
            image.sprite = trap.ImageTrap;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.CompareTag("Character"))
        {
            if (trapManager != null)
            {
                trapManager.ActivateTrapEffect(trap, collision.gameObject);
                
            }
        }
        Destroy(gameObject);
    }
    

    
}
