using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    private Vector3 initialPlayerPosition;
    private GameObject playerObject;

    private void Start()
    {
        // Encuentra el objeto del jugador por su nombre
        playerObject = GameObject.Find("Player");

        if (playerObject != null)
        {
            // Guarda la posición inicial del jugador al inicio del juego
            initialPlayerPosition = playerObject.transform.position;
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == playerObject) // Comprueba si el otro objeto es el jugador
        {
            Debug.Log("El jugador ha tocado el collider");
            AnimacionMuerte();
            // Devuelve al jugador a su posición inicial
            playerObject.transform.position = initialPlayerPosition;
        }
    }
}
