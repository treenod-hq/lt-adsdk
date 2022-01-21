using System;

namespace Treenod.Ad
{
    public interface IAdSdk
    {
        void Initialize ( Action onComplete );

        void LoadRewardedAd ( Action<bool> onComplete );

        bool IsRewardedAdReady ();

        void ShowRewardedAd ( Action onRewarded, Action<bool> onClose );

        string GetRewardedAdUnitId ();

        void SubscribeOnDisplayRewardedAd ( Action onDisplayAd );

        void SubscribeOnRewardedAdImpression ( Action<string> onImpression );
    }
}