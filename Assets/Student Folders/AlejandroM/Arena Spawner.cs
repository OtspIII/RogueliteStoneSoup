using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArenaSpawner : MonoBehaviour
{
    public static bool OnBeat = false;
    float bpm = 145f;
    float beatOffset = 0.15f;
    int lastBeatNumber = -1;

    float beatInterval;
    float beatWindow = 0.90f; // Timing 

    float spawnDistance = 9f; // The distance the enemies spawn away from the player
    float meleeHitRange = 4f; // how close the player is 

    AudioSource musicSource; // Audio source used to play the level 

    int currentLevel;
    int score = 0;

    List<ThingInfo> spawnedEnemies = new List<ThingInfo>(); // list of all currently spawned enemies 

    // Runs when the scene starts setting up the arena, level loop, UI and beat system
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        RemoveWalls();
        PlayMusic();
        StartCoroutine(BeatPulse());
        beatInterval = 60f / bpm;

        currentLevel = Mathf.Max(1, God.Session.Level);
        UpdateRhythmUI();

        yield return new WaitForSeconds(2f);

        StartCoroutine(LevelWaveLoop());
    }


    // Runs frames and attack inputs 
    // When the players click the arrows at a 
    void Update()
    {
        CheckDefeatedEnemies();
        RemoveEnemiesTouchingPlayer();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryDirectionAttack(Vector2.up);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryDirectionAttack(Vector2.right);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryDirectionAttack(Vector2.left);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryDirectionAttack(Vector2.down);
        }
    }

    // gameplay loop where the game spawns waves and increases each level
    IEnumerator LevelWaveLoop()
    {
        yield return StartCoroutine(ContinuousBeatSpawning());
    }

    // Enemies spawn around the player appearing in one direction at a time 
    IEnumerator ContinuousBeatSpawning()
    {
        while (true)
        {
            if (God.Session == null || God.Session.Player == null || God.Session.Player.Thing == null)
            {
                yield break;
            }

            yield return StartCoroutine(WaitForNextBeat());

            if (God.Session == null || God.Session.Player == null || God.Session.Player.Thing == null)
            {
                yield break;
            }

            Vector2 playerPos = God.Session.Player.Thing.transform.position;

            for (int i = 0; i < beatChart.Count; i++)
            {
                if (spawnedBeatIndexes.Contains(i))
                    continue;

                if (lastBeatNumber >= beatChart[i].beat)
                {
                    Vector2 spawnDirection = beatChart[i].direction;
                    Vector2 spawnPosition = playerPos + (spawnDirection * spawnDistance);

                    SpawnEnemy(spawnPosition);

                    spawnedBeatIndexes.Add(i);
                }
            }

            spawnedEnemies.RemoveAll(e => e == null || e.Destroyed || e.Thing == null);

            currentLevel = 1 + (score / 5);
            God.Session.Level = currentLevel;
            UpdateRhythmUI();
        }
    }
    // increases score when the enemies are defeated and checks when enemies are destroyed 
    void CheckDefeatedEnemies()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null || spawnedEnemies[i].Destroyed || spawnedEnemies[i].Thing == null)
            {
                spawnedEnemies.RemoveAt(i);

                score++;

                UpdateRhythmUI();

                Debug.Log("Score increased: " + score);
            }
        }
    }
    void TryDirectionAttack(Vector2 direction)
    {
        if (!OnBeat)
        {
            Debug.Log("Missed beat!");
            return;
        }

        if (God.Session == null || God.Session.Player == null || God.Session.Player.Thing == null)
            return;

        Vector2 playerPos = God.Session.Player.Thing.transform.position;

        ThingInfo bestEnemy = null;
        float closestDistance = 999f;

        foreach (ThingInfo enemy in spawnedEnemies)
        {
            if (enemy == null || enemy.Destroyed || enemy.Thing == null)
                continue;

            Vector2 enemyPos = enemy.Thing.transform.position;
            Vector2 toEnemy = (enemyPos - playerPos).normalized;

            float directionCheck = Vector2.Dot(direction, toEnemy);
            float distance = Vector2.Distance(playerPos, enemyPos);

            if (directionCheck > 0.7f && distance < closestDistance)
            {
                closestDistance = distance;
                bestEnemy = enemy;
            }
        }

        if (bestEnemy != null && closestDistance <= meleeHitRange)
        {
            bestEnemy.Destruct(God.Session.Player);
            Debug.Log("Correct arrow on beat!");
        }
        else
        {
            Debug.Log("Wrong arrow or enemy too far.");
        }
    }

    // Spawns enemies in a area
    void SpawnEnemy(Vector2 position)
    {
        ThingOption enemyOption = God.Library.GetThing(new SpawnRequest("Hater"));
        

        if (enemyOption == null)
        {
            Debug.LogError("Enemy not found. Check custom tag is Hater.");
            return;
        }

        ThingInfo enemyInfo = enemyOption.Create();

        enemyInfo.Spawn(position);

        spawnedEnemies.Add(enemyInfo);

        Debug.Log("Spawned enemy at " + position);
    }
    // updates the score and level UI
    void UpdateRhythmUI()
    {
        if (God.GM != null)
        {
            God.GM.SetUI("RhythmLevel", "Rhythm\nLevel: " + currentLevel, 1);
            God.GM.SetUI("RhythmScore", "Score: " + score, 2);
        }
    }
    // Locks player in place
    void LateUpdate()
    {
        LockPlayerInPlace();
    }
    // Has the player stay still and cannot move or be knocked back
    void LockPlayerInPlace()
    {
        if (God.Session == null || God.Session.Player == null || God.Session.Player.Thing == null)
            return;

        ThingController player = God.Session.Player.Thing;

        player.Info.DesiredMove = Vector2.zero;
        player.ActualMove = Vector2.zero;
        player.Knockback = Vector2.zero;

        if (player.RB != null)
        {
            player.RB.linearVelocity = Vector2.zero;
        }
    }
      // Playes the music thats in the resource folder
    void PlayMusic()
    {
        AudioClip song = Resources.Load<AudioClip>("AlejandroM/Songs/Song1");

        if (song == null)
        {
            Debug.LogError("Song not found. Put it in Resources/AlejandroM/Songs and name it Song1.");
            return;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = song;
        musicSource.loop = true;
        musicSource.Play();

        Debug.Log("Music started");
    }
    // Turns the on beat on and off repeatedly based on BPM
    IEnumerator BeatPulse()
    {
        while (true)
        {
            if (musicSource != null && musicSource.isPlaying)
            {
                float songPosition = musicSource.time - beatOffset;

                if (songPosition >= 0)
                {
                    int beatNumber = Mathf.FloorToInt(songPosition / beatInterval);
                    float timeIntoBeat = songPosition - (beatNumber * beatInterval);

                    OnBeat = timeIntoBeat <= beatWindow;

                    if (beatNumber != lastBeatNumber)
                    {
                        lastBeatNumber = beatNumber;
                        Debug.Log("BEAT " + beatNumber);
                    }
                }
            }

            yield return null;
        }
    }
    // Removes the walls from the room 
    void RemoveWalls()
    {
        ThingController[] allThings = GameObject.FindObjectsByType<ThingController>(FindObjectsSortMode.None);

        foreach (ThingController thing in allThings)
        {
            if (thing.Info == null || thing.Info.Type == null)
                continue;

            if (thing.Info.Type.HasTag(GameTags.Wall.ToString()))
            {
                Destroy(thing.gameObject);
            }
        }

        Debug.Log("Arena walls removed");
    }

    // Removes the walls from the room and prevents the enemy from pushing 
    void RemoveEnemiesTouchingPlayer()
    {
        if (God.Session == null || God.Session.Player == null || God.Session.Player.Thing == null)
            return;

        Vector2 playerPos = God.Session.Player.Thing.transform.position;

        foreach (ThingController thing in God.GM.Things.ToArray())
        {
            if (thing == null || thing.Info == null)
                continue;

            if (thing.Info.Team != GameTeams.Enemy)
                continue;

            float distance = Vector2.Distance(playerPos, thing.transform.position);

            // Enemy disappears before it is close enough to push or stun the player
            if (distance <= 1f)
            {
                thing.Info.Destruct(God.Session.Player);
                Debug.Log("Enemy reached player and disappeared");
            }
        }
    }
    IEnumerator WaitForNextBeat()
    {
        int startingBeat = lastBeatNumber;

        while (lastBeatNumber == startingBeat)
        {
            yield return null;
        }
    }

    class BeatSpawn
    {
        public float beat;
        public Vector2 direction;

        public BeatSpawn(float beat, Vector2 direction)
        {
            this.beat = beat;
            this.direction = direction;
        }
    }

    List<BeatSpawn> beatChart = new List<BeatSpawn>()
{

 new BeatSpawn(0.13f, Vector2.up),
new BeatSpawn(1.13f, Vector2.right),
new BeatSpawn(2.13f, Vector2.up),
new BeatSpawn(4.13f, Vector2.right),
new BeatSpawn(5.13f, Vector2.up),
new BeatSpawn(6.13f, Vector2.up),
new BeatSpawn(8.13f, Vector2.right),
new BeatSpawn(9.13f, Vector2.right),
new BeatSpawn(11.13f, Vector2.up),
new BeatSpawn(12.13f, Vector2.up),
new BeatSpawn(13.13f, Vector2.right),
new BeatSpawn(16.13f, Vector2.right),
new BeatSpawn(17.13f, Vector2.right),
new BeatSpawn(18.13f, Vector2.up),
new BeatSpawn(19.13f, Vector2.up),
new BeatSpawn(20.13f, Vector2.right),
new BeatSpawn(21.13f, Vector2.right),
new BeatSpawn(22.13f, Vector2.right),
new BeatSpawn(24.13f, Vector2.right),
new BeatSpawn(25.13f, Vector2.right),
new BeatSpawn(26.13f, Vector2.right),
new BeatSpawn(27.13f, Vector2.up),
new BeatSpawn(28.13f, Vector2.up),
new BeatSpawn(29.13f, Vector2.up),
new BeatSpawn(30.13f, Vector2.right),
new BeatSpawn(32.13f, Vector2.right),
new BeatSpawn(33.13f, Vector2.right),
new BeatSpawn(34.13f, Vector2.left),
new BeatSpawn(35.13f, Vector2.left),
new BeatSpawn(36.13f, Vector2.left),
new BeatSpawn(37.13f, Vector2.right),
new BeatSpawn(38.13f, Vector2.up),
new BeatSpawn(39.13f, Vector2.left),
new BeatSpawn(40.13f, Vector2.right),
new BeatSpawn(41.13f, Vector2.up),
new BeatSpawn(42.13f, Vector2.left),
new BeatSpawn(43.13f, Vector2.up),
new BeatSpawn(44.13f, Vector2.left),
new BeatSpawn(45.13f, Vector2.right),
new BeatSpawn(46.13f, Vector2.right),
new BeatSpawn(47.13f, Vector2.left),
new BeatSpawn(48.13f, Vector2.right),
new BeatSpawn(49.13f, Vector2.left),
new BeatSpawn(50.13f, Vector2.right),
new BeatSpawn(51.13f, Vector2.left),
new BeatSpawn(52.13f, Vector2.down),
new BeatSpawn(53.13f, Vector2.down),
new BeatSpawn(54.13f, Vector2.down),
new BeatSpawn(55.13f, Vector2.down),
new BeatSpawn(56.13f, Vector2.down),
new BeatSpawn(57.13f, Vector2.up),
new BeatSpawn(58.13f, Vector2.up),
new BeatSpawn(59.13f, Vector2.up),
new BeatSpawn(60.13f, Vector2.up),
new BeatSpawn(61.13f, Vector2.up),
new BeatSpawn(62.13f, Vector2.down),
new BeatSpawn(63.13f, Vector2.left),
new BeatSpawn(64.13f, Vector2.down),
new BeatSpawn(65.13f, Vector2.left),
new BeatSpawn(66.13f, Vector2.down),
new BeatSpawn(67.13f, Vector2.left),
new BeatSpawn(68.13f, Vector2.down),
new BeatSpawn(69.13f, Vector2.left),
new BeatSpawn(70.13f, Vector2.up),
new BeatSpawn(71.13f, Vector2.down),
new BeatSpawn(72.13f, Vector2.up),
new BeatSpawn(73.13f, Vector2.down),
new BeatSpawn(74.13f, Vector2.up),
new BeatSpawn(75.13f, Vector2.down),
new BeatSpawn(76.13f, Vector2.up),
new BeatSpawn(77.13f, Vector2.down),
new BeatSpawn(78.13f, Vector2.left),
new BeatSpawn(79.13f, Vector2.left),
new BeatSpawn(80.13f, Vector2.left),
new BeatSpawn(81.13f, Vector2.left),
new BeatSpawn(82.13f, Vector2.up),
new BeatSpawn(83.13f, Vector2.left),
new BeatSpawn(84.13f, Vector2.right),
new BeatSpawn(85.13f, Vector2.right),
new BeatSpawn(86.13f, Vector2.right),
new BeatSpawn(87.13f, Vector2.right),
new BeatSpawn(88.13f, Vector2.up),
new BeatSpawn(89.13f, Vector2.left),
new BeatSpawn(90.13f, Vector2.up),
new BeatSpawn(91.13f, Vector2.left),
new BeatSpawn(92.13f, Vector2.left),
new BeatSpawn(93.13f, Vector2.up),
new BeatSpawn(94.13f, Vector2.up),
new BeatSpawn(95.13f, Vector2.up),
new BeatSpawn(96.13f, Vector2.left),
new BeatSpawn(99.13f, Vector2.left),
new BeatSpawn(100.13f, Vector2.left),
new BeatSpawn(101.13f, Vector2.down),
new BeatSpawn(102.13f, Vector2.left),
new BeatSpawn(103.13f, Vector2.down),
new BeatSpawn(104.13f, Vector2.left),
new BeatSpawn(106.13f, Vector2.down),
new BeatSpawn(107.13f, Vector2.left),
new BeatSpawn(108.13f, Vector2.down),
new BeatSpawn(109.13f, Vector2.right),
new BeatSpawn(110.13f, Vector2.right),
new BeatSpawn(111.13f, Vector2.right),
new BeatSpawn(112.13f, Vector2.right),
new BeatSpawn(114.13f, Vector2.right),

    };

    List<int> spawnedBeatIndexes = new List<int>();







}

