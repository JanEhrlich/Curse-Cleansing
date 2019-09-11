using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentScene
{
    public bool[] enemySpawns;
    public bool[] interactions;
    public bool[] trigger;
    public bool[] respawnpoints;

    public int lastRespawnpoint = 0;

    public List<GameObject> spawnedEnemies;
}
