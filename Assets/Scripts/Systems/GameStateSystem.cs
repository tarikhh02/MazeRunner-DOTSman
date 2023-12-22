using Unity.Entities;
using UnityEngine;

public partial struct GameStateSystem : ISystem
{
    EntityQuery collectableQuery;
    EntityQuery playerQuery;
    
    public void OnUpdate(ref SystemState systemState)
    {
        if (MonoBehaviour.FindAnyObjectByType<UIManager>() == null)
            return;

        if (!UIManager.Instance.canPlayerPlay)
        { 
            if (playerQuery.IsEmpty)
                return;
            else
                UIManager.Instance.canPlayerPlay = true;
        }

        if(collectableQuery.IsEmpty)
        {
            UIManager.Instance.Win();
            AudioManager.instance.PlayMusic("win", false);
            AudioManager.instance.isGameFinished = true;
            if(!playerQuery.IsEmpty)
                systemState.EntityManager.DestroyEntity(playerQuery.GetSingletonEntity());
        }
        else if(playerQuery.IsEmpty)
        {
            UIManager.Instance.Lose();
            AudioManager.instance.PlayMusic("lose", false);
            AudioManager.instance.isGameFinished = true;
        }
        else
        {
            string score = "Points: " + playerQuery.GetSingleton<Points>().points;
            string pellets = "Pellets: " + collectableQuery.CalculateEntityCount().ToString();
            UIManager.Instance.UpdateGameUI(pellets, score);
        }
    }

    public void OnCreate(ref SystemState systemState)
    {
        collectableQuery = systemState.GetEntityQuery(ComponentType.ReadOnly<Collectable>());
        playerQuery = systemState.GetEntityQuery(ComponentType.ReadOnly<Player>(), ComponentType.ReadOnly<Points>());
    }

    public void OnDestroy(ref SystemState systemState)
    {

    }
}