using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public InputActionReference moveAction;

    public CharacterController charCon;

    public float moveSpeed;

    private float ySpeed = 0f;

    public InputActionReference jumpAction;
    public float jumpForce;

    public InputActionReference lookAction;
    private float horiRot, vertRot;
    public float lookSpeed;
    public Camera theCam;
    public float minLookAngle, maxLookAngle;

    public LayerMask whatIsStock;
    public float interactionRange;

    private StockObject heldPickup;
    public Transform holdPoint;

    public float throwForce;

    public LayerMask whatIsShelf;

    public LayerMask whatIsStockBox;
    public StockBoxController heldBox;
    public Transform boxHoldPoint;

    public float waitToPlaceStock;
    private float placeStockCounter;

    public LayerMask whatIsBin;

    public LayerMask whatIsFurniture;
    public Transform furniturePoint;
    public GameObject heldFurniture;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(UIController.instance.updatePricePannel != null)
        {
            if(UIController.instance.updatePricePannel.activeSelf == true)
            {
                return;
            }
        }

        if(UIController.instance.buyMenueScreen != null)
        {
            if(UIController.instance.buyMenueScreen.activeSelf == true)
            {
                return;
            }
        }


        Vector2 lookinput = lookAction.action.ReadValue<Vector2>();

        horiRot = horiRot + lookinput.x * Time.deltaTime * lookSpeed;
        transform.rotation = Quaternion.Euler(0f, horiRot, 0f);

        vertRot -= lookinput.y * Time.deltaTime * lookSpeed;
        vertRot = Mathf.Clamp(vertRot, minLookAngle, maxLookAngle);
        theCam.transform.localRotation = Quaternion.Euler(vertRot, 0f, 0f);

        
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        //Debug.Log(moveInput);

        //transform.position = transform.position + new Vector3(moveInput.x * Time.deltaTime * moveSpeed, 0f, moveInput.y * Time.deltaTime * moveSpeed);

        //Vector3 moveAmount = new Vector3(moveInput.x, 0f, moveInput.y);

        Vector3 vertMove = transform.forward * moveInput.y;
        Vector3 horiMove = transform.right * moveInput.x;

        Vector3 moveAmount = horiMove + vertMove;
        moveAmount = moveAmount.normalized;

        moveAmount = moveAmount * moveSpeed;

        if (charCon.isGrounded == true)
        {
            ySpeed = 0f;

            if (jumpAction.action.WasPressedThisFrame())
            {
                ySpeed = jumpForce;
            }
        }

        ySpeed = ySpeed + (Physics.gravity.y * Time.deltaTime);

       


        moveAmount.y = ySpeed;

        charCon.Move(moveAmount * Time.deltaTime);

        //check for pickup
        Ray ray = theCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        RaycastHit hit;

        /* if(Physics.Raycast(ray, out hit, interactionRange, whatIsStock))
         {
             Debug.Log("I see a pickup");
         } else
         {
             Debug.Log("I can't see anything!!!!");
         } */
        if (heldPickup == null && heldBox == null && heldFurniture == null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsStock))
                {
                    //Debug.Log("I see a pickup);

                    /* heldPickup = hit.collider.gameObject;
                    heldPickup.transform.SetParent(holdPoint);
                    heldPickup.transform.localPosition = Vector3.zero;
                    heldPickup.transform.localRotation = Quaternion.identity;

                    heldPickup.GetComponent<Rigidbody>().isKinematic = true; */

                    heldPickup = hit.collider.GetComponent<StockObject>();
                    heldPickup.transform.SetParent(holdPoint);
                    heldPickup.Pickup();

                    return;
                }

                if (Physics.Raycast(ray, out hit, interactionRange, whatIsStockBox))
                {
                    heldBox = hit.collider.GetComponent<StockBoxController>();

                    heldBox.transform.SetParent(boxHoldPoint);
                    heldBox.Pickup();

                    if (heldBox.flap1.activeSelf == true)
                    {
                        heldBox.OpenClose();
                    }
                    return;
                }

            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                {
                    heldPickup = hit.collider.GetComponent<ShelfSpaceController>().GetStock();

                    if(heldPickup != null)
                    {
                        heldPickup.transform.SetParent(holdPoint);
                        heldPickup.Pickup();
                    }

                    return;
                }

                if (Physics.Raycast(ray, out hit, interactionRange, whatIsStockBox))
                {
                    hit.collider.GetComponent<StockBoxController>().OpenClose();
                }
            }

            if(Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                {
                    hit.collider.GetComponent<ShelfSpaceController>().StartPriceUpdate();
                }
            }

            if(Keyboard.current.rKey.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsFurniture))
                {
                    heldFurniture = hit.transform.gameObject;

                    heldFurniture.transform.SetParent(furniturePoint);
                    heldFurniture.transform.localPosition = Vector3.zero;
                    heldFurniture.transform.localRotation = Quaternion.identity;
                }
            }
        }
        else
        {
            if (heldPickup != null)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                    {
                        /*heldPickup.transform.position = hit.transform.position;
                        heldPickup.transform.rotation = hit.transform.rotation;


                        heldPickup.transform.SetParent(null);
                        heldPickup = null; */

                        /* heldPickup.MakePlaced();

                        heldPickup.transform.SetParent(hit.transform);
                        heldPickup = null; */

                        hit.collider.GetComponent<ShelfSpaceController>().PlaceStock(heldPickup);

                        if (heldPickup.isPlaced == true)
                        {
                            heldPickup = null;
                        }
                    }
                }

                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    //Rigidbody pickupRB = heldPickup.GetComponent<Rigidbody>();
                    //pickupRB.isKinematic = false;
                    heldPickup.Release();

                    heldPickup.theRB.AddForce(theCam.transform.forward * throwForce, ForceMode.Impulse);

                    heldPickup.transform.SetParent(null);
                    heldPickup = null;
                }
            }

            if(heldBox != null)
            {
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                   
                    heldBox.Release();
                    heldBox.theRB.AddForce(theCam.transform.forward * throwForce, ForceMode.Impulse);

                    heldBox.transform.SetParent(null);
                    heldBox = null;
                }

                if(Keyboard.current.eKey.wasPressedThisFrame)
                {
                    heldBox.OpenClose();
                }

                if(Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (heldBox.stockInBox.Count > 0)
                    {

                        if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                        {
                            heldBox.PlaceStockOnShelf(hit.collider.GetComponent<ShelfSpaceController>());

                            placeStockCounter = waitToPlaceStock;
                        }
                    } else
                    {
                        if (Physics.Raycast(ray, out hit, interactionRange, whatIsBin))
                        {
                            Destroy(heldBox.gameObject);

                            heldBox = null;
                        }
                    }


                }

                if(Mouse.current.leftButton.isPressed)
                {
                    placeStockCounter -= Time.deltaTime;

                    if(placeStockCounter <= 0)
                    {
                        if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                        {
                            heldBox.PlaceStockOnShelf(hit.collider.GetComponent<ShelfSpaceController>());

                            placeStockCounter = waitToPlaceStock;
                        }
                    }
                }
            }

            if(heldFurniture != null)
            {
                heldFurniture.transform.position = new Vector3(furniturePoint.position.x, 0f, furniturePoint.position.z);
                heldFurniture.transform.LookAt(new Vector3(transform.position.x, 0f, transform.position.z));

                if(Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.rKey.wasPressedThisFrame)
                {
                    heldFurniture.transform.SetParent(null);
                    heldFurniture = null;
                }
            }
        }
    }
}
