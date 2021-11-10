using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.Player
{
    public class Player : MonoBehaviour
    {
        #region Variables
        public float speed = 3.5f;

        public float horizontalInput;
        public float verticalInput;
        #endregion

        #region BuiltIn Methods
        void Start()
        {
            transform.position = new Vector3 (0, 0, 0);
        }
        void Update()
        {
            CalculateMovement();
        }
        #endregion

        #region Custom Methods

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
        #endregion
    }
}
