using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utilities : MonoBehaviour
{
    private static Dictionary<string, int> platformsActivatedPerAsteroid = new Dictionary<string, int>();
    private static bool isPaused = false; // using for game pause

    private static string currentSceneName = null;
    private static string currentLevel = null;
    private static string nextLevelName = null;
    private static int totalPlatforms = 0;
    private static int totalPlatformsActivated = 0;
    private static float gameProgress = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void CurrentScene() {
        currentSceneName = SceneManager.GetActiveScene().name;

        string lastChar = currentSceneName.Substring(currentSceneName.Length - 1);

        int currentLevelNum = 0;
        try {
            currentLevelNum = int.Parse(lastChar) + 1;
            nextLevelName = string.Format("Scene{0}", currentLevelNum);
        } catch (Exception ex) {
            Debug.Log("Utilities --> cannot get current scene number: " + ex);

            currentSceneName = "Scene1";
            nextLevelName = currentSceneName;
        }

        Debug.Log("Utilities --> currentSceneName: " + currentSceneName + " --- lastChar: " + lastChar);
        Debug.Log("Utilities --> nextLevelName: " + nextLevelName);
    }

    public static GameObject getChild(GameObject parent, string childName, bool showChildCount) {
        
        if (showChildCount) {
            int childCount = parent.transform.childCount;
            string info = string.Format("Parent [{0}] has [{1}] children.", parent.name, childName);
            Debug.Log(info);
        }

        return parent.transform.Find(childName).gameObject;
    }

    public static void activatePlatform(GameObject platformCollider) {
        // colliding with a platform and getting its child objects and its parent asteroid object
        GameObject platformPrefab = platformCollider.transform.parent.gameObject;
        Debug.Log("This platformPrefab is --> " + platformPrefab.name);

        bool isActivated = platformPrefab.GetComponent<PlatformController>().IsActivated();

        if (!isActivated) {
            platformPrefab.GetComponent<PlatformController>().activatePlatform();

            activateAsteroidStructures(platformPrefab);
        } else {
            Debug.Log("This platformPrefab is --> " + platformPrefab.name + " - activated: " + isActivated);
        }
    }

    private static void activateAsteroidStructures(GameObject platformPrefab) {
        // get root object, asteroid
        GameObject asteroidPrefab = platformPrefab.transform.parent.gameObject;

        // if no kay with the name of current asteroid, count its platforms and add to the dictionary
        if (!platformsActivatedPerAsteroid.ContainsKey(asteroidPrefab.name)) {
            
            int asteroidTotalPlatforms = 0;

            // loop over each child of this asteroid and count platforms
            foreach (Transform child in asteroidPrefab.transform) {
                string childName = child.name.Remove(child.name.Length - 1, 1); // remove Suffix 
                if (childName.Equals("Platform")) {
                    asteroidTotalPlatforms++;
                }
            }
            Debug.Log("This steroidPrefab name: " + asteroidPrefab.name + " - platforms: " + asteroidTotalPlatforms);

            platformsActivatedPerAsteroid.Add(asteroidPrefab.name, asteroidTotalPlatforms - 1); // one platform is activated

            totalPlatformsActivated++;

            calculateProgress();
        } else {
            int count = platformsActivatedPerAsteroid[asteroidPrefab.name];
            if (count > 0) { // try to keep it 0 if no more platforms left to activate, we may need this in future
                platformsActivatedPerAsteroid[asteroidPrefab.name] = count - 1;

                totalPlatformsActivated++;

                calculateProgress();
            }
        }

        // check if no more platforms left to activate for a specific asteroid
        if (platformsActivatedPerAsteroid.ContainsKey(asteroidPrefab.name)) {
            if (platformsActivatedPerAsteroid[asteroidPrefab.name] == 0) {
                GameObject asteroidStructure = null, asteroidStructureB = null, asteroidExtraDock = null, asteroidMiner = null;
                if (asteroidPrefab.transform.Find("AsteroidStructure") != null) {
                    asteroidStructure = asteroidPrefab.transform.Find("AsteroidStructure").gameObject;
                }
                if (asteroidPrefab.transform.Find("AsteroidStructureB") != null) {
                    asteroidStructureB = asteroidPrefab.transform.Find("AsteroidStructureB").gameObject;
                }
                if (asteroidPrefab.transform.Find("AsteroidExtraDock") != null) {
                    asteroidExtraDock = asteroidPrefab.transform.Find("AsteroidExtraDock").gameObject;
                }
                if (asteroidPrefab.transform.Find("AsteroidMiner") != null) {
                    asteroidMiner = asteroidPrefab.transform.Find("AsteroidMiner").gameObject;
                }

                if (asteroidStructure != null) {
                    asteroidStructure.GetComponent<AsteroidStructureController>().updateMaterial();
                }
                if (asteroidStructureB != null) {
                    asteroidStructureB.GetComponent<AsteroidStructureController>().updateMaterial();
                }
                if (asteroidExtraDock != null) {
                    asteroidExtraDock.GetComponent<AsteroidExtraDockController>().updateMaterial();
                }
                if (asteroidMiner != null) {
                    asteroidMiner.GetComponent<AsteroidMinerController>().updateMaterial();
                }
            }
        }

        // test print
        foreach (KeyValuePair<string, int> entry in platformsActivatedPerAsteroid) {
            Debug.Log("Asteroid: " + entry.Key + " - platforms left to activate: " + entry.Value);
        }
    }

    public static void CountPlatforms() {

        GameObject[] asteroidsPrefabs = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach (GameObject ap in asteroidsPrefabs) {
            // loop over each child of this asteroid and count platforms
            foreach (Transform asteroid in ap.transform) {
                foreach (Transform child in asteroid.transform) {
                    string childName = child.name.Remove(child.name.Length - 1, 1); // remove Suffix
                    if (childName.Equals("Platform")) {
                        totalPlatforms++;
                    }
                }
            }
        }

        string out1 = string.Format("Total asteroids in {0}", currentSceneName);
        string out2 = string.Format("are {0}", asteroidsPrefabs.Length);
        string out3 = string.Format("and total platforms: {0}", totalPlatforms);
        string out4 = string.Format("{0} {1} {2}", out1, out2, out3);
        Debug.Log(out4);
    }

    private static void calculateProgress() {
        float progress = (float) 100 / totalPlatforms * (float) totalPlatformsActivated;
        gameProgress = progress;
        Debug.Log("Out of " + totalPlatforms + " platforms activated are " + totalPlatformsActivated + " : " + progress + "%");
    }

    public static void LoadScene(string levelName) {
        resatAll();
        SceneManager.LoadScene(levelName);
    }

    public static void SetIsPaused(bool val) {
        isPaused = val;
    }

    public static bool GetIsPaused() {
        return isPaused;
    }

    public static void PauseGame(bool val) {
        if (val) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1;
        }
    }

    private static void resatAll() {
        totalPlatforms = 0;
        totalPlatformsActivated = 0;
        gameProgress = 0.0f;
        platformsActivatedPerAsteroid = new Dictionary<string, int>();
        SetIsPaused(false);
        PauseGame(isPaused);
    }

    public static string GetCurrentSceneName() {
        return currentSceneName;
    }  

    public static string GetNextLevelName() {
        return nextLevelName;
    }  

    public static float GetGameProgress() {
        return gameProgress;
    }
}
