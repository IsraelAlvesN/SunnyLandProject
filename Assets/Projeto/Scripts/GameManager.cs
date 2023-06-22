using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Points
    private int score;
    public Text txtScore;
    //Audio
    public AudioSource fxGame;
    public AudioClip fxCarrot;

    public void Points(int qtdPontos)
    {
        //play a time
        fxGame.PlayOneShot(fxCarrot);
        score += qtdPontos;
        txtScore.text = score.ToString();
    }
}
