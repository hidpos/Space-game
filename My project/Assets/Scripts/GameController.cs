using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class GameController : MonoBehaviour
{
    public GameObject floor;

    // ui elements
    public Text score1, score2;
    public Text bestscore1, bestscore2;
    public Text playtime1, playtime2;
    public GameObject newRecordText;

    public int score;
    public GameObject gameOverCanvas;
    public AsteroidController ASTcontroller;
    public SpaceshipController SPCcontroller;
    public float secs = 0;
    private int bestScore;
    public GameObject planets;
    private bool isGameOver = false;
    public bool isBoostEnable = false;

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        
        StartCoroutine(ScoreAddEverySec());

        // creating new record if dont exists
        string path = Application.dataPath + "/record.txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "0");
        }
        bestScore = int.Parse(File.ReadAllText(path));
    }

    public void SetGameOverFalse() // :)
    {
        isGameOver = false;
    }

    IEnumerator ScoreAddEverySec()
    {
        if (!isGameOver)
        {
            if (isBoostEnable)
                score++;
            else
                score += 2;
        }
        yield return new WaitForSeconds(1); 
    }
    IEnumerator SpeedBoost()
    {
        isBoostEnable = true;
        SPCcontroller.smooth = 100;
        ASTcontroller.spawnDelay = .6f;
        yield return new WaitForSeconds(3);
    
        isBoostEnable = false;
        SPCcontroller.smooth = 70;
        ASTcontroller.spawnDelay = .7f;
        yield return new WaitForSeconds(20);
    }
    public void GameOver()
    {
        // if reaching new score, then change it in file
        if (bestScore < score)
        {
            newRecordText.SetActive(true);
            string path = Application.dataPath + "/record.txt";
            File.WriteAllText(path, score.ToString());
            bestScore = score;
        }
        gameOverCanvas.SetActive(true);

        // stopping all scripts and reseting all stats and coordinates
        ASTcontroller.enabled = false;
        SPCcontroller.enabled = false;
        SPCcontroller.gameObject.transform.position = new Vector3(0, 0.77231f, -0.7f);
        secs = 0;
        score = 0;
        isGameOver = true;

        // clearing scene from asteroids
        Transform[] astros = ASTcontroller.gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in astros)
        {
            if (child.gameObject.name != "Asteroids")
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // spaceship boosting on space
        if (Input.GetKeyDown(KeyCode.Space) && !isBoostEnable)
        {
            StartCoroutine(SpeedBoost());
        }

        // environment animation
        planets.transform.position -= new Vector3(0, 0, 0.02f);

        // updating stats
        if (!isGameOver)
        {
            secs += Time.deltaTime;
            score1.text = "score: " + score.ToString();
            score2.text = "score: " + score.ToString();
            bestscore1.text = bestScore.ToString();
            bestscore2.text = bestScore.ToString();
            var seconds = Math.Round(secs, 0, MidpointRounding.AwayFromZero);
            if (secs > 60)
            {
                var mins = (int)seconds / 60;
                var secFin = (mins * 60 - seconds) * -1;
                if (mins * 60 - seconds < 10)
                {
                    playtime1.text = $"0{mins}:0{secFin}";
                    playtime2.text = $"0{mins}:0{secFin}";  
                }
                else
                {
                    playtime1.text = $"0{mins}:{secFin}";
                    playtime2.text = $"0{mins}:{secFin}";  
                }
            }
            else
            {
                if (secs < 10)
                {
                    playtime1.text = $"00:0{seconds}";
                    playtime2.text = $"00:0{seconds}";
                }
                else
                {
                    playtime1.text = $"00:{seconds}";
                    playtime2.text = $"00:{seconds}";  
                }
            }
        }
    }
}
