using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private UndergroundBehavior undergroundBehavior;
    private int numberOfSpawners;
    private int maxNumberOfSpawners;
    private int numberOfAliveZombies = 0;
    private int numberOfKilledZombies = 0;

    public void AddSpawner()
    {
        numberOfSpawners++;
        maxNumberOfSpawners = numberOfSpawners;
    }

    public void RemoveSpawner()
    {
        numberOfSpawners--;
        undergroundBehavior.SetSpawnersText(numberOfSpawners, maxNumberOfSpawners);

        if (numberOfSpawners == 0)
        {
            undergroundBehavior.DisplayWonGameCanvas();
        }
    }

    public void AddAliveZombie()
    {
        numberOfAliveZombies++;
        undergroundBehavior.SetAliveZombiesText(numberOfAliveZombies);
    }

    public void RemoveAliveZombie()
    {
        numberOfAliveZombies--;
        undergroundBehavior.SetAliveZombiesText(numberOfAliveZombies);
    }

    public void AddKilledZombie()
    {
        numberOfKilledZombies++;
        undergroundBehavior.SetKilledZombiesText(numberOfKilledZombies);
    }

}
