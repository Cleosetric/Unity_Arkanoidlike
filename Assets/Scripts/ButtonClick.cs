using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public void OnNextStageClicked(){
        GameManager.Instance.NextStage();
    }
    
    public void OnRestartLevelClicked(){
        GameManager.Instance.RestartGame();
    }
}
