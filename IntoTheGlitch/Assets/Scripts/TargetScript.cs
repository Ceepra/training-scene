using UnityEngine;

public class TargetScript : MonoBehaviour
{
   public float health = 1f;

   public void TakeDamage(float amount)
   {
      health -= amount;
      if (health<=0f)
      {
         Destroy(gameObject);
      }
   }
}
