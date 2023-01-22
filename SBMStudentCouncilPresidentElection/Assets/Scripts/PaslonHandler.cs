using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class PaslonHandler : MonoBehaviour
{
    public TMP_Text votesCounter;
    public int votes;
    public bool _showDisabled;
    public bool showDisabled
    {
        get { return _showDisabled; }
        set {

            _showDisabled = value;
            if (value == true) { disableVote(); } else { showVotes(); }
        }
    }
    public Image coreImage;
    public Image showSprite;
    public Sprite enabledSprite;
    public Sprite disabledSprite;
    private void Start()
    {
        showDisabled = false;
    }
    public async Task randomizeVoteText()
    {
        votesCounter.text = "?";
        showDisabled = false;
        while (!MainManager.instance.revealClicked)
        {
            votesCounter.text = UnityEngine.Random.Range(1,999).ToString();
            if (MainManager.instance.countFinished == false) {
                showDisabled = true; updateVotes();return;
            }
            await Task.Yield();
        }
        updateVotes();
    }
    public void addVote(int voteAdd)
    {
        if (MainManager.instance.countFinished) { return; }
        if (Mathf.Sign(voteAdd)>=0)
        {
            GameVFXManager.SpawnParticle("Green",transform.position);
            AudioManager.PlayAudioEffect("Success");
        }
        else
        {
            GameVFXManager.SpawnParticle("Red", transform.position);
            AudioManager.PlayAudioEffect("Fail");
        }
        votes = Mathf.Clamp(votes+voteAdd, 0, 99999);
        updateVotes();
        MainManager.instance.saveVotes();
    }
    public void updateVotes()
    {
        votesCounter.text = votes.ToString();
    }
    public void toggleVote()
    {
        showDisabled = !showDisabled;
    }
    public void showVotes()
    {
        Color vc = votesCounter.color;
        vc.a = 1f;
        votesCounter.color = vc;
    }
    public void disableVote()
    {
        Color vc = votesCounter.color;
        vc.a = 0f;
        votesCounter.color = vc;
    }
    public void resetVote()
    {
        if (!Input.GetKey(KeyCode.R)) { return; }
        votes = 0;
        updateVotes();
    }
}
