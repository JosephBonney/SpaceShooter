using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.Player
{
    public class Player : MonoBehaviour
    {
        #region Variables

        public float speed = 3.5f;
        private float originalSpeed;
        public float newspeed = 8.5f;

        public float horizontalInput;
        public float verticalInput;

        [SerializeField]
        private GameObject _laserPrefab;
        [SerializeField]
        private GameObject _tripleShot;
        [SerializeField]
        private GameObject Shields;
        [SerializeField]
        private GameObject[] engines;

        private EngineDamage engineDamage;

        [SerializeField]
        private float _fireRate = 0.5f;
        [SerializeField]
        private float _canFire = -1;
        [SerializeField]
        private float TripleShotCoolDownRate = 5.0f;
        [SerializeField]
        private float SpeedBoostCoolDownRate = 5.0f;


        [SerializeField]
        private int _lives = 3;

        private SpawnManager spawner;
        private UIManager uiManager;


        private bool isTripleShotActive = false;
        private bool isSpeedBoostActive = false;
        [HideInInspector]
        public bool IsShieldActive = false;


        public int score = 0;

        private AudioClips AC;

        public int ShieldHits = 2;

        private SpriteRenderer NewShieldColor;

        [SerializeField]
        Color[] ShieldColors;
        [HideInInspector]
        public bool ShieldDestroyed = false;

        [SerializeField]
        public int maxAmmo = 15;

        public int currentAmmo;
        [HideInInspector]
        public bool hasAmmo = true;

        #endregion

        #region BuiltIn Methods
        void Start()
        {
            originalSpeed = speed;
            Shields.GetComponent<SpriteRenderer>().enabled = false;
            Shields.GetComponent<Collider2D>().enabled = false;
            ShieldHits = 0;
            currentAmmo = maxAmmo;

            transform.position = new Vector3(0, 0, 0);

            spawner = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
            AC = GameObject.Find("AudioManager").GetComponent<AudioClips>();
            //NewShieldColor = GameObject.Find("Shields").GetComponent<SpriteRenderer>();

            if (uiManager == null)
            {
                Debug.Log("The UI Manager Is NULL");
            }

            if (spawner == null)
            {
                Debug.LogError("The Spawn Manager Is NULL");
            }

            if (AC == null)
            {
                Debug.LogError("No Audio source or clip");
            }

            if (NewShieldColor == null)
            {
                Debug.Log("There are no shields on start");
            }

        }
        void Update()
        {
            FireLaser();
            CalculateMovement();
            ButtonBoost();
        }
        #endregion

        #region Custom Methods

        #region Fire Laser
        void FireLaser()
        {
            Vector3 offset = new Vector3(0f, 1.0f, 0f);
            AmmoCount();
            if (Input.GetButtonDown("Fire1") && hasAmmo == false)
            {
                AC.GetNoAmmoAudio();
            }
            if (Input.GetButtonDown("Jump") && hasAmmo == false)
            {
                AC.GetNoAmmoAudio();
            }
            if (isTripleShotActive == true)
            {
                if (Input.GetButtonDown("Fire1") && Time.time > _canFire && hasAmmo == true)
                {
                    currentAmmo -= 1;
                    _canFire = Time.time + _fireRate;
                    Instantiate(_tripleShot, transform.position, Quaternion.identity);
                    AC.GetLaserAudioClip();
                }
                if (Input.GetButtonDown("Jump") && Time.time > _canFire && hasAmmo == true)
                {
                    currentAmmo -= 1;
                    _canFire = Time.time + _fireRate;
                    Instantiate(_tripleShot, transform.position, Quaternion.identity);
                    AC.GetLaserAudioClip();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1") && Time.time > _canFire && hasAmmo == true)
                {
                    currentAmmo -= 1;
                    _canFire = Time.time + _fireRate;
                    Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
                    AC.GetLaserAudioClip();
                }

                if (Input.GetButtonDown("Jump") && Time.time > _canFire && hasAmmo == true)
                {
                    currentAmmo -= 1;
                    _canFire = Time.time + _fireRate;
                    Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
                    AC.GetLaserAudioClip();
                }
            }
            uiManager.UpdateAmmo(currentAmmo);

        }

        #region TripleShot

        public void TripleShotActive()
        {
            isTripleShotActive = true;
            StartCoroutine(TripleShotPowerDownRoutine());
        }

        #region Speed Boost

        public void SpeedBoostActive()
        {
            isSpeedBoostActive = true;
            StartCoroutine(SpeedBoostPowerDownRoutine());
            if (isSpeedBoostActive == true)
            {
                speed = newspeed;
            }
            else
                speed = originalSpeed;
        }

        #endregion

        #region Shields

        public void ShieldsActive()
        {
            TurnShieldComponentsOn();
            NewShieldColor = Shields.GetComponent<SpriteRenderer>();
            IsShieldActive = true;
            ShieldHits = 3;
            NewShieldColor.color = ShieldColors[0];
            uiManager.UpdateShieldLives(3);
        }

        #endregion


        #endregion

        #region IEnum

        IEnumerator TripleShotPowerDownRoutine()
        {
            yield return new WaitForSeconds(TripleShotCoolDownRate);
            isTripleShotActive = false;
        }

        IEnumerator SpeedBoostPowerDownRoutine()
        {
            yield return new WaitForSeconds(SpeedBoostCoolDownRate);
            speed = originalSpeed;
            isSpeedBoostActive = false;
        }

        #endregion

        #endregion

        #region Movement
        void CalculateMovement()
        {
            #region Input
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);
            // Optimize
            // Create a new Vector3 variable and set it to the proper inputs
            // Vector3 direction = new Vector3 (horizontalInput, verticalInput, 0)
            // Then translate the transform by the new Vector3 variable
            // transform.Translate (direction * speed * Time.deltaTime)
            #endregion

            #region Screen Constraint
            // variable float positions
            float posX = transform.position.x;
            float posY = transform.position.y;

            //Vector3 Pos = new Vector3(posX, posY, 0);
            // Variable vector
            // Vector3 Pos = new Vector3 (posX, posY, 0)
            // Didn't work. Did but didn't. Tried posY = 0 in if and then transform.position = pos but it didn't stop at 0 and could continue to move.
            // The variable axes work though
            //if (posY >= 0)
            //{
            //    posY = 0;
            //    transform.position = Pos;
            //    transform.position = Pos(posX, posY, 0);
            //    transform.position = Pos(posX, 0, 0);
            //    transform.position = new Pos;
            //    transform.position = new Pos (posX, posY, 0);
            //    transform.position = new Pos (posX, 0, 0);
            //    pos = (posX, 0, 0);
            //}

            if (posY >= 0)
            {
                transform.position = new Vector3(posX, 0, 0);
            }
            else if (posY <= -4)
            {
                posY = -4;
                transform.position = new Vector3(posX, -4, 0);
            }

            // Could just clamp posY^^

            if (posX >= 11.3f)
            {
                transform.position = new Vector3(-11.3f, posY, 0);
            }
            else if (posX <= -11.3f)
            {
                transform.position = new Vector3(11.3f, posY, 0);
            }
            #endregion
        }
        #endregion

        #region Damage

        public void Damage()
        {
            GameObject REngine = engines[0];
            GameObject LEngine = engines[1];

            if (IsShieldActive == true)
            {
                
                TurnShieldComponentsOn();
                NewShieldColor = Shields.GetComponent<SpriteRenderer>();
                if (AC == null)
                {
                    Debug.LogError("No Audio source or clip");
                }
                if (ShieldHits == 3)
                {
                    ShieldDamage();
                    ShieldDestroyed = false;
                    NewShieldColor.color = ShieldColors[0];
                    uiManager.UpdateShieldLives(3);
                }
                if (ShieldHits == 2)
                {
                    ShieldDamage();
                    ShieldDestroyed = false;
                    NewShieldColor.color = ShieldColors[1];
                    uiManager.UpdateShieldLives(2);
                    return;
                }
                if (ShieldHits == 1)
                {
                    ShieldDamage();
                    ShieldDestroyed = false;
                    NewShieldColor.color = ShieldColors[2];
                    uiManager.UpdateShieldLives(1);
                    return;
                }
                else
                {
                    IsShieldActive = false;
                    TurnShieldComponentsOff();
                    Debug.Log("ShieldDestroyed");
                    uiManager.UpdateShieldLives(0);
                    return;
                }
            }

            _lives -= 1;

            if (_lives == 2)
            {

                GameObject randomEngine = engines[Random.Range(0, engines.Length)];
                engineDamage = randomEngine.GetComponent<EngineDamage>();
                if (randomEngine.GetComponent<SpriteRenderer>() == null)
                {
                    Debug.LogError("Engine Damage Script is Null");
                }
                if (engineDamage.isEngineActive == true)
                {
                    randomEngine.GetComponent<SpriteRenderer>().enabled = true;
                    engineDamage.EnableEngineDamage();
                    Debug.Log(_lives);
                }
            }
            if (_lives == 1)
            {
                SpriteRenderer RengineRenderer = engines[0].GetComponent<SpriteRenderer>();
                SpriteRenderer LengineRenderer = engines[1].GetComponent<SpriteRenderer>();
                Debug.Log(_lives);
                GameObject randomEngine = engines[Random.Range(0, engines.Length)];
                engineDamage = randomEngine.GetComponent<EngineDamage>();
                if (randomEngine.GetComponent<EngineDamage>() == null)
                {
                    Debug.LogError("Engine Damage Script is Null");
                }
                if (RengineRenderer.enabled == false)
                {
                    RengineRenderer.enabled = true;
                    engineDamage.EnableEngineDamage();
                }

                if (LengineRenderer.enabled == false)
                {
                    LengineRenderer.enabled = true;
                    engineDamage.EnableEngineDamage();
                }

            }

            uiManager.UpdateLives(_lives);

            if (_lives <= 0)
            {
                spawner.OnPlayerDeath();
                Destroy(this.gameObject);
                AC.GetExplosionAudio();
            }
        }

        #endregion

        #region Score

        public void AddScore(int points)
        {
            score += points;
            uiManager.UpdateScore(score);
        }

        #endregion

        #region ButtonBoost

        void ButtonBoost()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = speed * 2;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = originalSpeed;
            }
        }

        #endregion


        #region Shield Behavior
        public void ShieldDamage()
        {
            ShieldHits -= 1;
        }

        void TurnShieldComponentsOn()
        {
            Shields.GetComponent<Collider2D>().enabled = true;
            Shields.GetComponent<SpriteRenderer>().enabled = true;
        }

        void TurnShieldComponentsOff()
        {
            Shields.GetComponent<Collider2D>().enabled = false;
            Shields.GetComponent<SpriteRenderer>().enabled = false;
        }

        #endregion

        #region AmmoBehavior

        public void AmmoCount()
        {
            if (currentAmmo <= 0)
            {
                currentAmmo = 0;
                hasAmmo = false;
                Debug.Log("No Ammo");
            }
            else
            {
                hasAmmo = true;
            }
        }

        #endregion
        #endregion
    }
}