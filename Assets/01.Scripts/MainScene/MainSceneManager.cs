using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField]
    private CanvasGroup titleGroup;

    [SerializeField]
    private CanvasGroup tutorialGroup;

    [SerializeField]
    private CanvasGroup stageGroup;

    private bool exitTitle;
    private bool tutorialing;

    private void Start(){
        if(PlayerPrefs.GetInt("Retry", 0) == 1){
            PlayerPrefs.SetInt("Retry", 0);
            OpenStageGroup();
        } else {
            OpenTitleGroup();
        }
    }

    public void OpenTitleGroup(){
        titleGroup.gameObject.SetActive(true);
        if(exitTitle){
            Tween openTween = stageGroup.DOFade(0.0f, 0.25f);

            openTween.OnComplete(() => {
                stageGroup.gameObject.SetActive(false);
                titleGroup.DOFade(1.0f, 0.25f);
            });
        } else {
            exitTitle = true;
            Tween openTween = titleGroup.DOFade(1.0f, 0.25f);
        }
    }

    public void OpenTutorial(){
        Tween openTween = titleGroup.DOFade(0.0f, 0.25f);
        openTween.OnComplete(() => {
            titleGroup.gameObject.SetActive(false);
            tutorialGroup.gameObject.SetActive(true);
            tutorialGroup.DOFade(1.0f, 0.25f);

            tutorialing = true;
        });
    }

    public void OpenStageGroup(){
        Tween openTween = tutorialGroup.DOFade(0.0f, 0.25f);
        openTween.OnComplete(() => {
            tutorialing = false;

            tutorialGroup.gameObject.SetActive(false);
            stageGroup.gameObject.SetActive(true);
            stageGroup.DOFade(1.0f, 0.25f);
        });
    }

    public void GameStart(){
        SceneManager.LoadScene("Game");
    }

    private void Update(){
        if(tutorialing){
            if(Input.anyKeyDown){
                OpenStageGroup();
            }
        }
    }
}
