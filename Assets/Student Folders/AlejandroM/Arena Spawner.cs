using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSpawner : MonoBehaviour
{
    public static bool OnBeat = false;

    float beatInterval = 0.5f;
    float beatWindow = 0.15f;
    float meleeHitRange = 1.7f;

    AudioSource musicSource;

    int currentLevel;
    int score = 0;

    List<ThingInfo> spawnedEnemies = new List<ThingInfo>();

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        RemoveWalls();
        PlayMusic();
        StartCoroutine(BeatPulse());

        currentLevel = Mathf.Max(1, God.Session.Level);
        UpdateRhythmUI();

        yield return new WaitForSeconds(2f);

        StartCoroutine(LevelWaveLoop());
    }

    void Update()
    {
        CheckDefeatedEnemies();

        if (Input.GetMouseButtonDown(0))
        {
            if (OnBeat)
            {
                Debug.Log("Perfect rhythm attack!");
                HitClosestEnemy();
            }
            else
            {
                Debug.Log("Missed beat!");
            }
        }
    }

    IEnumerator LevelWaveLoop()
    {
        while (true)
        {
            Debug.Log("Starting Rhythm Level " + currentLevel);

            SpawnLevelEnemies();

            while (spawnedEnemies.Count > 0)
            {
                yield return null;
            }

            Debug.Log("Level cleared!");

            currentLevel++;
            God.Session.Level = currentLevel;

            UpdateRhythmUI();

            yield return new WaitForSeconds(2f);
        }
    }

    void SpawnLevelEnemies()
    {
        Vector2 playerPos = God.Session.Player.Thing.transform.position;

        SpawnEnemy(playerPos + new Vector2(0, 5));
        SpawnEnemy(playerPos + new Vector2(5, 0));

        if (currentLevel >= 4)
        {
            SpawnEnemy(playerPos + new Vector2(-5, 0));
        }

        if (currentLevel >= 7)
        {
            SpawnEnemy(playerPos + new Vector2(0, -5));
        }

        if (currentLevel >= 10)
        {
            SpawnEnemy(playerPos + new Vector2(3.5f, 3.5f));
            SpawnEnemy(playerPos + new Vector2(-3.5f, 3.5f));
        }
    }

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

    void HitClosestEnemy()
    {
        Vector2 playerPos = God.Session.Player.Thing.transform.position;

        ThingInfo closestEnemy = null;
        float closestDistance = 999f;

        foreach (ThingController thing in God.GM.Things)
        {
            if (thing == null || thing.Info == null)
                continue;

            if (thing.Info.Team != GameTeams.Enemy)
                continue;

            float distance = Vector2.Distance(playerPos, thing.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = thing.Info;
            }
        }

        if (closestEnemy != null && closestDistance <= meleeHitRange)
        {
            closestEnemy.Destruct(God.Session.Player);

            Debug.Log("Enemy defeated on beat!");
        }
        else
        {
            Debug.Log("No enemy close enough to hit.");
        }
    }

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

    void UpdateRhythmUI()
    {
        if (God.GM != null)
        {
            God.GM.SetUI("RhythmLevel", "Rhythm\nLevel: " + currentLevel, 1);
            God.GM.SetUI("RhythmScore", "Score: " + score, 2);
        }
    }

    void LateUpdate()
    {
        LockPlayerInPlace();
    }

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

    IEnumerator BeatPulse()
    {
        while (true)
        {
            OnBeat = true;
            Debug.Log("BEAT");

            yield return new WaitForSeconds(beatWindow);

            OnBeat = false;

            yield return new WaitForSeconds(beatInterval - beatWindow);
        }
    }

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
}