using System;
using UnityEngine;



namespace Treenod.Ad
{
    public class AppLovinAdapter : IAdSdk
    {
        private bool _initialized;
        private AppLovinSettingData _settingData;
        private Action _onInitialize;
        private Action<bool> _onLoadRewardedAd;
        private Action _onReceiveRewardedAd;
        private Action<bool> _onCloseRewardedAd;
        private Action _onDisplayRewardedAd;
        private Action<string> _onRewardedAdImpression;

        #region initialize

        public void Initialize ( Action onComplete )
        {
            if ( _initialized ) return;

            _initialized = true;
            _settingData = AppLovinSettingData.LoadData();

            _onInitialize = onComplete;

            MaxSdkCallbacks.OnSdkInitializedEvent += OnInitialize;
            MaxSdk.SetSdkKey( _settingData.SdkKey );
            //MaxSdk.SetUserId( userId );
            MaxSdk.InitializeSdk();
        }

        private void OnInitialize ( MaxSdkBase.SdkConfiguration sdkConfiguration )
        {
            SetRewardedAdCallbacks();

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
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
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
            _onDisplayRewardedAd = onDisplayAd;
        }

        public void SubscribeOnRewardedAdImpression ( Action<string> onImpression )
        {
            _onRewardedAdImpression = onImpression;
        }

        #endregion

        #region callBack

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

        private void OnRewardedAdDisplayedEvent ( string adUnitId, MaxSdkBase.AdInfo adInfo )
        {
            Debug.Log( "Rewarded ad displayed" );

            if ( _onDisplayRewardedAd != null )
            {
                _onDisplayRewardedAd.Invoke();
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
        }

        private void OnRewardedAdRevenuePaidEvent ( string adUnitId, MaxSdkBase.AdInfo adInfo )
        {
            Debug.Log( "Rewarded ad revenue paid" );

            if ( _onRewardedAdImpression != null )
            {
                _onRewardedAdImpression.Invoke( adInfo.NetworkName );
            }
        }

        #endregion
    }
}