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
        private GameObject ShieldsPrefab;
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
        private bool isShieldActive = false;
        private GameObject newShield;

        public int score = 0;

        #endregion

        #region BuiltIn Methods
        void Start()
        {
            originalSpeed = speed;

            transform.position = new Vector3 (0, 0, 0);

            spawner = GameObject.Find("SpawnManager"). GetComponent<SpawnManager>();
            uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

            if(uiManager == null)
            {
                Debug.Log("The UI Manager Is NULL");
            }

            if(spawner == null)
            {
                Debug.LogError("The Spawn Manager Is NULL");
            }

        }
        void Update()
        {
            FireLaser();
            CalculateMovement();
        }
        #endregion

        #region Custom Methods

        #region Fire Laser
        void FireLaser()
        {
            Vector3 offset = new Vector3(0f, 1.0f, 0f);

            if(isTripleShotActive == true)
            {
                if (Input.GetButtonDown("Fire1") && Time.time > _canFire)
                {
                    _canFire = Time.time + _fireRate;
                    Instantiate(_tripleShot, transform.position, Quaternion.identity);
                }
                if (Input.GetButtonDown("Jump") && Time.time > _canFire)
                {
                    _canFire = Time.time + _fireRate;
                    Instantiate(_tripleShot, transform.position, Quaternion.identity);
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1") && Time.time > _canFire)
                {
                    _canFire = Time.time + _fireRate;
                    Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
                }
                if (Input.GetButtonDown("Jump") && Time.time > _canFire)
                {
                    _canFire = Time.time + _fireRate;
                    Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
                }
            }
            
        }

        #region TripleShot

        public void TripleShotActive()
        {
            isTripleShotActive = true;
            StartCoroutine(TripleShotPowerDownRoutine());
        }

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

        public void ShieldsActive()
        {
            isShieldActive = true;

            if (isShieldActive == true)
            {
                newShield = Instantiate(ShieldsPrefab, transform.position, Quaternion.identity);
                newShield.transform.parent = this.transform;
            }
            else
                Debug.Log("No Shields");
        }


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
            if(isShieldActive == true)
            {
                isShieldActive = false;
                Destroy(newShield);
                return;
            }

            _lives -= 1;

            uiManager.UpdateLives(_lives);
                

            if(_lives <= 0)
            {
                spawner.OnPlayerDeath();
                Destroy(this.gameObject);
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

        #endregion
    }
}
