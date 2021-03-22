using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    private void OnCarregarFase(InputValue input)
    {
        Debug.Log("OnCarregarFase");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
