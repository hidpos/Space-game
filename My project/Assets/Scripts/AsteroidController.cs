using System.Collections;
using UnityEngine;
public class AsteroidController : MonoBehaviour
{
    public GameObject asteroid, teleport;
    public GameController controller;
    private GameObject[] asteroidLine = new GameObject[5];
    public float spawnDelay = .7f;
    private float[] coordinates = new float[5] {-1, -0.5f, 0, 0.5f, 1 }; // coordinates for asteroids
    public Material[] skyboxes;  

    void Start()
    {
        StartCoroutine(CreateLines());
    }
    IEnumerator CreateLines()
    {
        while (true)
        {
            var rand = new System.Random();
            var astr = 0;
            // teleport creating
            if (rand.Next(1, 6) == 1)
            {
                GameObject tp = Instantiate(teleport, new Vector3(-0.43f, 1.59f, 57), new Quaternion());
                tp.AddComponent<LevelLoad>();
                tp.GetComponent<LevelLoad>().Init(skyboxes, 150, controller);
                tp.tag = "teleport";
            }

            // asteroid line generation
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if(rand.Next(1, 3) == 1 && astr != 3)
                    {
                        asteroidLine[i] = Instantiate(asteroid, new Vector3(coordinates[i], 1, 57), new Quaternion());
                        asteroidLine[i].AddComponent<LevelLoad>();
                        asteroidLine[i].GetComponent<LevelLoad>().Init(150, controller);
                        asteroidLine[i].tag = "asteroid";
                        asteroidLine[i].transform.SetParent(this.transform);
                        astr++;
                    }
                }
            }

            yield return new WaitForSeconds(spawnDelay);
        }   
    }
}