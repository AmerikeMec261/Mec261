using UnityEngine;

public interface IEnemyInfo //Esta interfaz onliga a cualquier clase que la tenga en mostrar las estadisticas del enemigo
{
    public void Data(); //Es la funcion que muestra la informacion de los enemigos en la consola
    
    //Estos son los valores que devuelven un valor actual de los datos del enemigo tanto como vida, escudo y velocidad
    float Life();
    float Shield();
    float Speed();
}
