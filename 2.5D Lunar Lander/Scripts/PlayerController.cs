using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float hitMagnitude = 0.0f; // 3.0 if good
    public int speed = 0;
    public float engineWaitTime = 0.0f;
    public float timerSpeed = 0.0f;
    private float timerHL = 0.0f, timerHR = 0.0f, timerVU = 0.0f, timerVD = 0.0f;
    public float maxRateOvertimeMainEngine = 0.0f;
    public float minRateOvertimeMainEngine = 0.0f;
    public float accelerationRateMainEngine = 0.0f;
    public float trusterMax = 0.0f, glowMax = 0.0f, lineMax = 0.0f;

    // private Dictionary<string, int> platformsActivatedPerAsteroid = new Dictionary<string, int>();

    // this should be particles systems that are attached to the space-ship
    public GameObject[] shipExplosions = new GameObject[3];

    // trusters
    public GameObject engineLeftPrefab, engineTopPrefab, engineRightPrefab, engineBottomPrefab;

    private GameObject glowLeft, lineLeft, glowTop, lineTop, glowRight, lineRight, mainEngine;

    // private enum Platform { PlatformA, PlatformB, PlatformC, PlatformD };
    public string[] platformColliders = {"PlatformA", "PlatformB", "PlatformC", "PlatformD"};

    private static PlayerController instance = null;

    private bool isLeft = false, isUp = false, isRight = false, isDown = false;

    private float horizontalMove = 0.0f, verticalMove = 0.0f;

    private float movementSpeedH = 0.0f, movementSpeedV = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Utilities.CurrentScene();
        getEngineJets();
        Utilities.CountPlatforms();

        SetInstance(this);
    }

    // Update is called once per frame
    void Update()
    {
        movements();
    }

    private void movements() {
        if (Utilities.GetIsPaused()) { return; } // if game is paused do not listen inputs

        // Debug.Log("movements isLeft: " + isLeft + " isUp: " + isUp + " isRight: " + isRight + " isDown: " + isDown);

        // float horizontalMove = Input.GetAxis("Horizontal");
        // float verticalMove = Input.GetAxis("Vertical");

        // Debug.Log("movements horizontalMove: " + horizontalMove + " - verticalMove: " + verticalMove);

        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        if (horizontalMove < 0 || horizontalMove > 0 || verticalMove < 0 || verticalMove > 0) {
            isLeft = false;
            isRight = false;
            isUp = false;
            isDown = false;
        }


        if (isLeft) {
            horizontalMove = movementSpeedH;
        } else if (isRight) {
            horizontalMove = movementSpeedH;
        } else {
            // horizontalMove = Input.GetAxis("Horizontal");
        }

        if (isUp) {
            verticalMove = movementSpeedV;
        } else if (isDown) {
            verticalMove = movementSpeedV;
        } else {
            // verticalMove = Input.GetAxis("Vertical");
        }

        // Debug.Log("movements horizontalMove: " + horizontalMove + " - verticalMove: " + verticalMove);

        if (horizontalMove < 0) {
            // left
            timerHR = 0.0f;
            sideThrusterControl(horizontalMove, true, false);
            if (timerHL > engineWaitTime) {
                move(horizontalMove, 0);
            }
            timerHL += timerSpeed;
        }
        if (horizontalMove > 0) {
            // right
            timerHL = 0.0f;
            sideThrusterControl(horizontalMove, true, false);
            if (timerHR > engineWaitTime) {
                move(horizontalMove, 0);
            }
            timerHR += timerSpeed;
        }
        if (verticalMove > 0) {
            // down
            timerVD = 0.0f;
            topThrusterControl(false, true);
            mainEngineControl(true, false);
            if (timerVU > engineWaitTime) {
                move(0, verticalMove);
            }
            
            timerVU += timerSpeed;
        }
        if (verticalMove < 0) { 
            // up
            timerVU = 0.0f;
            topThrusterControl(true, false);
            mainEngineControl(false, true);
            if (timerVD > engineWaitTime) {
                move(0, verticalMove);
            }

            timerVD += timerSpeed;
        }
        if (horizontalMove == 0) {
            sideThrusterControl(0.0f, false, true);
            timerHL = 0.0f;
            timerHR = 0.0f;
        }
        if (verticalMove == 0) {
            topThrusterControl(false, true);
            mainEngineControl(false, true);
            timerVU = 0.0f;
            timerVD = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            sideThrusterControl(0.0f, false, true);
            topThrusterControl(false, true);
            mainEngineControl(false, true);
            timerHL = 0.0f;
            timerHR = 0.0f;
            timerVU = 0.0f;
            timerVD = 0.0f;
        }
    }

    private void OnCollisionEnter(Collision hitInfo) {
        Debug.Log("OnCollisionEnter: " + hitInfo.relativeVelocity.magnitude);
        // hitInfo.gameObject.tag

        if (hitInfo.relativeVelocity.magnitude > hitMagnitude) {
            exploded();
        }
        else if ( platformColliders.Contains(hitInfo.gameObject.tag) ) {
            Debug.Log("OnCollisionEnter: " + hitInfo.gameObject.tag);
            activatePlatform(hitInfo.gameObject);
        }
    }

    private void getEngineJets() {
        if (engineLeftPrefab != null) {
            if (glowLeft == null) { glowLeft = Utilities.getChild(engineLeftPrefab, "Glow", true); }
            if (lineLeft == null) { lineLeft = Utilities.getChild(engineLeftPrefab, "Line", true); }
        } else {
            Debug.Log("engineLeft NULL");
        }

        if (engineTopPrefab != null) {
            if (glowTop == null) { glowTop = Utilities.getChild(engineTopPrefab, "Glow", true); }
            if (lineTop == null) { lineTop = Utilities.getChild(engineTopPrefab, "Line", true); }
        } else {
            Debug.Log("engineTopPrefab NULL");
        }

        if (engineRightPrefab != null) {
            if (glowRight == null) { glowRight = Utilities.getChild(engineRightPrefab, "Glow", true); }
            if (lineRight == null) { lineRight = Utilities.getChild(engineRightPrefab, "Line", true); }
        } else {
            Debug.Log("engineRightPrefab NULL");
        }

        if (engineBottomPrefab != null) {
            if (mainEngine == null) { mainEngine = Utilities.getChild(engineBottomPrefab, "Energy", true); }
        } else {
            Debug.Log("engineBottomPrefab NULL");
        }
    }

    private void sideThrusterControl(float direction, bool start, bool stop) {
        if (start && stop) { return; }

        ParticleSystem p0 = null;
        ParticleSystem p1 = null;
        ParticleSystem p2 = null;
        ParticleSystem p3 = null;
        ParticleSystem p4 = null;
        ParticleSystem p5 = null;

        if (engineLeftPrefab != null  && engineRightPrefab != null) {
            p0 = engineLeftPrefab.GetComponent<ParticleSystem>();
            p1 = glowLeft.GetComponent<ParticleSystem>();
            p2 = lineLeft.GetComponent<ParticleSystem>();

            p3 = engineRightPrefab.GetComponent<ParticleSystem>();
            p4 = glowRight.GetComponent<ParticleSystem>();
            p5 = lineRight.GetComponent<ParticleSystem>();
        } else {
            Debug.Log("engineLeftPrefab OR engineRightPrefab NULL");
        }

        if (direction < 0) {
            p3 = engineLeftPrefab.GetComponent<ParticleSystem>();
            p4 = glowLeft.GetComponent<ParticleSystem>();
            p5 = lineLeft.GetComponent<ParticleSystem>();

            p0 = engineRightPrefab.GetComponent<ParticleSystem>();
            p1 = glowRight.GetComponent<ParticleSystem>();
            p2 = lineRight.GetComponent<ParticleSystem>();
        }
        if (direction > 0) {
            p0 = engineLeftPrefab.GetComponent<ParticleSystem>();
            p1 = glowLeft.GetComponent<ParticleSystem>();
            p2 = lineLeft.GetComponent<ParticleSystem>();

            p3 = engineRightPrefab.GetComponent<ParticleSystem>();
            p4 = glowRight.GetComponent<ParticleSystem>();
            p5 = lineRight.GetComponent<ParticleSystem>();
        }

        if (stop) {
            var em0 = p0.emission;
            var em1 = p1.emission;
            var em2 = p2.emission;
            var em3 = p3.emission;
            var em4 = p4.emission;
            var em5 = p5.emission; 

            em0.enabled = false;
            em1.enabled = false;
            em2.enabled = false;
            em3.enabled = false;
            em4.enabled = false;
            em5.enabled = false;

            em0.rateOverTime = 0.0f;
            em1.rateOverTime = 0.0f;
            em2.rateOverTime = 0.0f;
            em3.rateOverTime = 0.0f;
            em4.rateOverTime = 0.0f;
            em5.rateOverTime = 0.0f;
            return;
        }

        if (start) {
            var em0 = p0.emission;
            float constant0 = em0.rateOverTime.constant;

            var em1 = p1.emission;
            float constant1 = em1.rateOverTime.constant;

            var em2 = p2.emission;
            float constant2 = em2.rateOverTime.constant;

            var em3 = p3.emission;
            var em4 = p4.emission;
            var em5 = p5.emission;

            if (em0.enabled == false) {
                em0.enabled = true;
            }
            if (em1.enabled == false) {
                em1.enabled = true;
            }
            if (em2.enabled == false) {
                em2.enabled = true;
            }

            if (constant0 < maxRateOvertimeMainEngine) {
                em0.rateOverTime = constant0 + accelerationRateMainEngine;
                em1.rateOverTime = constant1 + accelerationRateMainEngine;
                em2.rateOverTime = constant2 + accelerationRateMainEngine;
            }

            if (em3.enabled == true || em4.enabled == true || em5.enabled == true) {
                em3.enabled = false;
                em4.enabled = false;
                em5.enabled = false;

                em3.rateOverTime = 0.0f;
                em4.rateOverTime = 0.0f;
                em5.rateOverTime = 0.0f;
            }
        } else {
            var em0 = p0.emission;
            float constant0 = em0.rateOverTime.constant;

            var em1 = p1.emission;
            float constant1 = em1.rateOverTime.constant;

            var em2 = p2.emission;
            float constant2 = em2.rateOverTime.constant;

            var em3 = p3.emission;
            var em4 = p4.emission;
            var em5 = p5.emission;

            if (constant0 == minRateOvertimeMainEngine) {
                if (em0.enabled == true) {
                    em0.enabled = false;
                }
                if (em1.enabled == true) {
                    em1.enabled = false;
                }
                if (em2.enabled == true) {
                    em2.enabled = false;
                }
            }

            if (constant0 > minRateOvertimeMainEngine) {
                em0.rateOverTime = constant0 - accelerationRateMainEngine;
                em1.rateOverTime = constant1 - accelerationRateMainEngine;
                em2.rateOverTime = constant2 - accelerationRateMainEngine;
            }

            if (em3.enabled == true || em4.enabled == true || em5.enabled == true) {
                em3.enabled = false;
                em4.enabled = false;
                em5.enabled = false;

                em3.rateOverTime = 0.0f;
                em4.rateOverTime = 0.0f;
                em5.rateOverTime = 0.0f;
            }
        }
    }

    private void topThrusterControl(bool start, bool stop) {
        if (start && stop) { return; }

        ParticleSystem p0 = null;
        ParticleSystem p1 = null;
        ParticleSystem p2 = null;

        if (engineTopPrefab != null) {
            p0 = engineTopPrefab.GetComponent<ParticleSystem>();
            p1 = glowTop.GetComponent<ParticleSystem>();
            p2 = lineTop.GetComponent<ParticleSystem>();
        } else {
            Debug.Log("topThruster NULL");
            return;
        }

        if (stop) {
            var em0 = p0.emission;
            var em1 = p1.emission;
            var em2 = p2.emission;

            em0.enabled = false;
            em1.enabled = false;
            em2.enabled = false;
            em0.rateOverTime = 0.0f;
            em1.rateOverTime = 0.0f;
            em2.rateOverTime = 0.0f;
            return;
        }

        if (start) {
            var em0 = p0.emission;
            float constant0 = em0.rateOverTime.constant;

            var em1 = p1.emission;
            float constant1 = em1.rateOverTime.constant;

            var em2 = p2.emission;
            float constant2 = em2.rateOverTime.constant;

            if (em0.enabled == false) {
                em0.enabled = true;
            }
            if (em1.enabled == false) {
                em1.enabled = true;
            }
            if (em2.enabled == false) {
                em2.enabled = true;
            }

            if (constant0 < maxRateOvertimeMainEngine) {
                em0.rateOverTime = constant0 + accelerationRateMainEngine;
                em1.rateOverTime = constant1 + accelerationRateMainEngine;
                em2.rateOverTime = constant2 + accelerationRateMainEngine;
            }
        } else {
            var em0 = p0.emission;
            float constant0 = em0.rateOverTime.constant;

            var em1 = p1.emission;
            float constant1 = em1.rateOverTime.constant;

            var em2 = p2.emission;
            float constant2 = em2.rateOverTime.constant;

            em0.rateOverTime = 0.0f;
            em1.rateOverTime = 0.0f;
            em2.rateOverTime = 0.0f;

            em0.enabled = false;
            em1.enabled = false;
            em1.enabled = false;
        } 
    }

    private void mainEngineControl(bool stat, bool stop) {
        if (stat && stop) { return; }

        ParticleSystem p0 = null;
        ParticleSystem p1 = null;

        if (mainEngine != null) {
            p0 = engineBottomPrefab.GetComponent<ParticleSystem>();
            p1 = mainEngine.GetComponent<ParticleSystem>();
        } else {
            Debug.Log("mainEngine NULL");
            return;
        }

        if (stop) {
            var em0 = p0.emission;
            var em1 = p1.emission;

            em0.rateOverTime = 0.0f;
            em1.rateOverTime = 0.0f;
            em0.enabled = false;
            em1.enabled = false;
            return;
        }

        if (stat) {
            var em0 = p0.emission;
            float constant0 = em0.rateOverTime.constant;

            var em1 = p1.emission;
            float constant = em1.rateOverTime.constant;

            if (em0.enabled == false) {
                em0.enabled = true;
            }
            if (em1.enabled == false) {
                em1.enabled = true;
            }

            if (constant < maxRateOvertimeMainEngine) {
                em0.rateOverTime = constant + accelerationRateMainEngine;
                em1.rateOverTime = constant + accelerationRateMainEngine;
            }
        } else {
            var em0 = p0.emission;
            float constant0 = em0.rateOverTime.constant;

            var em1 = p1.emission;
            float constant = em1.rateOverTime.constant;
            
            if (constant == minRateOvertimeMainEngine) {
                if (em0.enabled == true) {
                    em0.enabled = false;
                }
                if (em1.enabled == true) {
                    em1.enabled = false;
                }
            }

            if (constant > minRateOvertimeMainEngine) {
                em0.rateOverTime = constant - accelerationRateMainEngine;
                em1.rateOverTime = constant - accelerationRateMainEngine;
            }
        }

    } 

    private void move(float horizontalMove, float verticalMove) {
        Vector3 v = new Vector3();
        Rigidbody r = this.gameObject.GetComponent<Rigidbody>();

        // left
        if (horizontalMove < 0) {
            // left
            v.x = -speed; v.y = 0; v.z = 0;
        }
        if (horizontalMove > 0) {
            // right
            v.x = speed; v.y = 0; v.z = 0;
        }
        if (verticalMove > 0) {
            // up
            v.x = 0; v.y = speed; v.z = 0;
        }
        if (verticalMove < 0) {
            // down
            v.x = 0; v.y = -speed; v.z = 0;
        }


        

        r.AddForce(v);
    }

    private void exploded() {

        if (shipExplosions == null) { Debug.Log("shipExplosions is NULL"); return; }
        if (shipExplosions.Length == 0) { Debug.Log("shipExplosions is EMPTY"); return; }

        int i = Random.Range(0,3);
        Debug.Log("shipExplosions index: " + i);

        GameObject expGameObj = shipExplosions[i].gameObject;
        ParticleSystem explosion = expGameObj.GetComponent<ParticleSystem>();
        
        var em0 = explosion.emission;
        em0.enabled = true;
        explosion.Play();

        destroyShip();
    }

    private void destroyShip() {
        // hide ship to have enough time to play explosion animation and after destroy
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(this.gameObject, 3.0f);
    }

    private void activatePlatform(GameObject platformCollider) {
        Utilities.activatePlatform(platformCollider); // this will activate asteroid strictures as well
    }

    public void Left(float movementSpeed) {
        Debug.Log("Left");
        // instance.makeMovements("left");
        // verticalMove = -1.0f;

        isLeft = true;
        isRight = false;
        this.movementSpeedH = movementSpeed;
    }

    public void Up(float movementSpeed) {
        Debug.Log("Up");
        // instance.makeMovements("up");
        // verticalMove = 1.0f;

        isUp = true;
        isDown = false;
        this.movementSpeedV = movementSpeed;
    }

    public void Right(float movementSpeed) {
        Debug.Log("Right");
        // instance.makeMovements("right");
        // horizontalMove = 1.0f;

        isRight = true;
        isLeft = false;
        this.movementSpeedH = movementSpeed;
    }

    public void Down(float movementSpeed) {
        Debug.Log("Down");
        // instance.makeMovements("down");
        // verticalMove = -1.0f;

        isDown = true;
        isUp = false;
        this.movementSpeedV = movementSpeed;
    }

    public void Stop() {
        Debug.Log("Stop");
        // instance.makeMovements("stop");
        // verticalMove = 0.0f;

        isLeft = false;
        isUp = false;
        isRight = false;
        isDown = false;
    }

    
    public static void SetInstance(PlayerController playerController) {
        Debug.Log("PlayerController SetInstance");

        instance = playerController;
    }

    public static PlayerController GetInstance() {
        return instance;
    }
    



    // Keep this code for references
    /*
    private void test() {
        // test: get gameobject script 

        // public GameObject utilitiesScript;
        // utilitiesScript = GameObject.Find("UtilitiesScript");

        // Utilities utilities = utilitiesScript.GetComponent<Utilities>();
        // utilities.getChiled(engineLeft, "Glow", true);
    }
    */

    /*
    private void activatePlatformTest(GameObject platformCollider) {

        //Material platformA = platformPrefab.GetComponent<Material>();
        GameObject platformPrefab = platformCollider.transform.parent.gameObject; // this has material
        GameObject pointLightA = platformPrefab.transform.Find("Point LightA").gameObject;
        GameObject pointLightB = platformPrefab.transform.Find("Point LightB").gameObject;
        GameObject pointLightC = platformPrefab.transform.Find("Point LightC").gameObject;

        Light lightA = pointLightA.GetComponent<Light>();
        Light lightB = pointLightB.GetComponent<Light>();
        Light lightC = pointLightC.GetComponent<Light>();
        lightA.enabled = false;
        lightB.enabled = false;
        lightC.enabled = true;

        Debug.Log("activatePlatform platformCollider.name: " + platformCollider.name);
        Debug.Log("activatePlatform platform platformPrefab.name: " + platformPrefab.name);

        Material[] materials = Resources.FindObjectsOfTypeAll<Material>();
        Material platformOn = null;
        for (int i = 0; i < materials.Length; i++) {
            Material m = materials[i];
            if (m.name == "PlatformB") {
                platformOn = m;
                break;
            }
        }

        Material platformOnTest = Resources.Load<Material>("PlatformB");
        // platformPrefab.GetComponent<MeshRenderer>().material = platformOn;

        Material[] materialsTest = platformPrefab.GetComponent<MeshRenderer>().materials;
        materialsTest[0] = platformOn;
        platformPrefab.GetComponent<MeshRenderer>().materials = materialsTest;
    }
    */
}
