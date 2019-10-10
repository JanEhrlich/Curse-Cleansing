using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodScreen : MonoBehaviour
{
    private GameObject gm;
    private SystemMainCharacterMovement mainCharacterMovement;

    private bool once = true;
    private float timer = 0;
    private float disableTime = 2;

    // Start is called before the first frame update
    void Awake()
    {
        if (once)
        {
            gm = GameObject.Find("GameLogic");
            mainCharacterMovement = gm.GetComponent<SystemMainCharacterMovement>();
            mainCharacterMovement.registerBloodScreen(this.gameObject);
            this.gameObject.SetActive(false);
            once = false;
        }
    }

    private void OnEnable()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > disableTime)
        {
            this.gameObject.SetActive(false);
        }
    }
}
