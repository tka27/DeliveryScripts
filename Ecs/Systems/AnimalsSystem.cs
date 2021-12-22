using Leopotam.Ecs;

sealed class AnimalsSystem : IEcsRunSystem
{
    EcsFilter<Animal> animalFilter;

    void IEcsRunSystem.Run()
    {
        foreach (var animalInd in animalFilter)
        {
            ref var animal = ref animalFilter.Get1(animalInd);
            if (!animal.animalData.isAlive)
            {
                animalFilter.GetEntity(animalInd).Destroy();
                continue;
            }

            if (animal.animalData.agent.remainingDistance < 2)
            {
                animal.animalData.SetPath();
            }
        }
    }
}



