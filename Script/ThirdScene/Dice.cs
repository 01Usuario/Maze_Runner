using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Dice : MonoBehaviour
{
    public TextMeshProUGUI number;
    private List<string> lista;
    private int value { get; set; }

   void Awake()
    {
        lista = new List<string> { "1", "4", "3", "2", "5", "6" };
    }


   public void Init()
    {
        StopAllCoroutines();
        StartCoroutine(VisualEffect());
        
    }

    public IEnumerator VisualEffect()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < lista.Count; j++)
            {
                number.text = lista[j];
                yield return new WaitForSeconds(0.05f); 
            }
        }
        PrintRandomNumber();
    }

   private void PrintRandomNumber()
    {
        int randomNum = UnityEngine.Random.Range(1, 7); // Genera un número aleatorio entre 1 y 6
        number.text = randomNum.ToString();
    }
}
