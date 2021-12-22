using Leopotam.Ecs;


sealed class UpdateLabSystem : IEcsRunSystem
{
    StaticData staticData;
    SceneData sceneData;
    EcsFilter<LabUpdateRequest, ResearchLab, ProductBuyer, Inventory> labFilter;

    void IEcsRunSystem.Run()
    {
        foreach (var labEntity in labFilter)
        {
            ref var lab = ref labFilter.Get2(labEntity);
            ref var labBuyer = ref labFilter.Get3(labEntity);
            ref var labInventory = ref labFilter.Get4(labEntity);


            lab.progress = 0;
            lab.requirement = ResearchLab.DEFAULT_REQUIREMENT + ResearchLab.DEFAULT_REQUIREMENT * sceneData.researchCurve.Evaluate(staticData.researchLvl);
            labInventory.maxMass = ResearchLab.DEFAULT_REQUIREMENT + ResearchLab.DEFAULT_REQUIREMENT * 0.5f * sceneData.researchCurve.Evaluate(staticData.researchLvl);


            labBuyer.buyingProductTypes.Clear();
            labBuyer.buyingProductTypes.Add(sceneData.researchList[staticData.researchLvl].type);


            labInventory.inventory.Clear();
            labInventory.inventory.Add(sceneData.researchList[staticData.researchLvl]);
            labBuyer.tradePointData.buyProductImage.sprite = labInventory.inventory[0].icon;



            labBuyer.tradePointData.labProgress.text = lab.progress.ToString() + "/" + lab.requirement.ToString();
            labFilter.GetEntity(labEntity).Del<LabUpdateRequest>();
        }
    }
}



