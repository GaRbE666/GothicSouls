using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    public class CameraHandler : MonoBehaviour
    {
        #region FIELDS
        InputHandler inputHandler;
        PlayerManager playerManager;

        public Transform targetTransform;  //The transform the camera follows The player
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        public Transform myTransform;
        public Camera cameraObject;

        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers;
        public LayerMask enviromentLayer;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float leftAndRightLookSpeed = 250f;
        public float followSpeed = 1f;
        public float upAndDownLookSpeed = 250f;

        private float targetPosition;
        private float defaultPosition;

        private float leftAndRightAngle;
        private float upAndDownAngle;

        public float minimumLookUpAngle = -35;
        public float maximumLookUpAngle = 35;

        public float cameraSphereRaidus = 0.2f;
        public float cameraCollisionOffSet = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float lockedPivotPosition = 2.25f;
        public float unlockedPivotPosition = 1.65f;

        public CharacterManager currentLockOnTarget;

        List<CharacterManager> availableTargets = new List<CharacterManager>();
        public CharacterManager nearestLockOntarget;
        public CharacterManager leftLockTarget;
        public CharacterManager rightLockTarget;
        public float maximumLockOndistance = 30;
        #endregion

        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = (1 << 0 | 1 << 9);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
        }

        private void Start()
        {
            enviromentLayer = LayerMask.NameToLayer("Enviroment");
        }

        public void FollowTarget()
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, Time.deltaTime / followSpeed);
            myTransform.position = targetPosition;

            HandleCameraCollisions();
        }

        public void HandleCameraRotation()
        {
            if (inputHandler.lockOnFlag && currentLockOnTarget != null)
            {
                HandleLockedCameraRotation();
            }
            else
            {
                HandleStandardCameraRotation();
            }
        }

        public void HandleStandardCameraRotation()
        {
            leftAndRightAngle += inputHandler.mouseX * leftAndRightLookSpeed * Time.deltaTime;
            upAndDownAngle -= inputHandler.mouseY * upAndDownLookSpeed * Time.deltaTime;
            upAndDownAngle = Mathf.Clamp(upAndDownAngle, minimumLookUpAngle, maximumLookUpAngle);

            Vector3 rotation = Vector3.zero;
            rotation.y = leftAndRightAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = upAndDownAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleLockedCameraRotation()
        {
            Vector3 dir = currentLockOnTarget.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
        }

        private void HandleCameraCollisions()
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRaidus, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffSet);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, Time.deltaTime / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if (character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;
                    if (character.transform.root != targetTransform.transform.root 
                        && viewableAngle > -50 
                        && viewableAngle < 50 
                        && distanceFromTarget <= maximumLockOndistance)
                    {
                        if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            
                            if (hit.transform.gameObject.layer == enviromentLayer)
                            {
                                //Cannot lock onto target, object in the way
                            }
                            else
                            {
                                availableTargets.Add(character);
                            }
                        }
                    }
                }
            }

            for (int h = 0; h < availableTargets.Count; h++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[h].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOntarget = availableTargets[h];
                }

                if (inputHandler.lockOnFlag)
                {
                    Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTargets[h].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;

                    if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget && availableTargets[h] != currentLockOnTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[h];
                    }

                    if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget && availableTargets[h] != currentLockOnTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[h];
                    }
                }
            }
        }

        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            nearestLockOntarget = null;
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

            if (currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }
        }
    }
}
