using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickCtrl : MonoBehaviour
{
    private RectTransform tr;
    private Vector3 startPos;
    private float radius;
    private bool isTouch = false;
    private int touchID = -1;
    public Vector3 differ;
    public Vector3 normalDiffer;
    private PlayerCtrl player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        tr = GetComponent<RectTransform>();
        startPos = tr.position;
        radius = 140.0f;
    }

    void FixedUpdate()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            HandTouchInput();
        }
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            HandTouch(Input.mousePosition);
        }
    }
    public void OnTouchDown()
    {
        isTouch = true;
    }
    public void OnTouchUp()
    {
        isTouch = false;
        tr.position = startPos;
        HandTouch(startPos);
    }
    void HandTouchInput()
    {
        int i = 0;
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                i++;
                Vector3 movePos = new Vector3(touch.position.x, touch.position.y);
                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x < startPos.x + radius && touch.position.x > startPos.x - radius && 
                    touch.position.y < startPos.y + radius && touch.position.y > startPos.y - radius)
                    {
                        touchID = i;
                    }
                }
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (touchID == i)
                    {
                        HandTouch(movePos);
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if (touchID == i)
                    {
                        touchID = -1;
                    }
                }
                
            }
        }
    }
    void HandTouch(Vector3 input)
    {
        if (isTouch)
        {
            Vector3 movePos = input - startPos;
            if (movePos.sqrMagnitude < radius * radius)
            {
                tr.position = startPos + movePos;
            }
            else
            {
                movePos = movePos.normalized * radius;
                tr.position = startPos + movePos;
            }   
        }
        else
        {
            tr.position = startPos;
        }
        differ = tr.position - startPos;
        normalDiffer = new Vector3(differ.x / radius, differ.y / radius);
        if (player != null)
        {
            player.GetStickPos(normalDiffer);
        }
    }
}
