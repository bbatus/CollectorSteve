using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 180f;
    public GameObject player;
    public GameObject playerChild;
    private Rigidbody rb;
    Sequence collectObjSequence;
    [SerializeField] GameObject distrubatedObject;

    private int yellowAmount = 0;
    private int redAmount = 0;
    private int greenAmount = 0;
    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (UIManager.instance.levelState == LevelState.Playing)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
{
    float horizontalInput = 0f;
    float verticalInput = 0f;

#if UNITY_EDITOR
    horizontalInput = Input.GetAxis("Horizontal");
    verticalInput = Input.GetAxis("Vertical");
#else
    if (Input.touchCount > 0)
    {
        JoystickPlayerExample.instance.Move();
        Touch touch = Input.GetTouch(0);
        horizontalInput = Mathf.Clamp(Input.touches[0].deltaPosition.x, -1f, 1f);
        verticalInput = Mathf.Clamp(Input.touches[0].deltaPosition.y, -1f, 1f);
    }
#endif

    Vector3 forward = transform.forward;
    Vector3 right = transform.right;

    Vector3 movement = (forward * verticalInput + right * horizontalInput) * moveSpeed * Time.fixedDeltaTime;

    rb.MovePosition(rb.position + movement);

    if (movement != Vector3.zero)
    {
        Quaternion toRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime));
    }
}


    public void ObjCollectMove(GameObject obj, ObjType objType)
    {
        Vector3 initialPosition = obj.transform.localPosition;
        obj.transform.SetParent(null); 

        Vector3 targetPosition = initialPosition + new Vector3(0f, 5f, 0f);

        if (objType == ObjType.Yellow)
        {
            yellowAmount++;
            UIManager.instance.YellowAmountTextChange(yellowAmount);
        }
        else if (objType == ObjType.Green)
        {
            greenAmount++;
            UIManager.instance.GreenAmountTextChange(greenAmount);
        }
        else if (objType == ObjType.Red)
        {
            redAmount++;
            UIManager.instance.RedAmountTextChange(redAmount);
        }

        collectObjSequence = DOTween.Sequence().SetAutoKill(false);
        collectObjSequence.Append(obj.transform.DOLocalJump(targetPosition, 1f, 1, 0.5f).OnComplete(() =>
        {
            Debug.Log($"Eklendi");
            Taptic.Light();

            
            obj.transform.SetParent(playerChild.transform);
        }));
        collectObjSequence.Join(obj.transform.DOScale(new Vector3(1f, 1f, 1f), .5f));


    }

    public void BringObjectToPool(GameObject pool, PoolType poolType)
    {
        if (poolType == PoolType.GreenPool)
        {
            DistrubateGreen(gameObject, ObjType.Green);
        }
        // else if (poolType == PoolType.RedPool)
        // {
        //     DistrubateRed(gameObject, ObjType.Red);
        // }
        // else if (poolType == PoolType.YellowPool)
        // {
        //     DistrubateYellow(gameObject, ObjType.Yellow);
        // }
    }
    public void DistrubateGreen(GameObject obj, ObjType objType)
    {
        if (objType == ObjType.Green || objType == ObjType.Red || objType == ObjType.Yellow)
        {
            DistributeObject(obj);
        }
    }

    // public void DistrubateRed(GameObject obj, ObjType objType)
    // {
    //     if (objType == ObjType.Red)
    //     {
    //         DistributeObject(obj);
    //     }
    // }

    // public void DistrubateYellow(GameObject obj, ObjType objType)
    // {
    //     if (objType == ObjType.Yellow)
    //     {
    //         DistributeObject(obj);
    //     }
    // }

    private void DistributeObject(GameObject obj)
{
    // Access the child objects of playerChild
    foreach (Transform child in playerChild.transform)
    {
        // Check if the child object has the CollectObjects component
        CollectObjects collectObjects = child.GetComponent<CollectObjects>();

        if (collectObjects != null)
        {
            // Perform distribution logic for objects
            // For example, you can move the object to a specific position
            child.DOMove(new Vector3(-6f, 1f, 21f), 1.5f).OnComplete(() =>
            {
                // Update any relevant UI or perform other actions
                Debug.Log($"{collectObjects.objType} object distributed!");
                Taptic.Light();
                UIManager.instance.GemTextUpdate(true, 50); // Adjust the gem amount as needed

                // Check if the child transform is still valid before attempting to destroy
                if (child != null && child.gameObject != null)
                {
                    // Destroy the object after 2 seconds
                    float delay = 0.2f;
                    DOVirtual.DelayedCall(delay, () =>
                    {
                        // Check again before destroying
                        if (child != null && child.gameObject != null)
                        {
                            Destroy(child.gameObject);
                        }
                    });
                }
            });
        }
    }
}

}
