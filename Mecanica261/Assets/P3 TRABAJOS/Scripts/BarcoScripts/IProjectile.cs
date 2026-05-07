using UnityEngine;

public interface IProjectile //Esta es una interfaz que describe lo que debe hacer cualquier proyectil
{
    public void Fire(); //Inicia el disparo de la bala
    public void DealDamage(); // Aplica el dao al objetivo despues de impactar

    //Estos devuelven la velocidad y cantidad de daño de la bala
    public float Speed();
    public float Damage();

}
