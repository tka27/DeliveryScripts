using Leopotam.Ecs;


sealed class ImmobilizeSystem : IEcsRunSystem
{

    EcsFilter<Player, Movable, ImmobilizeRequest> reqestFilter;

    void IEcsRunSystem.Run()
    {
        foreach (var fPlayer in reqestFilter)
        {
            ref var player = ref reqestFilter.Get1(fPlayer);
            player.currentTorque = 0;
            foreach (var wheel in player.carData.drivingWheelColliders)
            {
                wheel.motorTorque = player.currentTorque;
            }
            foreach (var brakingWheel in player.carData.brakingWheelColliders)
            {

                brakingWheel.brakeTorque = 1;
            }
            foreach (var wheelData in player.carData.wheelDatas)
            {
                wheelData.particles.Stop();
            }
            reqestFilter.GetEntity(fPlayer).Del<Movable>();
        }
    }
}
