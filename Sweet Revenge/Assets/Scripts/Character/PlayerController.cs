using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float runCost = 20f;
    [SerializeField] private float staminaRechargeRate = 2f;
   
    private float stamina;
    public float Stamina
    {
        get => stamina; // Getter
        set => stamina = Mathf.Max(0, value); // Setter with validation (e.g., no negative values)
    }

    [Header("UI")]
    [SerializeField] private Image staminaBar;

    public Coroutine rechargeCoroutine;
    private Transform playerTransform;
    public bool recharging;
    public bool r = true;
    public bool canRotate = true;

    private void Awake()
    {
        stamina = maxStamina;
        playerTransform = transform;
    }

    void Update()
    {
        HandleRotation();
        HandleMovement();
    }
    private void HandleRotation()
    {
        Vector3 targetRotation = Input.mousePosition - Camera.main.WorldToScreenPoint(playerTransform.position);
        float angle = Mathf.Atan2(targetRotation.y, targetRotation.x) * Mathf.Rad2Deg;
        playerTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void HandleMovement()
    {
        Vector3 moveDir = GetInputDirection();
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        if(!isRunning)
        {
         r = true;   
        }
        if (isRunning && stamina > 0)
        {
            recharging = false;
            Move(moveDir * 2); // Double speed when running
            stamina -= runCost * Time.deltaTime;
            if (stamina < 0) stamina = 0;
            UpdateStaminaBar();
            // Start recharge coroutine if stamina is above 0 and not already running
            if (stamina < maxStamina)
            {
                rechargeCoroutine = StartCoroutine(RechargeStamina());
            }
        }
        else
        {
            Move(moveDir);
            // Start recharge coroutine if not running and stamina is below max
            if (stamina < maxStamina && !recharging && r)
            {
                rechargeCoroutine = StartCoroutine(RechargeStamina());
            }
        }
    }

    private Vector3 GetInputDirection()
    {
        //vector normalized for direction
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        return new Vector3(horizontal, vertical).normalized;
    }
    private void Move(Vector3 direction)
    {
        playerTransform.position += direction * speed * Time.deltaTime;
    }

    public IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1.5f);
        if (r)
        {
        while (stamina < maxStamina)
        {
            recharging = true;
            stamina += staminaRechargeRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina,0, maxStamina); // Clamp stamina to maxStamina
            UpdateStaminaBar();
            yield return null; // Wait for the next frame
        }
        if(stamina == maxStamina)
        {
            r = false;    
        }
        }

        
    }

    public void UpdateStaminaBar()
    {
        staminaBar.fillAmount = stamina/maxStamina;
    }
}
