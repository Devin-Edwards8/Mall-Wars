using Unity.VisualScripting;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    [SerializeField] private int startHitPoints;
    [SerializeField] private Sprite damageBlock;
    [SerializeField] private Sprite healthyBlock;
    [SerializeField] private Player player;
    private int hitPoints;
    private SpriteRenderer sr;

    void Start()
    {
        hitPoints = startHitPoints;
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if(hitPoints == 0){
            gameObject.SetActive(false);
        }
    }

    public void DamageBlock(int damage)
    {
        hitPoints -= damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player enter");
        }
        if (other.CompareTag("Projectile"))
        {
            DamageBlock(1);
            sr.sprite = damageBlock;
            float count = 0;
            while(count < 100) {
                count += Time.deltaTime;
                continue; 
            }
            sr.sprite = healthyBlock;
            Destroy(other.gameObject);
        }
    }
}