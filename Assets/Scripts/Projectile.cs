

using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BreakableBlock")) {
            Debug.Log("porjectile collision");
        }
    }
}