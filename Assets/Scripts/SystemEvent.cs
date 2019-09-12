using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

/*
 * Checks for current Player relevant events in the game
 * TODO:
 *  -Set ComponentMainCharacterState if new Curse Ability was received
 *  -Set ComponentProgression to current point of the game, e.g. Level 1 Part 4
 */
public class SystemEvent : MonoBehaviour
{
    SystemGameMaster gameMaster;
    GameObject gameLogic;

    public GameObject[] startEnemies;

    public GameObject[] enemySpawns;
    private List<Action>[] enemySpawnsActions;
    public GameObject[] interactions;
    private List<Action>[] interactionsActions;
    private Dictionary<GameObject, int> enabledInteractions = new Dictionary<GameObject, int>();
    public GameObject[] triggers;
    private List<Action>[] triggersActions;
    public GameObject[] respawnPoints;
    private List<Action>[] respawnActions;


    private int numberRespawn = 0;
    public bool playerDied = false;
    private ComponentScene lastRespawnState = new ComponentScene();
    public ComponentScene currentState = new ComponentScene();
    ContactFilter2D filter = new ContactFilter2D();

    private Collider2D[] overlap = new Collider2D[10];

    private void Awake()
    {
        gameLogic = GameObject.Find("GameLogic");


        //Debug.Log(enemySpawns.Length);
        enemySpawnsActions = Enumerable.Range(0, enemySpawns.Length).Select((i) => new List<Action>()).ToArray();
        interactionsActions = Enumerable.Range(0, interactions.Length).Select((i) => new List<Action>()).ToArray();
        triggersActions =  Enumerable.Range(0, triggers.Length).Select((i) => new List<Action>()).ToArray();
        respawnActions = Enumerable.Range(0, respawnPoints.Length).Select((i) => new List<Action>()).ToArray();

        //Debug.Log(enemySpawnsActions[0].Count);

        currentState.spawnedEnemies = new List<GameObject>();

        gameMaster = gameLogic.GetComponent<SystemGameMaster>();
        filter.useTriggers = false;
        filter.SetLayerMask(1<<LayerMask.NameToLayer("Player"));
        filter.useLayerMask = true;

    }
    private void FixedUpdate()
    {
        //do nothing until the this is the same scene

        //Debug.Log(""+ triggersActions[0].Count);
        for (int i = 0; i < triggers.Length; i++)
        {
            if (triggers[i].GetComponent<BoxCollider2D>().OverlapCollider(filter,overlap) == 0) continue;
            foreach (Action func in triggersActions[i])
            {
                func();
            }
        }

        foreach (KeyValuePair<GameObject,int> item in enabledInteractions)
        {
            if(item.Key.GetComponent<BoxCollider2D>().OverlapCollider(filter, overlap) == 0) continue;
            foreach (Action func in interactionsActions[item.Value])
            {
                func();
            }
        }
        
    }


    #region addFunctions

    public void AddActionTrigger(Action func, int id)
    {
        triggersActions[id].Add(func);
    }

    public void AddActionEnemySpawn(Action func, int id)
    {
        enemySpawnsActions[id].Add(func);
    }

    public void AddActionInteraction(Action func, int id)
    {
        interactionsActions[id].Add(func);
    }

    public void AddActionrespawnPoint(Action func, int id)
    {
        respawnActions[id].Add(func);
    }

    #endregion

    #region getter
    public GameObject getEnemySpawn(int id)
    {
        return enemySpawns[id];
    }

    public GameObject getTrigger(int id)
    {
        return triggers[id];
    }

    public GameObject getInteraction(int id)
    {
        return interactions[id];
    }

    public GameObject getRespawnpoint(int id)
    {
        return respawnPoints[id];
    }
    #endregion

    #region callFunctionManually
    public void CallEnemySpawnAction(int id)
    {
        foreach (Action func in enemySpawnsActions[id])
        {
            func();
        }
    }

    public void CallInteractionAction(int id)
    {
        foreach (Action func in interactionsActions[id])
        {
            func();
        }
    }

    public void CallRespawnfunction(int id)
    {
        foreach (Action func in respawnActions[id])
        {
            func();
        }
    }

    public void EnableInteraction(int id)
    {
        interactions[id].SetActive(true);
        enabledInteractions.Add(interactions[id], id);
    }

    public void DisableInteraction(int id)
    {
        interactions[id].SetActive(false);
        enabledInteractions.Remove(interactions[id]);
    }

    public void EnableTrigger(int id)
    {
        triggers[id].SetActive(true);
        enabledInteractions.Add(triggers[id], id);
    }

    public void DisableTrigger(int id)
    {
        triggers[id].SetActive(false);
        enabledInteractions.Remove(triggers[id]);
    }

    #endregion
}
