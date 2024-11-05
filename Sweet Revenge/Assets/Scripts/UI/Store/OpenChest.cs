using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Components")]
    public Animator animator; // Reference to the Animator component
    public GameObject chestUI; // Reference to the chest UI
    public GameObject circlePrefab; // Reference to the circle effect prefab
    public Transform itemSpawnPoint; // Where to spawn the items

    private bool canOpen = false; // Can the chest be opened?
    private float cooldownTime = 2f; // Cooldown time in seconds
    private float lastOpenTime = 0f; // Last time the chest was opened

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        animator.SetBool("Open",false);
    }

    public void OpenChest()
    {
        if (canOpen && Time.time >= lastOpenTime + cooldownTime)
        {
            animator.SetBool("Open",true); // Trigger the opening animation
            lastOpenTime = Time.time; // Update the last open time
            canOpen = false; // Prevent further openings until items are spawned
            StartCoroutine(CloseChestAfterDelay(2f));
        }
    }

    private IEnumerator CloseChestAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("Open", false); // Set the "Open" boolean to false to close the chest
    }

    public void ToggleChest()
    {
        chestUI.SetActive(!chestUI.activeSelf); // Toggle the chest UI visibility
    }

    public void SpawnItem(GameObject itemPrefab)
    {
        // Instantiate the circle effect
        GameObject circleEffect = Instantiate(circlePrefab, transform.position, Quaternion.identity);
        CircleMovement circleMovement = circleEffect.GetComponent<CircleMovement>();
        circleMovement.target = itemSpawnPoint; // Set the target to the item spawn point

        // Spawn the item after a short delay to allow the animation to play
        StartCoroutine(SpawnItemWithDelay(itemPrefab, 0.5f)); // Adjust delay as needed
    }

    private IEnumerator SpawnItemWithDelay(GameObject itemPrefab, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(itemPrefab, itemSpawnPoint.position, Quaternion.identity); // Spawn the item at the spawn point
    }

    public void SetCanOpen(bool value)
    {
        canOpen = value; // Set the ability to open the chest
    }
}
