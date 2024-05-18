using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{


    [SerializeField] private Texture2D cursorTexture;
    private Vector2 cursorHotspot;

    
    
    void Start()
    {
        cursorHotspot = new Vector2(cursorTexture.width / 2f,  cursorTexture.height / 2f);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
