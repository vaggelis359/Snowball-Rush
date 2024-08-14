using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    private Vector3 spawnPos = new Vector3(0, -8.1f, 1000);
    public float startDelay = 1;
    public float repeatRate = 0.5f;                             // Time in seconds
    private float obstacleSpacing = 20f;
    private float currentZPosition = 1000f;

    public GameObject treesObstacle;
    private float treesSpawnCount;

    public GameObject molotovObstacle;
    // Check if SnowBall is big so we can spawn the molotovs
    public GameObject checkObject;                              // snowBall
    private float desiredScale = 30f;                           // The scale we want
    //private bool molotovObstacleAvailable = false;              // Check if molotov is available
    private float lastMolotovTime = -10f;                       // Initial value to allow first spawn immediately

    public TextMeshProUGUI scoreText;

    private float[] lanePositions = new float[] { -15f, 0f, 15f };

    void Start()
    {
        StartCoroutine(SpawnObstacleRoutine());
    }

    IEnumerator SpawnObstacleRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            float currentScore = GetScoreFromText();
            if (currentScore < 1000)
            {
                SpawnStage01();
                yield return new WaitForSeconds(4.5f);

            }
            else if (currentScore >= 5000)
            {
                if (currentScore < 6000) { SpawnStage01(); yield return new WaitForSeconds(2.5f); }
                else if (currentScore >= 9000) { SpawnStage02(); yield return new WaitForSeconds(2.5f); }
                else { SpawnStage01(); yield return new WaitForSeconds(2.5f); }
            }
            else
            {
                SpawnStage02();
                yield return new WaitForSeconds(2.5f);
            }
        }
    }

    List<int> spawnOfObjects()
    {
        List<int> usedLanes = new List<int>();
        while (usedLanes.Count < 2)
        {
            int laneIndex = Random.Range(0, lanePositions.Length);
            if (!usedLanes.Contains(laneIndex))
            {
                usedLanes.Add(laneIndex);
            }
        }
        return usedLanes;
    }
    void SpawnStage01()                                                                                     // First stage with 2 obstacles
    {
        for (float z = currentZPosition; z > 800; z -= obstacleSpacing)
        {
            List<int> usedLanes = spawnOfObjects();
            int numberOfObstacles = Random.Range(1, 2);                                                     // Spawn either 1 or 2 obstacles
            for (int i = 0; i < numberOfObstacles; i++)
            {
                if (usedLanes.Count > 0)
                {
                    int laneIndex = usedLanes[i % usedLanes.Count];
                    float x = lanePositions[laneIndex] + Random.Range(-1f, 1f);
                    SpawnObstacles(x, z);
                }
            }
        }

    }
    void SpawnStage02()                                                                                     // Tree stage 
    {
        List<int> usedLanes = spawnOfObjects();
        HashSet<int> treeLanes = new HashSet<int>();
        for (float z = currentZPosition; z > 800; z -= obstacleSpacing)
        {
            foreach (int laneIndex in usedLanes)
            {
                float x = lanePositions[laneIndex] + Random.Range(-1f, 1f);
                SpawnTreeObstacle(x, z);
                treeLanes.Add(laneIndex);
            }
            if (checkObject.transform.localScale.x > desiredScale && Time.time - lastMolotovTime >= 10f)    //Molotov boolean
            {
                int molotovLaneIndex;
                do
                {
                    molotovLaneIndex = Random.Range(0, lanePositions.Length);
                } while (treeLanes.Contains(molotovLaneIndex));

                float molotovX = lanePositions[molotovLaneIndex] + Random.Range(-1f, 1f);
                SpawnMolotovObstacle(molotovX, z);
                lastMolotovTime = Time.time;                                                                // Update the last spawn time
            }
        }
    }


    void SpawnObstacles(float xPos, float zPos)
    {
        Vector3 spawnPosition = new Vector3(xPos, spawnPos.y, zPos);
        int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
        Instantiate(obstaclePrefabs[obstacleIndex], spawnPosition, Quaternion.identity);
    }

    void SpawnTreeObstacle(float xPos, float zPos)
    {
        Vector3 spawnPosition = new Vector3(xPos, spawnPos.y, zPos);
        Instantiate(treesObstacle, spawnPosition, Quaternion.identity);
    }

    void SpawnMolotovObstacle(float xPos, float zPos)
    {
        Vector3 spawnPosition = new Vector3(xPos, spawnPos.y, zPos);
        Instantiate(molotovObstacle, spawnPosition, Quaternion.identity);
    }

    int GetScoreFromText()
    {
        if (scoreText != null)
        {
            string scoreString = scoreText.text.Replace("Score: ", "");
            if (int.TryParse(scoreString, out int score))
            {
                return score;
            }
        }
        return 0;
    }
}

