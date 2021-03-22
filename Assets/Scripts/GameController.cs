using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI marcadorPontuacao;
    public TextMeshProUGUI tempo;
    public TextMeshProUGUI qntMoedas;
    // public string proximaFase;

    [SerializeField]
    private int pontuacao = 0;

    [SerializeField]
    private int moedasNaFase = 0;

    [SerializeField]
    private int moedasColetadas = 0;

    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {

        RocketController.colisaoAcao += MarcarPontoBonus;
        Destructable.Destruido += MarcarPontoBonus;
        GameObject[] moedas = GameObject.FindGameObjectsWithTag("score");
        moedasNaFase = moedas.Length;
        atualizarGUI();
    }

    // Update is called once per frame
    void Update()
    {
        atualizarGUI();
        if (moedasColetadas>= moedasNaFase)
        {
            FimJogo();
        }
    }

    public void MarcarPonto(PontuacaoJogo x)
    {
        pontuacao += (int)x;
    }

    public void MarcarPontoParedes()
    {
        this.MarcarPonto(PontuacaoJogo.PontuacaoParede);
    }

    public void MarcarPontoBonus()
    {
        this.MarcarPonto(PontuacaoJogo.PontuacaoBonus);
    }

    public void MarcarPontoBonus(int ponto)
    {
        pontuacao += ponto;
    }

    private void atualizarGUI()
    {
        marcadorPontuacao.text = $"Pontuação: {pontuacao:D3}";

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

public enum PontuacaoJogo
{
    PontuacaoParede = 5,
    PontuacaoBonus = 15,
}
