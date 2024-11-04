using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] public float speed = 5f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float runCost = 20f;
    [SerializeField] private float staminaRechargeRate = 2f;
    [SerializeField] public float attackCost = 10f;


    private float stamina;
    public float Stamina
    {
        get => stamina;
        set => stamina = Mathf.Max(0, value); // Setter with validation
    }

    [Header("UI")]
    [SerializeField] private Image staminaBar;

    private Coroutine rechargeCoroutine;
    private Transform playerTransform;
    public bool isRecharging;
    
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

        if (isRunning && stamina > 0)
        {
            Move(moveDir * 2); // Double speed when running
            Stamina -= runCost * Time.deltaTime;
            UpdateStaminaBar();
            StartRechargeCoroutine();
        }
        else
        {
            Move(moveDir);
            StartRechargeCoroutine();
        }
    }

    private Vector3 GetInputDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        return new Vector3(horizontal, vertical).normalized;
    }

    private void Move(Vector3 direction)
    {
        playerTransform.position += direction * speed * Time.deltaTime;
    }

    private void StartRechargeCoroutine()
    {
        if (stamina < maxStamina && !isRecharging)
        {
            isRecharging = true;
            if (rechargeCoroutine != null)
            {
                StopCoroutine(rechargeCoroutine);
            }
            rechargeCoroutine = StartCoroutine(RechargeStamina());
        }
    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1.5f);
        while (stamina < maxStamina)
        {
            Stamina += staminaRechargeRate * Time.deltaTime;
            UpdateStaminaBar();
            yield return null; // Wait for the next frame
        }
        isRecharging = false; // Reset the recharging state
    }

   public void UpdateStaminaBar()
    {
        staminaBar.fillAmount = stamina / maxStamina;
    }
}
