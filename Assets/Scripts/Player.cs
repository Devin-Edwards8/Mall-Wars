using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Player: MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefabs;
    private GameObject[] players;
    private int activePlayer;
    private bool isAiming;
    private Vector3 position;
    private float rotation;
    private bool touchingWall;

    public Vector3 Position { 
        get { return position; }
        set { position = value; }
    }
    public float Rotation { 
        get { return rotation; }
        set { rotation = value; }
    }

    public float MovementDirection
    {
        set {
            if(!isAiming)
            {
                rotation = value;
            }
        }
    }
    public bool IsAiming { 
        get { return isAiming; }
        set { isAiming = value; }
    }

    void Start()
    {
        isAiming = false;
        position = transform.position;
        rotation = 0;
        players = new GameObject[playerPrefabs.Length];
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            players[i] = Instantiate(playerPrefabs[i]);
            players[i].SetActive(false);
        }
        activePlayer = 0;
        players[activePlayer].SetActive(true);
    }

    void Update()
    {
        players[activePlayer].transform.position = position;
        players[activePlayer].transform.rotation = activePlayer switch
        {
            0 or 4 => Quaternion.Euler(0f, 0f, rotation-90),
            1 or 5 => rotation < 0 ? Quaternion.Euler(0f, 0f, rotation+180): Quaternion.Euler(0f, 0f, rotation-180),
            // 2 => Quaternion.Euler(0f, 0f, -rotation),
            _ => Quaternion.Euler(0f, 0f, rotation),
        };
        SetPlayerSprite();
    }

    private void SetPlayerSprite()
    {
        if(isAiming)
        {
            if(rotation >= 45 && rotation < 135)            // left attack
            {
                players[activePlayer].SetActive(false);
                activePlayer = 0;
                players[activePlayer].SetActive(true);
            }
            else if(rotation >= 135 && rotation <= 180 || rotation >= -180 && rotation <= -135)      // down attack
            {
                players[activePlayer].SetActive(false);
                activePlayer = 1;
                players[activePlayer].SetActive(true);
            }
            else if(rotation >= -135 && rotation < -45)      // right attack
            {
                players[activePlayer].SetActive(false);
                activePlayer = 2;
                players[activePlayer].SetActive(true);
            }
            else                                            // up attack
            {
                players[activePlayer].SetActive(false);
                activePlayer = 3;
                players[activePlayer].SetActive(true);
            }
            return;
        }
        if(rotation >= 45 && rotation < 135)             // left walk
        {
            players[activePlayer].SetActive(false);
            activePlayer = 4;
            players[activePlayer].SetActive(true);
        }
        else if(rotation >= 135 && rotation <= 180 || rotation >= -180 && rotation <= -135)      // down walk
        {
            players[activePlayer].SetActive(false);
            activePlayer = 5;
            players[activePlayer].SetActive(true);
        }
        else if(rotation >= -135 && rotation < -45)      // right walk
        {
            players[activePlayer].SetActive(false);
            activePlayer = 6;
            players[activePlayer].SetActive(true);
        }
        else                                            // up walk
        {
            players[activePlayer].SetActive(false);
            activePlayer = 7;
            players[activePlayer].SetActive(true);
        }
    }
}