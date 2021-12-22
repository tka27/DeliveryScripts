using Leopotam.Ecs;
using UnityEngine.Advertisements;


sealed class AdSystem : IEcsInitSystem, IEcsDestroySystem
{
    StaticData staticData;
    const string SKIP = "Interstitial_Android";

    public void Init()
    {
        CarReturnBtns.returnEvent += AdCheck;
        Advertisement.Initialize("4492089");
    }
    public void Destroy()
    {
        CarReturnBtns.returnEvent -= AdCheck;
    }

    void AdCheck()
    {
        if (staticData.adProgress >= 100)
        {
            staticData.adProgress = staticData.adProgress % 100;
            if (Advertisement.IsReady(SKIP))
            {
                Advertisement.Show(SKIP);
            }
        }
    }


}



