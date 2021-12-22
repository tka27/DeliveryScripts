using Leopotam.Ecs;
using UnityEngine;


sealed class EcsStartup : MonoBehaviour
{
    EcsWorld _world;
    EcsSystems systems;
    EcsSystems fixedSystems;
    public StaticData staticData;
    public SceneData sceneData;
    public BuildingsData buildingsData;
    public PathData pathData;
    public ProductData productData;
    public SoundData soundData;
    public GameSettings gameSettings;
    public FlowingText flowingText;
    public UIData uiData;

    void Start()
    {
        _world = new EcsWorld();
        systems = new EcsSystems(_world);
        fixedSystems = new EcsSystems(_world);
#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(systems);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(fixedSystems);
#endif
        SystemsDataInject();
        systems
            .Add(new GameInitSystem())
            .Add(new BuildingsInitSystem())
            .Add(new WheelsUpdateSystem())
            .Add(new CameraSwitchSystem())
            .Add(new DrawPathSystem())
            .Add(new ViewCameraSystem())
            .Add(new DestroyRoadSystem())
            .Add(new UpdateUISystem())
            .Add(new AutoServiceSystem())
            .Add(new BuySystem())
            .Add(new SellSystem())
            .Add(new UpdateCargoSystem())
            .Add(new FactoryProduceSystem())
            .Add(new RepriceSystem())
            .Add(new ClearInventorySystem())
            .Add(new CratesDisplaySystem())
            .Add(new InventoryDisplaySystem())
            .Add(new QuestUpdateSystem())
            .Add(new CarSoundSystem())
            .Add(new WorldCoinsReplaceSystem())
            .Add(new AnimalsSystem())
            .Add(new TrailFollowSystem())
            .Add(new ReturnToLastPointSystem())
            .Add(new AdSystem())
            .Add(new CarReturnBtnSwitchSystem())



            ;
        AddTutorialSystem();
        systems.Init();

        FixedSystemsDataInject();
        fixedSystems
            .Add(new PlayerMoveSystem())
            .Add(new DamageSystem())
            .Add(new FuelSystem())
            .Add(new ImmobilizeSystem())
            .Add(new FarmProduceSystem())
            .Add(new ShopQuestSystem())
            .Add(new ResearchSystem())
            .Add(new UpdateLabSystem())
            .Add(new ProductDestroyingSystem())





            .Init();
    }
    void SystemsDataInject()
    {
        systems
        .Inject(staticData)
        .Inject(sceneData)
        .Inject(buildingsData)
        .Inject(uiData)
        .Inject(productData)
        .Inject(soundData)
        .Inject(flowingText)
        .Inject(gameSettings)
        .Inject(pathData);
    }
    void FixedSystemsDataInject()
    {
        fixedSystems
        .Inject(staticData)
        .Inject(sceneData)
        .Inject(buildingsData)
        .Inject(uiData)
        .Inject(productData)
        .Inject(gameSettings)
        .Inject(pathData);
    }

    void AddTutorialSystem()
    {
        if (gameSettings.tutorialLvl < 0) return;

        systems.Add(new TutorialSystem());
    }








    void Update()
    {
        systems?.Run();

    }
    void FixedUpdate()
    {
        fixedSystems?.Run();
    }

    void OnDestroy()
    {
        if (systems != null)
        {
            systems.Destroy();
            systems = null;
            _world.Destroy();
            _world = null;
        }
    }
}
