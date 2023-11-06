using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    private Vector3 initialPlayerPosition;
    private GameObject playerObject;
    private GameObject CheckPoint;

    private void Start()
    {
        // Encuentra el objeto del jugador por su nombre
        playerObject = GameObject.Find("Player");
        CheckPoint = GameObject.Find("Checkpoint");

        if (playerObject != null)
        {
            // Guarda la posición inicial del jugador al inicio del juego
            initialPlayerPosition = CheckPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("No se pudo encontrar el objeto del jugador.");
        }

    }
    IEnumerator AnimacionMuerte()
    {

        yield return new WaitForSeconds(100.2f);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") // Comprueba si el otro objeto es el jugador
        {
            AnimacionMuerte();
            // Devuelve al jugador a su posición inicial
            playerObject.transform.position = initialPlayerPosition;
        }
    }
}
