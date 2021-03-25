using UnityEngine;
using TMPro;

public class GameStats : MonoBehaviour
{
    public TextMeshProUGUI totalTime;
    public TextMeshProUGUI deaths;
    public TextMeshProUGUI restarts;

    public TextMeshProUGUI totalCollisions;
    public TextMeshProUGUI wallCollisions;
    public TextMeshProUGUI debrisCollisions;
    public TextMeshProUGUI asteroidCollisions;
    public TextMeshProUGUI flamingAsteroidCollisions;

    public TextMeshProUGUI totalObjectsDestroyed;
    public TextMeshProUGUI debrisDestroyed;
    public TextMeshProUGUI asteroidsDestroyed;
    public TextMeshProUGUI flamingAsteroidsDestroyed;


    void Start()
    {
        totalTime.text = $"Final Time: {(int) GameController.totalTime / 60:00}:{GameController.totalTime % 60:00.000}";
        deaths.text = $"Deaths: {GameController.deaths}";
        restarts.text = $"Restarts: {GameController.restarts}";

        wallCollisions.text = $"Walls: {GameController.wallCollisions}";
        debrisCollisions.text = $"Debris: {GameController.debrisCollisions}";
        asteroidCollisions.text = $"Asteroids: {GameController.asteroidCollisions}";
        flamingAsteroidCollisions.text = $"Flaming Asteroids: {GameController.deaths}"; // Collisions with flame asteroids is the same as deaths
        totalCollisions.text = $"Total Collisions: {GameController.wallCollisions + GameController.debrisCollisions + GameController.asteroidCollisions + GameController.deaths}";

        debrisDestroyed.text = $"Debris: {GameController.debrisDestroyed}";
        asteroidsDestroyed.text = $"Asteroids: {GameController.asteroidsDestroyed}";
        flamingAsteroidsDestroyed.text = $"Flaming Asteroids: {GameController.flamingAsteroidsDestroyed}";
        totalObjectsDestroyed.text = $"Objects Destroyed: {GameController.debrisDestroyed + GameController.asteroidsDestroyed + GameController.flamingAsteroidsDestroyed}";

    }
}
