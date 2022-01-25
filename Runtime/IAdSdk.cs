using System;

namespace Treenod.Ads.AppLovin
{
    public interface IAdSdk
    {
        /// <summary>
        /// 광고 sdk 초기화 메서드
        /// </summary>
        /// <param name="onComplete">
        /// 초기화 완료시 호출
        /// </param>
        void Initialize ( Action onComplete );

        /// <summary>
        /// 광고 sdk에 광고를 로드하는 메서드
        /// </summary>
        /// <param name="onComplete">
        /// 광고 로드가 완료되면 호출
        /// </param>
        void LoadRewardedAd ( Action<bool> onComplete );

        /// <summary>
        /// 광고가 로드되었는지 확인하는 메서드
        /// </summary>
        /// <returns>로드 완료 여부</returns>
        bool IsRewardedAdReady ();

        /// <summary>
        /// 광고를 보여주는 메서드.
        /// onRewarded와 onClose 모두 광고가 종료되면 호출이 되며 onClose보다 onRewarded가 먼저 호출된다.
        /// </summary>
        /// <param name="onRewarded">
        /// 보상을 받을수 있을때 호출
        /// </param>
        /// <param name="onClose">
        /// 보상여부와 상관없이 광고가 종료되면 호출이 되며 정상적으로 종료시 true. 비정상으로 종료시 false를 반환
        /// </param>
        void ShowRewardedAd ( Action onRewarded, Action<bool> onClose );

        /// <summary>
        /// AppLovinSettingData 셋팅 파일에서 설정한 adUnitId 반환
        /// </summary>
        /// <returns></returns>
        string GetRewardedAdUnitId ();
        
        /// <summary>
        /// 광고가 시작될때 노출된 광고 정보를 받아야 할경우 설정한다. 
        /// </summary>
        /// <param name="onImpression">
        /// 광고 정보를 포함하는 콜백
        /// </param>
        void SubscribeOnRewardedAdImpression ( Action<AdInfo> onImpression );
        
        /// <summary>
        /// 광고가 시작될때 노출된 광고 정보를 받아야 할경우에 등록하였던 콜백 제거
        /// </summary>
        /// <param name="onDisplayAd"></param>
        void CancelSubscriptionOnRewardedAdImpression( Action<AdInfo> onImpression ); 

        /// <summary>
        /// 광고 본후 보상받을수 있을때 호출되는 콜백등록.
        /// ShowRewardedAd() 의 onRewarded콜백과 같은 타이밍에 호촐되며 ShowRewardedAd() 외에 이벤트 콜백등록이 필요할경우 사용 
        /// </summary>
        /// <param name="onReceived"></param>
        void SubscribeOnRewaredVideoReceived ( Action onReceived );
        
        /// <summary>
        /// 광고 본후 보상받을수 있을때 호출되는 콜백등록한것을 제거
        /// </summary>
        /// <param name="onDisplayAd"></param>
        void CancelSubscriptionOnRewaredVideoReceived( Action onReceived ); 
        
        /// <summary>
        /// 광고 종료시 콜백등록.
        /// ShowRewardedAd() 의 onRewarded콜백과 같은 타이밍에 호촐되며 ShowRewardedAd() 외에 이벤트 콜백등록이 필요할경우 사용 
        /// </summary>
        /// <param name="onReceived"></param>
        void SubscribeOnRewardedVideoClose ( Action onClose );
        
        /// <summary>
        ///광고 종료시 콜백등록한것을 제거
        /// </summary>
        /// <param name="onDisplayAd"></param>
        void CancelSubscriptionOnRewardedVideoClose( Action onClose ); 
    }
}