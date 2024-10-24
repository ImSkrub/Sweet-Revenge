using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]   
    [SerializeField] private float speed;
    [SerializeField] private bool running = false;
    [SerializeField] private Transform player;
    [SerializeField] public float Stamina, MaxStamina;
    [SerializeField] public float AttackCost;
    [SerializeField] private float RunCost;
    [SerializeField] private float ChargeRate;

    [Header("UI")]
    [SerializeField] public Image StaminaBar;
    public Coroutine recharge;

    private Vector3 targetRotation;
    private Vector3 target;

    public bool canRotate = true;


    void Update()
    {
        targetRotation = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(targetRotation.y, targetRotation.x) * Mathf.Rad2Deg;
        if(canRotate) player.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        Move();
    }

  
    public IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (Stamina < MaxStamina)
        {
            Stamina += ChargeRate / 10f;
            StaminaBar.fillAmount = Stamina / MaxStamina;
            if (Stamina > MaxStamina)
            {
                Stamina = MaxStamina;
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    private void Move()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        var MoveDir = new Vector3(horizontal, vertical).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            running = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {

            running = false;
        }

        if (running && (MoveDir.x != 0 || MoveDir.y != 0) && Stamina > 0)
        {
            transform.position += MoveDir * Time.deltaTime * speed * 2;
            Stamina -= RunCost * Time.deltaTime;
            if (Stamina < 0) Stamina = 0;
            StaminaBar.fillAmount = Stamina / MaxStamina;

            if (recharge != null) StopCoroutine(recharge);


            recharge = StartCoroutine(RechargeStamina());
        }
        else
            transform.position += MoveDir * Time.deltaTime * speed;
    }
}
