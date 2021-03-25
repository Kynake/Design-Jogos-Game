using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuFinal : MonoBehaviour
{
    private void OnCarregarFase(InputValue input)
    {
        SceneManager.LoadScene(0);
    }

}
