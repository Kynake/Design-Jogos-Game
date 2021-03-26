using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour
{
    private void OnCarregarFase(InputValue input)
    {
        // Reset Stats when loading first level
        GameController.resetStats();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
