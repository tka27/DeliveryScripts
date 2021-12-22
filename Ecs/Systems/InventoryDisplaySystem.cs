using Leopotam.Ecs;


sealed class InventoryDisplaySystem : IEcsRunSystem
{

    SceneData sceneData;
    BuildingsData buildingsData;
    UIData uiData;
    void IEcsRunSystem.Run()
    {

        if (sceneData.buildCam.position.y <= SceneData.BUILDCAM_Y_THRESHOLD && sceneData.gameMode == GameMode.View && !uiData.inventoryCanvas.activeSelf)
        {
            uiData.inventoryCanvas.SetActive(true);
            UIData.UpdateUI();

        }
        else if (uiData.inventoryCanvas.activeSelf && sceneData.buildCam.position.y > SceneData.BUILDCAM_Y_THRESHOLD || sceneData.gameMode != GameMode.View && uiData.inventoryCanvas.activeSelf)
        {
            uiData.inventoryCanvas.SetActive(false);
            UIData.UpdateUI();
        }
    }
}
