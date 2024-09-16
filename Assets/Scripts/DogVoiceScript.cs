using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
public class DogVoiceScript : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    public Text scoreText, commentaires, finalScore;
    public CharacterController controller;
    public float ActSpeed, Speed = 0f;
    public float horizontal;
    public float vertical;
    public Animator animator;
    public AudioSource Breack;

    Vector3 vector3 = Vector3.forward;
    private Vector3[] orientations = new Vector3[] { Vector3.forward, Vector3.right, -Vector3.forward, Vector3.left };
    public string[] Demande = new string[] { "devant", "droite", "tourne", "gauche" };

    public Vector3[][] matrix = new Vector3[][] {
        new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward },
        new Vector3[] { Vector3.right, -Vector3.forward, Vector3.left, Vector3.forward },
        new Vector3[] { -Vector3.forward, Vector3.left, Vector3.forward, Vector3.right },
        new Vector3[] { Vector3.left, Vector3.forward, Vector3.right, -Vector3.forward }
    };

    public float timeValue = 180;
    public Text timeText;
    public GameObject pauseMenu, howPanel, fin;
    public bool destroyed = false;

    private void Start()
    {
        actions.Add("devant", Front);
        actions.Add("bouge", Move);
        actions.Add("avance", Move);
        actions.Add("tourne", Turn);
        actions.Add("droite", Right);
        actions.Add("gauche", Left);
        actions.Add("stop", Stop);
        actions.Add("plus vite", Fast);
        actions.Add("vite", Fast);
        actions.Add("moin vite", Slow);
        actions.Add("pause", Pause);
        actions.Add("quitter", Quit);
        actions.Add("continue", Resume);
        actions.Add("Comment jouer", How);
        actions.Add("retour", Back);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += ReconizedSpeech;
        keywordRecognizer.Start();
    }
    private void ReconizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        commentaires.text = speech.text;
        actions[speech.text].Invoke();
    }
    private void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
        }
        DisplayTime(timeValue);
        if (timeValue == 0)
        {
            Time.timeScale = 0f;
            fin.SetActive(true);
        }
        if (Speed > 1f)
        {
            ActSpeed = Speed;
        }
        horizontal = vector3.z;
        vertical = vector3.x;
        PlayerMove();
    }

    void PlayerMove()
    {
        Vector3 Direction = new Vector3(vertical, 0f, horizontal).normalized;

        if (Direction.magnitude > 0.1f)
        {
            controller.Move(Direction * Speed * Time.deltaTime);
        }
        animator.SetFloat("Speed", Speed);
    }
    
    public Vector3 GetVectorFromMatrix(Vector3 currentOrientation, string action)
    {
        int x = System.Array.IndexOf(orientations, currentOrientation);
        int y = System.Array.IndexOf(Demande, action);
        return matrix[y][x];
    }
    // MOVE
    private void Front()
    {
        vector3 = GetVectorFromMatrix(vector3, "devant");
        transform.rotation = Quaternion.LookRotation(vector3);
        Debug.Log(transform.localEulerAngles.y);
    }
    private void Turn()
    {
        vector3 = GetVectorFromMatrix(vector3, "tourne");
        transform.rotation = Quaternion.LookRotation(vector3);
        Debug.Log(transform.localEulerAngles.y);
    }
    private void Right()
    {
        vector3 = GetVectorFromMatrix(vector3, "droite");
        transform.rotation = Quaternion.LookRotation(vector3);
        Debug.Log(transform.localEulerAngles.y);
    }
    private void Left()
    {
        vector3 = GetVectorFromMatrix(vector3, "gauche");
        transform.rotation = Quaternion.LookRotation(vector3);
        Debug.Log(transform.localEulerAngles.y);
    }
    private void Stop()
    {
        Speed = 0;
    }
    private void Fast()
    {
        if (Speed > 0)
        {
            Speed += 2f;
        }
    }
    private void Slow()
    {
        if (Speed > 0)
        {
            Speed -= 1f;
        }
    }
    private void Move()
    {
        if (Speed == 0)
        {
            Speed = 2;
        }
    }
    private void Setspeed()
    {
        Speed = ActSpeed;
    }
    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void Quit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1f;
    }
    public void Resume()
    {
        if (pauseMenu == true)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void How()
    {
        pauseMenu.SetActive(false);
        howPanel.SetActive(true);
    }
    public void Back()
    {
        if (howPanel == true)
        {
            pauseMenu.SetActive(true);
            howPanel.SetActive(false);
        }
    }
}
