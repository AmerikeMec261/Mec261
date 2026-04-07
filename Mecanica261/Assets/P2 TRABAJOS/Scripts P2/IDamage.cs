using UnityEngine;

public interface IDamage //Esta es una interfaz que un objeto puede recibir daño
{
    public void TakeDamage(float damage); //Este es una funcion que se llama cuando se quiere hacer daño al objetivo
}
