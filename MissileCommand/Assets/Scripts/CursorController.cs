using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab;
    [SerializeField] GameObject missileLauncherPrefab;

    AudioSource myAudio;

    [SerializeField] private Texture2D cursorTexture;
    private Vector2 cursorHotspot;

    private GameController myGameController;

    void Start()
    {
        myGameController = FindObjectOfType<GameController>();
        
        myAudio = GetComponent<AudioSource>();

        cursorHotspot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && myGameController.currentMissilesLoadedInLauncher > 0 && !myGameController.isGameOver)
        {
            // Instantiate the missile
            GameObject newMissile = Instantiate(missilePrefab, missileLauncherPrefab.transform.position, Quaternion.identity);
            //myGameController.playerMissilesRemaining--;
            myGameController.PlayerFiredMissile();
            myAudio.Play();
            

            // Calculate the direction towards the mouse position
            Vector3 targetDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(missileLauncherPrefab.transform.position);
            targetDirection.z = 0f; // Ensure z component is zero since we're working in 2D

            // Calculate the rotation angle towards the target
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

            // Apply rotation to the missile, considering the red tip is at the top
            newMissile.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }
    }








} //class
