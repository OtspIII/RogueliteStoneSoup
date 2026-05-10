using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArenaSpawner : MonoBehaviour
{
    public static bool OnBeat = false;
    float bpm = 98f;
    float beatOffset = 0.15f;
    int lastBeatNumber = -1;

    float beatInterval = 60f / 98f;
    float beatWindow = 0.25f;

    float spawnDistance = 7f;
    float meleeHitRange = 3f; // how close the player is 

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

            int beat = lastBeatNumber;

            // First pattern: top, right, top, right
            if (currentLevel <= 3)
            {
                if (beat % 2 == 0)
                {
                    SpawnEnemy(playerPos + new Vector2(0, spawnDistance)); // top
                }
                else
                {
                    SpawnEnemy(playerPos + new Vector2(spawnDistance, 0)); // right
                }
            }
            // Second pattern: top, right, left
            else if (currentLevel <= 6)
            {
                if (beat % 3 == 0)
                {
                    SpawnEnemy(playerPos + new Vector2(0, spawnDistance)); // top
                }
                else if (beat % 3 == 1)
                {
                    SpawnEnemy(playerPos + new Vector2(spawnDistance, 0)); // right
                }
                else
                {
                    SpawnEnemy(playerPos + new Vector2(-spawnDistance, 0)); // left
                }
            }
            // Third pattern: top, right, left, bottom
            else
            {
                if (beat % 4 == 0)
                {
                    SpawnEnemy(playerPos + new Vector2(0, spawnDistance)); // top
                }
                else if (beat % 4 == 1)
                {
                    SpawnEnemy(playerPos + new Vector2(spawnDistance, 0)); // right
                }
                else if (beat % 4 == 2)
                {
                    SpawnEnemy(playerPos + new Vector2(-spawnDistance, 0)); // left
                }
                else
                {
                    SpawnEnemy(playerPos + new Vector2(0, -spawnDistance)); // bottom
                }
            }
            spawnedEnemies.RemoveAll(e => e == null || e.Destroyed || e.Thing == null);
            // Increase level based on score instead of clearing waves
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

            if (distance <= 0.8f)
            {
                thing.Info.Destruct(God.Session.Player);
                Debug.Log("Enemy hit player and disappeared");
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
}