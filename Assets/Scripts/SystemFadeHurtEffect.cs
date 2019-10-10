using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemFadeHurtEffect : MonoBehaviour
{
     SystemGameMaster gameMaster;
     ComponentMainCharacterState componentMainCharacterState;
     public float FadeRate;
     private Image image;

     //tmp
     private float targetAlpha;
     int lastHealth = 0;
     Color curColor;
     bool inFade = false;
     float timeForFadeOut = 0f;
     float timeToFade = 0.3f;
     bool isFading = false;

     // Use this for initialization
     void Start () {
         gameMaster = GameObject.Find("GameLogic").GetComponent<SystemGameMaster>();
         this.image = this.GetComponent<Image>();
         componentMainCharacterState = gameMaster.ComponentMainCharacterState;
         lastHealth = componentMainCharacterState.health;
         if(this.image==null)
         {
             Debug.LogError("Error: No image on "+this.name);
         }
         this.targetAlpha = this.image.color.a;
     }
     
     // Update is called once per frame
     void Update () {
         curColor = this.image.color;
         float alphaDiff = Mathf.Abs(curColor.a-this.targetAlpha);
         if (alphaDiff>0.0001f)
         {
             curColor.a = Mathf.Lerp(curColor.a,targetAlpha,this.FadeRate*Time.deltaTime);
             this.image.color = curColor;
         }


     }

     void FixedUpdate(){
         if(lastHealth != componentMainCharacterState.health){
             lastHealth = componentMainCharacterState.health;
             timeForFadeOut = Time.time + timeToFade;
             isFading = true;
             FadeIn();
         }

         if(isFading && timeForFadeOut < Time.time){
             isFading = false;
             FadeOut();
         }


     }
 
     public void FadeOut()
     {
         this.targetAlpha = 0.0f;
     }
 
     public void FadeIn()
     {
         this.targetAlpha = 1.0f;
     }
}
