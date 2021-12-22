using Leopotam.Ecs;


sealed class CarReturnBtnSwitchSystem : IEcsRunSystem
{
    UIData uiData;
    EcsFilter<Player> playerFilter;
    void IEcsRunSystem.Run()
    {
        ref var player = ref playerFilter.Get1(0);
        if (player.currentTorque > player.maxTorque * 0.9f && player.playerRB.velocity.magnitude < 0.5f && !uiData.carReturnBtn.activeSelf)
        {
            uiData.carReturnBtn.SetActive(true);
        }
        else if (player.playerRB.velocity.magnitude > 2 && uiData.carReturnBtn.activeSelf)
        {
            uiData.carReturnBtn.SetActive(false);
        }
    }
}
