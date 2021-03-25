using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    /*
    more possible stats:

    times stuck?

    most attempted level: level name + restarts (death + restarts) in level
    */

    // Static Stats
    public static float totalTime = 0;
    public static int deaths = 0;
    public static int restarts = 0;

    public static int wallCollisions = 0;
    public static int debrisCollisions = 0;
    public static int asteroidCollisions = 0;

    public static int debrisDestroyed = 0;
    public static int asteroidsDestroyed = 0;
    public static int flamingAsteroidsDestroyed = 0;

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

    // Game Stats

    public void addLevelTimeToTotal() {
        totalTime += timer;
    }

    public static void resetStats() {
        totalTime = 0;
        deaths = 0;
        restarts = 0;

        wallCollisions = 0;
        debrisCollisions = 0;
        asteroidCollisions = 0;

        debrisDestroyed = 0;
        asteroidsDestroyed = 0;
        flamingAsteroidsDestroyed = 0;
    }

    /*
    totalTime
    deaths
    restarts

    totalCollisions
    wallCollisions
    debrisCollisions
    asteroidCollisions
    flamingAsteroidCollisions

    totalObjectsDestroyed
    debrisDestroyed
    asteroidsDestroyed
    flamingAsteroidsDestroyed

    most attempted level: level name + restarts (death + restarts) in level
    */
}
