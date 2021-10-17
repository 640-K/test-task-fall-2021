using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [Header("Points")]
    public Transform A;
    public Transform B;

    [Header("Objects")]
    public List<GameObject> spawnObjects;

    [Space]
    [Space]

    public List<GameObject> instanciatedObjects;

    [Space]
    [Space]

    public SpawnAreaTypes type;

    [DrawIf("type", SpawnAreaTypes.multipleObjectsByOrder, ComparisonType.GreaterOrEqual)]
    public int from;

    [DrawIf("type", SpawnAreaTypes.multipleObjectsByOrder, ComparisonType.GreaterOrEqual)]
    public int to;

    [DrawIf("type", SpawnAreaTypes.oneObjectMultipleTimes, ComparisonType.Equals)]
    public int multipleObjectIndex;

    [DrawIf("type", SpawnAreaTypes.oneObjectMultipleTimes, ComparisonType.Equals)]
    public int objectCount;

    [DrawIf("type", SpawnAreaTypes.oneObject, ComparisonType.Equals)]
    public int objectIndex;


    public void OnDrawGizmos()
    {
        if (A != null && B != null)
        {
            Gizmos.DrawLine(A.position, new Vector3(A.position.x - (A.position.x - B.position.x), A.position.y, A.position.z));
            Gizmos.DrawLine(new Vector3(A.position.x - (A.position.x - B.position.x), A.position.y, A.position.z), B.position);
            Gizmos.DrawLine(B.position, new Vector3(B.position.x - (B.position.x - A.position.x), B.position.y, B.position.z));
            Gizmos.DrawLine(new Vector3(B.position.x - (B.position.x - A.position.x), B.position.y, B.position.z), A.position);
        }
    }

    public void Spawn()
    {
        if (type == SpawnAreaTypes.oneObject)
        {
            float xSpawnPos = Random.Range(Mathf.Min(A.position.x, B.position.x), Mathf.Max(A.position.x, B.position.x));
            float ySpawnPos = Random.Range(Mathf.Min(A.position.y, B.position.y), Mathf.Max(A.position.y, B.position.y));

            GameObject sObject = Instantiate(spawnObjects[objectIndex]);
            sObject.transform.position = new Vector2(xSpawnPos, ySpawnPos);
            instanciatedObjects.Add(sObject);
        }
        else if(type == SpawnAreaTypes.allRandom)
        {
            for(int i = 0; i < spawnObjects.Count; i++)
            {
                int rnd = Random.Range(0, 1);

                if(rnd == 1) 
                {
                    float xSpawnPos = Random.Range(Mathf.Min(A.position.x, B.position.x), Mathf.Max(A.position.x, B.position.x));
                    float ySpawnPos = Random.Range(Mathf.Min(A.position.y, B.position.y), Mathf.Max(A.position.y, B.position.y));

                    GameObject sObject = Instantiate(spawnObjects[i]);
                    sObject.transform.position = new Vector2(xSpawnPos, ySpawnPos);
                    instanciatedObjects.Add(sObject);
                }
            }
        }
        else if(type == SpawnAreaTypes.multipleObjectsByOrder)
        {
            for (int i = from; i < Mathf.Min(to, spawnObjects.Count); i++)
            {
                float xSpawnPos = Random.Range(Mathf.Min(A.position.x, B.position.x), Mathf.Max(A.position.x, B.position.x));
                float ySpawnPos = Random.Range(Mathf.Min(A.position.y, B.position.y), Mathf.Max(A.position.y, B.position.y));

                GameObject sObject = Instantiate(spawnObjects[i]);
                sObject.transform.position = new Vector2(xSpawnPos, ySpawnPos);
                instanciatedObjects.Add(sObject);
            }
        }
        else if (type == SpawnAreaTypes.multipleObjectsByRandom)
        {
            for (int i = from; i < Mathf.Min(to, spawnObjects.Count); i++)
            {
                float xSpawnPos = Random.Range(Mathf.Min(A.position.x, B.position.x), Mathf.Max(A.position.x, B.position.x));
                float ySpawnPos = Random.Range(Mathf.Min(A.position.y, B.position.y), Mathf.Max(A.position.y, B.position.y));

                GameObject sObject = Instantiate(spawnObjects[Random.Range(0, spawnObjects.Count - 1)]);
                sObject.transform.position = new Vector2(xSpawnPos, ySpawnPos);
                instanciatedObjects.Add(sObject);
            }
        }
        else if (type == SpawnAreaTypes.oneObjectMultipleTimes)
        {
            for (int i = 0; i < objectCount; i++)
            {
                float xSpawnPos = Random.Range(Mathf.Min(A.position.x, B.position.x), Mathf.Max(A.position.x, B.position.x));
                float ySpawnPos = Random.Range(Mathf.Min(A.position.y, B.position.y), Mathf.Max(A.position.y, B.position.y));

                GameObject sObject = Instantiate(spawnObjects[multipleObjectIndex], transform);
                sObject.transform.position = new Vector2(xSpawnPos, ySpawnPos);
                instanciatedObjects.Add(sObject);
            }
        }
    }


}

public enum SpawnAreaTypes
{
    allRandom,
    oneObject,
    oneObjectMultipleTimes,
    multipleObjectsByOrder,
    multipleObjectsByRandom,
}
