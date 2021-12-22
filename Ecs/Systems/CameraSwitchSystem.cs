using Leopotam.Ecs;



sealed class CameraSwitchSystem : IEcsRunSystem
{
    SceneData sceneData;

    void IEcsRunSystem.Run()
    {
        if (sceneData.gameMode == GameMode.View && !sceneData.buildCam.gameObject.activeSelf || sceneData.gameMode == GameMode.Build && !sceneData.buildCam.gameObject.activeSelf) //camera view build
        {
            sceneData.buildCam.gameObject.SetActive(true);
        }
        else if (sceneData.gameMode == GameMode.Drive && sceneData.buildCam.gameObject.activeSelf) //camera drive
        {
            sceneData.buildCam.gameObject.SetActive(false);
        }
    }
}
