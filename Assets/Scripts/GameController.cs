using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI tempo;
    public TextMeshProUGUI qntMoedas;

    [SerializeField]
    private int moedasNaFase = 0;

    [SerializeField]
    private int moedasColetadas = 0;

    private float timer = 0;

    void Start()
    {
        GameObject[] moedas = GameObject.FindGameObjectsWithTag("score");
        moedasNaFase = moedas.Length;
        atualizarGUI();
    }

    void Update()
    {
        atualizarGUI();
        if (moedasColetadas>= moedasNaFase)
        {
            FimJogo();
        }
    }

    private void atualizarGUI()
    {
        GameObject[] moedas = GameObject.FindGameObjectsWithTag("score");
        moedasColetadas = moedasNaFase - moedas.Length;
        qntMoedas.text = $"{moedasColetadas:D2}/{moedasNaFase:D2}";

        timer += Time.deltaTime;
        tempo.text = $"{(int) timer / 60:00}:{timer % 60:00.000}";
    }

    private void FimJogo()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1));

        // if (string.IsNullOrEmpty(proximaFase))
        // {
        //     SceneManager.LoadScene("TelaGameOver");
        //     return;
        // }
        // SceneManager.LoadScene(proximaFase);
    }

}
