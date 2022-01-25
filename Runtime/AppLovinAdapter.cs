using System;
using System.Collections.Generic;
using UnityEngine;

namespace Treenod.Ads.AppLovin
{
    public class AppLovinAdapter : IAdSdk
    {
        private bool _initialized;
        private AppLovinSettingData _settingData;
        private Action _onInitialize;
        private Action<bool> _onLoadRewardedAd;
        private Action _onReceiveRewardedAd;
        private Action<bool> _onCloseRewardedAd;
        
        private List<Action> _displayRewardedAdListeners = new List<Action>();
        private List<Action<AdInfo>> _rewardedAdImpressionListeners = new List<Action<AdInfo>>();
        private List<Action> _receiveRewardedAdListeners = new List<Action>();

        #region initialize

        public void Initialize ( Action onComplete )
        {
            if ( _initialized ) return;

            _initialized = true;
            _onInitialize = onComplete;
            _settingData = AppLovinSettingData.LoadData();
            
            MaxSdkCallbacks.OnSdkInitializedEvent += OnInitialize;
            SetRewardedAdCallbacks();
            
            MaxSdk.SetSdkKey( _settingData.SdkKey );
            //MaxSdk.SetUserId( userId );
            MaxSdk.InitializeSdk();
        }

        private void OnInitialize ( MaxSdkBase.SdkConfiguration sdkConfiguration )
        {
            if ( _onInitialize != null )
            {
                _onInitialize.Invoke();
                _onInitialize = null;
            }
        }

        private void SetRewardedAdCallbacks ()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        }

        #endregion

        #region IAdSdk implement
        
        public string GetRewardedAdUnitId ()
        {
            return _settingData == null ? string.Empty : _settingData.AdUnitIdentifire;
        }

        public void LoadRewardedAd ( Action<bool> onComplete )
        {
            _onLoadRewardedAd = onComplete;
            MaxSdk.LoadRewardedAd( GetRewardedAdUnitId() );
        }

        public bool IsRewardedAdReady ()
        {
            return MaxSdk.IsRewardedAdReady( GetRewardedAdUnitId() );
        }

        public void ShowRewardedAd ( Action onRewarded, Action<bool> onClose )
        {
            if ( MaxSdk.IsRewardedAdReady( GetRewardedAdUnitId() ) )
            {
                _onReceiveRewardedAd = onRewarded;
                _onCloseRewardedAd = onClose;
                MaxSdk.ShowRewardedAd( GetRewardedAdUnitId() );
            }
            else
            {
                onClose.Invoke( false );
            }
        }

        public void SubscribeOnDisplayRewardedAd ( Action onDisplayAd )
        {
            if ( _displayRewardedAdListeners.Contains( onDisplayAd ) )
            {
                return;
            }

            _displayRewardedAdListeners.Add( onDisplayAd );
        }

        public void CancelSubscriptionOnDisplayRewardedAd(Action onDisplayAd)
        {
            _displayRewardedAdListeners.Remove( onDisplayAd );
        }

        private void CallDisplayRewardedAdListeners()
        {
            foreach ( Action listener in _displayRewardedAdListeners )
            {
                listener.Invoke();
            }
        }

        public void SubscribeOnRewardedAdImpression ( Action<AdInfo> onImpression )
        {
            if ( _rewardedAdImpressionListeners.Contains( onImpression ) )
            {
                return;
            }

            _rewardedAdImpressionListeners.Add( onImpression );
        }
        
        public void CancelSubscriptionOnRewardedAdImpression( Action<AdInfo> onImpression )
        {
            _rewardedAdImpressionListeners.Remove( onImpression );
        }
        
        private void CallRewardedAdImpressionListeners(AdInfo info)
        {
            foreach ( Action<AdInfo> listener in _rewardedAdImpressionListeners )
            {
                listener.Invoke( info );
            }
        }
        
        public void SubscribeOnRewaredVideoReceived ( Action onReceived )
        {
            if ( _receiveRewardedAdListeners.Contains( onReceived ) )
            {
                return;
            }

            _receiveRewardedAdListeners.Add( onReceived );
        }
        
        public void CancelSubscriptionOnRewaredVideoReceived( Action onReceived )
        {
            _receiveRewardedAdListeners.Remove( onReceived );
        }
        
        private void CallReceiveRewardedAdListeners()
        {
            foreach ( Action listener in _receiveRewardedAdListeners )
            {
                listener.Invoke();
            }
        }

        #endregion

        #region callBack
        
        /*
         * 콜백 호출 플로우
         *
         * ShowRewardedAd() 호출시 아래 순서로 콜백 호출됨
         * 1. OnAdRevenuePaidEvent
         *
         * 광고 완료시 아래 순서로 콜백 호출됨
         * 1. OnAdReceivedRewardEvent
         * 2. OnAdHiddenEvent
         */

        private void OnRewardedAdLoadedEvent ( string adUnitId, MaxSdkBase.AdInfo adInfo )
        {
            Debug.Log( "Rewarded ad loaded" );

            if ( _onLoadRewardedAd != null )
            {
                _onLoadRewardedAd.Invoke( true );
                _onLoadRewardedAd = null;
            }
        }

        private void OnRewardedAdFailedEvent ( string adUnitId, MaxSdkBase.ErrorInfo errorInfo )
        {
            Debug.Log( "Rewarded ad failed to load with error code: " + errorInfo.Code );

            if ( _onLoadRewardedAd != null )
            {
                _onLoadRewardedAd.Invoke( false );
                _onLoadRewardedAd = null;
            }
        }

        private void OnRewardedAdFailedToDisplayEvent ( string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo )
        {
            Debug.Log( "Rewarded ad failed to display with error code: " + errorInfo.Code );

            if ( _onCloseRewardedAd != null )
            {
                _onCloseRewardedAd.Invoke( false );
                _onCloseRewardedAd = null;
            }
        }

        private void OnRewardedAdClickedEvent ( string adUnitId, MaxSdkBase.AdInfo adInfo )
        {
            Debug.Log( "Rewarded ad clicked" );
        }

        private void OnRewardedAdHiddenEvent ( string adUnitId, MaxSdkBase.AdInfo adInfo )
        {
            Debug.Log( "Rewarded ad dismissed" );
            if ( _onCloseRewardedAd != null )
            {
                _onCloseRewardedAd.Invoke( true );
                _onCloseRewardedAd = null;
            }
        }

        private void OnRewardedAdReceivedRewardEvent ( string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo )
        {
            Debug.Log( "Rewarded ad received reward" );

            if ( _onReceiveRewardedAd != null )
            {
                _onReceiveRewardedAd.Invoke();
                _onReceiveRewardedAd = null;
            }

            CallReceiveRewardedAdListeners();
        }

        private void OnRewardedAdRevenuePaidEvent ( string adUnitId, MaxSdkBase.AdInfo adInfo )
        {
            Debug.Log( "Rewarded ad revenue paid" );

            //AppLovin은 광고 시작시 이벤트를 받을수 없어 OnRewardedAdRevenuePaidEvent시 같이 처리함
            CallDisplayRewardedAdListeners();

            AdInfo info = ConvertAdInfo( adInfo );
            CallRewardedAdImpressionListeners( info );
        }

        private AdInfo ConvertAdInfo(MaxSdkBase.AdInfo adInfo)
        {
            AdInfo info = new AdInfo();
            info.revenue = adInfo.Revenue;
            info.networkName = adInfo.NetworkName;
            return info;
        }

        #endregion
    }
}