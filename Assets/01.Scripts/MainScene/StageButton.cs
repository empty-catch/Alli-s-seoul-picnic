using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageButton : MonoBehaviour
{
    [SerializeField]
    private GameObject lightObject;
    public static List<StageButton> stageButtons = new List<StageButton>();
    private Vector3 defaultScale;
    private Vector3 downScale;
    private Vector3 upScale;

    public static StageButton selectButton;
    
    private void Awake(){
        StageButton.stageButtons.Add(this);
    }

    private void Start(){
        defaultScale = gameObject.transform.localScale;
        downScale = defaultScale * 0.9f;
        upScale = defaultScale * 1.1f;
    }

    public static void AllDown(){
        stageButtons.ForEach((item) => {
            if(item != StageButton.selectButton){
                item.DownScale();
            }
        });
    } 
    
    public void ChangeScale(){
        lightObject.SetActive(true);
        gameObject.transform.DOScale(upScale, 0.5f);
    }

    public void DownScale(){
        lightObject.SetActive(false);
        gameObject.transform.DOScale(downScale, 0.5f);
    }

    public void SetButton(int index){
        PlayerPrefs.SetInt("Difficulty", index);
        StageButton.selectButton = this;
        StageButton.AllDown();

        ChangeScale();
    }

    public static void Reset(){
        StageButton.stageButtons.ForEach((item) => {
            item.DownScale();
        });
    }

    private void OnDestroy(){
        StageButton.stageButtons.Clear();
        StageButton.selectButton = null;
    }
}
