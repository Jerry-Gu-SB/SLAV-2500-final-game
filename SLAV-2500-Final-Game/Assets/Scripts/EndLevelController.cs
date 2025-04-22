using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EndLevelController : MonoBehaviour
{
    public Button endLevelButton;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            endLevelButton.gameObject.SetActive(true);
            Debug.Log("Player has reached the end level trigger.");
        }
    }
}
