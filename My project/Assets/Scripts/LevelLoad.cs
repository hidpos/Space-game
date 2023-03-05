
using UnityEngine;

public class LevelLoad : MonoBehaviour
{
    public float speed = 2500;
    private Transform lastChild;
    public GameObject parent;
    public GameController controller;
    public Material[] skyboxes;  
    
    // for asteroid controller script
    public void Init(Material[] skyb, float sp, GameController cont)
    {
        skyboxes = skyb; speed = sp; controller = cont;
    }
    public void Init(float sp, GameController cont)
    {
        speed = sp; controller = cont;
    }

    void Start()
    {
        // adding this script to every child, changing tag to avoid unnecessary scripts
        if (tag == "parent")
        {
            lastChild = transform.GetChild(transform.childCount - 1);
            Transform[] children = GetComponentsInChildren<Transform>();
            for (int i = 1; i < children.Length; i++)
            {
                children[i].gameObject.AddComponent<LevelLoad>();
                children[i].gameObject.GetComponent<LevelLoad>().lastChild = lastChild;
                children[i].gameObject.GetComponent<LevelLoad>().parent = this.gameObject;
                children[i].gameObject.GetComponent<LevelLoad>().controller = this.controller;
            }
            tag = "empty";
        }
    }

    private void OnTriggerEnter(Collider other) {
        // in dead zone - move after last block
        // selfdestroying if asteroid or teleport
        // also changing last block in parent script
        if (other.gameObject.tag == "deadzone")
        {
            if (gameObject.tag == "asteroid" || gameObject.tag == "teleport")
            {
                controller.score++;
                Destroy(this.gameObject);
                return;
            }
            var pos = parent.GetComponent<LevelLoad>().lastChild.transform.position.z + 3;
            transform.position = new Vector3(0, 0, pos);
            parent.GetComponent<LevelLoad>().lastChild = this.gameObject.GetComponent<Transform>();
        }

        // changing skybox if spaceship going through teleport
        if (other.gameObject.tag == "spaceship")
        {
            if (gameObject.tag == "teleport")
            {
                var rand = new System.Random();
                RenderSettings.skybox = skyboxes[rand.Next(0, skyboxes.Length)]; // random skybox
                RenderSettings.skybox.SetFloat("_Rotation", rand.Next(1, 361)); // and random rotation for every load
                var a = rand.Next(1,3);
                if (a == 1)
                {
                    MeshRenderer[] meshes = controller.floor.GetComponentsInChildren<MeshRenderer>();
                    for (int i = 0; i < meshes.Length; i++)
                        meshes[i].enabled = false;
                }
                else
                {
                    MeshRenderer[] meshes = controller.floor.GetComponentsInChildren<MeshRenderer>();
                    for (int i = 0; i < meshes.Length; i++)
                        meshes[i].enabled = true;
                }
                return;
            }   

            // if asteroid touches spaceship than end game
            controller.GameOver();
        }
    }

    void Update()
    {
        // constant moving & changing speed if boost is enabled
        if (tag != "parent")
        {
            if (controller.isBoostEnable)
                speed = 100;
            else
                speed = 150;
            transform.position -= new Vector3(0, 0, 10 / speed);
        }
    }
}
