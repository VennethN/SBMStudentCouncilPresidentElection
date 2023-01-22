using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    public Animator canvasAnimator;
    public List<Sprite> backSprites;
    public PaslonHandler[] paslonHandlers;
    public Transform paslonHolders;
    public GameObject[] allHideableObjects;
    public GameObject revealButton;
    public GameObject cancelButton;
    public GameObject fullScreenButton;
    public Image coreImage;
    public TMP_Text coreText;
    public TMP_Text mainText;
    public CanvasGroup handlersCanvasGroup;
    public CanvasGroup coreCanvasGroup;
    public bool UIVisible;
    public bool countFinished;
    public bool revealClicked;
    public bool highlightActivate;
    public Image backgroundImage;
    public float highlightAnimTime;
    private void Awake()
    {
        countFinished = false;
        revealButton.SetActive(false);
        cancelButton.SetActive(false);
        instance = this;
    }

    public void toggleUI()
    {
        if (countFinished) { return; }
        UIVisible = !UIVisible;
        updateUI();
    }
    public void changeSprite()
    {
        int randIndex = UnityEngine.Random.Range(0, backSprites.Count);
        backgroundImage.sprite = backSprites[randIndex];
    }
    public void finishedCounting()
    {
        if (!Input.GetKey(KeyCode.F)) { return; }
        revealButton.SetActive(true);
        cancelButton.SetActive(true);
        countFinished = true;
        UIVisible = false;
        updateUI();
        foreach (PaslonHandler ps in paslonHandlers)
        {
            ps.randomizeVoteText();
        }
    }
    public void saveVotes()
    {
        string path = Application.persistentDataPath + "/votesData.txt";
        string votesLog = Application.persistentDataPath + "/logVotesData.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        StreamWriter writer2 = new StreamWriter(votesLog, true);

        for (int i =0;i<paslonHandlers.Length;i++)
        {
            writer.WriteLine("Paslon "+(i+1)+": "+paslonHandlers[i].votes);
            writer2.WriteLine("Paslon " + (i + 1) + ": " + paslonHandlers[i].votes);
        }

        writer.Close();
        writer2.Close();
    }

    public void updateUI()
    {
        for(int i = 0; i < allHideableObjects.Length; i++)
        {
            allHideableObjects[i].SetActive(UIVisible);
        }
    }
    public void revealClickedTrue()
    {
        revealClicked = true;
        revealButton.SetActive(false);
        cancelButton.SetActive(false);
        fullScreenButton.SetActive(true);
        mainText.text = "Vote Results";
        int max = -999;
        int maxID = 0;
        for(int i = 0; i < paslonHandlers.Length; i++)
        {
            if(paslonHandlers[i].votes > max)
            {
                max = paslonHandlers[i].votes;
                maxID = i;
            }
        }
        highLightSet(maxID);
    }
    public void revealVotes()
    {
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftControl))
        {
            revealClickedTrue();
        }
        else {
            canvasAnimator.SetTrigger("revealTrigger");
        }
    }
    public void cancelReveal()
    {
        cancelButton.SetActive(false);
        revealButton.SetActive(false);
        countFinished = false;
        UIVisible = true;
        updateUI();
    }
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            toggleUI();
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            tlHL();
        }
        for (int i =0;i<paslonHandlers.Length;i++)
        {
            if (Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), "Alpha" + (i + 1)))) {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    paslonHandlers[i].addVote(-1);
                }
                else if(Input.GetKey(KeyCode.LeftAlt) )
                {
                    highLightSet(i);
                }
                else
                {
                    paslonHandlers[i].addVote(1);
                }
            }
        }
    }
    public void toggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    private void Start()
    {
        paslonHandlers = paslonHolders.GetComponentsInChildren<PaslonHandler>();
        allHideableObjects = GameObject.FindGameObjectsWithTag("ToHide");
        UIVisible = false;
        updateUI();
        highLightSet(0);
    }
    public void highLightSet(int setID)
    {
        coreImage.sprite = paslonHandlers[setID].coreImage.sprite;
        if (paslonHandlers[setID].showDisabled){
            coreText.text = "";
        }else
        {
            coreText.text = paslonHandlers[setID].votes.ToString();
        }
    }
    public void tlHL()
    {
        highlightActivate = !highlightActivate;
        toggleHighlight();
    }
    public void toggleHighlight()
    {
        LeanTween.alphaCanvas(handlersCanvasGroup, highlightActivate ? 0 : 1, highlightAnimTime).setEase(LeanTweenType.easeInOutExpo);
        LeanTween.alphaCanvas(coreCanvasGroup, highlightActivate ? 1 : 0, highlightAnimTime).setEase(LeanTweenType.easeInOutExpo);
    }
    public void disableAllCounter()
    {
        foreach (PaslonHandler ps in paslonHandlers)
        {
            ps.showDisabled = true;
        }
    }
    public void enableAllCounters()
    {
        if(!Input.GetKey(KeyCode.S)) { return; }
        foreach (PaslonHandler ps in paslonHandlers)
        {
            ps.showDisabled = false;
        }
    }
    public void resetAllVotes()
    {
        foreach (PaslonHandler ps in paslonHandlers)
        {
            ps.resetVote();
        }
    }
}
