using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ChuongGa
{
    public class ChuongGaManager : MonoBehaviour
    {
        [SerializeField]
        private Text station_name;
        
        [SerializeField]
        private CanvasGroup _canvasLayer1;
        [SerializeField]
        private Text LedStatus;
        [SerializeField]
        private Text PumpStatus;
        [SerializeField]
        private Text temperature;
        [SerializeField]
        private Text humidity;
        [SerializeField]
        private CanvasGroup status_led_on;
        [SerializeField]
        private CanvasGroup status_pump_on;
        [SerializeField]
        private CanvasGroup status_led_off;
        [SerializeField]
        private CanvasGroup status_pump_off;
        [SerializeField]
        private Button _btn_config;
        /// <summary>
        /// Layer 2 elements
        /// </summary>
        [SerializeField]
        private CanvasGroup _canvasLayer2;
        [SerializeField]
        private Toggle LedMode;
        [SerializeField]
        private Toggle PumpMode;

        /// <summary>
        /// General elements
        /// </summary>
        [SerializeField]
        private GameObject Btn_Quit;

        private Tween twenFade;

        private bool device_status = false;

        public void Update_Status(Status_Data _status_data)
        {
            station_name.text = _status_data.station_name;
            foreach(data_ss _data in _status_data.data_ss)
            {
                switch (_data.ss_name)
                {
                    case "temperature":
                        temperature.text = _data.ss_value + "°C";
                        break;

                    case "humidity":
                        humidity.text = _data.ss_value + "%";
                        break;

                    // case "mode_fan_auto":
                    //     if (_data.ss_value == "1") { 
                    //         ModeAuto.isOn = true;
                    //         LampControl.interactable = false;
                    //     }
                    //     else { 
                    //         ModeAuto.isOn = false;
                    //         LampControl.interactable = true;
                    //     }
                    //     break;
                    //case "device_status":
                    //    Debug.Log("_data.ss_value " + _data.ss_value);
                    //    if (_data.ss_value == "1")
                    //        _btn_config.interactable = true;
                       
                    //    break;
                }
                
            }
            if(_status_data.device_status=="1")
                _btn_config.interactable = true;

        }

        public void Update_Control(Config_Data _control_data)
        {
            if (_control_data.device == "LED")
            {
                // LampControl.interactable = true;
                // if (_control_data.fan_status == 1)
                //     LampControl.isOn = true;
                // else
                //     LampControl.isOn = false;
                if (_control_data.status != LedStatus.text)
                {
                    OnLedValueChange(_control_data.status);
                }
                
            }
            else if (_control_data.device == "PUMP")
            {
                if (_control_data.status != PumpStatus.text)
                {
                    OnPumpValueChange(_control_data.status);
                }
                
            }

        }

        public Config_Data Update_Led_Value(Config_Data _configdata)
        {
            _configdata.device = "LED";
            _configdata.status = LedMode.isOn ? "ON" : "OFF";
           
            return _configdata;
        }

        public Config_Data Update_Pump_Value(Config_Data _configdata)
        {
            _configdata.device = "PUMP";
            _configdata.status = PumpMode.isOn ? "ON" : "OFF";
           
            return _configdata;
        }

        public void OnLedValueChange(string status)
        {
            if (status == "ON")
            {
                LedStatus.text = "ON";
                LedStatus.color = new Color(0f, 255f, 0f);
                LedMode.isOn = true;
                if (status_led_on.interactable == false)
                {
                    SwitchLamp();
                }
            }
            else
            {
                LedStatus.text = "OFF";
                LedStatus.color = new Color(255f, 0f, 0f);
                LedMode.isOn = false;
                if (status_led_off.interactable == false)
                {
                    SwitchLamp();
                }
            }
        }

        public void OnPumpValueChange(string status)
        {
            if (status == "ON")
            {
                PumpStatus.text = "ON";
                PumpStatus.color = new Color(0f, 255f, 0f);
                PumpMode.isOn = true;
                if (status_pump_on.interactable == false)
                {
                    SwitchLampPump();
                }
            }
            else
            {
                PumpStatus.text = "OFF";
                PumpStatus.color = new Color(255f, 0f, 0f);
                PumpMode.isOn = false;
                if (status_pump_off.interactable == false)
                {
                    SwitchLampPump();
                }
            }
        }

        public void Disable_Config_Btn()
        {
            _btn_config.interactable = false;
        }

        public void Fade(CanvasGroup _canvas, float endValue, float duration, TweenCallback onFinish)
        {
            if (twenFade != null)
            {
                twenFade.Kill(false);
            }

            twenFade = _canvas.DOFade(endValue, duration);
            twenFade.onComplete += onFinish;
        }

        public void FadeIn(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 1f, duration, () =>
            {
                _canvas.interactable = true;
                _canvas.blocksRaycasts = true;
            });
        }

        public void FadeOut(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 0f, duration, () =>
            {
                _canvas.interactable = false;
                _canvas.blocksRaycasts = false;
            });
        }



        public IEnumerator _IESwitchLayer()
        {
            if (_canvasLayer1.interactable == true)
            {
                FadeOut(_canvasLayer1, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(_canvasLayer2, 0.25f);
            }
            else
            {
                FadeOut(_canvasLayer2, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(_canvasLayer1, 0.25f);
            }
        }

        IEnumerator _IESwitchlamp()
        {
            if (status_led_off.interactable == true)
            {
                FadeOut(status_led_off, 0.1f);
                yield return new WaitForSeconds(0.15f);
                FadeIn(status_led_on, 0.1f);
            }
            else
            {
                FadeOut(status_led_on, 0.1f);
                yield return new WaitForSeconds(0.15f);
                FadeIn(status_led_off, 0.1f);
            }
        }
        IEnumerator _IESwitchlampPump()
        {
            if (status_pump_off.interactable == true)
            {
                FadeOut(status_pump_off, 0.1f);
                yield return new WaitForSeconds(0.15f);
                FadeIn(status_pump_on, 0.1f);
            }
            else
            {
                FadeOut(status_pump_on, 0.1f);
                yield return new WaitForSeconds(0.15f);
                FadeIn(status_pump_off, 0.1f);
            }
        }
        public void SwitchLayer()
        {
            StartCoroutine(_IESwitchLayer());
        }

        public void SwitchLamp()
        {
            StartCoroutine(_IESwitchlamp());

        }
        public void SwitchLampPump()
        {
            StartCoroutine(_IESwitchlampPump());

        }
    }
}