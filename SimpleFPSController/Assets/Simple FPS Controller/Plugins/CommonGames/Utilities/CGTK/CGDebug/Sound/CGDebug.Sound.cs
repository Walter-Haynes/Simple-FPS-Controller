namespace CommonGames.Utilities.CGTK
{
    using System;
    
    using UnityEngine;
    
    using Object = UnityEngine.Object;
    using static UnityEngine.Random;
    
    public static partial class CGDebug
    {
        private static readonly AudioSource Previewer = 
        #if UNITY_EDITOR
            UnityEditor.EditorUtility.CreateGameObjectWithHideFlags("Audio Previewer", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
        #else
            null;
        #endif
        
        public static void PlaySound(AudioEvent audioEvent, AudioSource audioSource = null)
        {
            if (audioSource == null)
            {
                audioSource = Previewer;
            }
            
            audioEvent.Play(audioSource);
        }
        
        public static void PlaySound(AudioClip audioClip, AudioSource audioSource = null)
        {
            if (audioSource == null)
            {
                audioSource = Previewer;
            }
            
            audioSource.clip = DingAudioClip;
            audioSource.volume = Range(Volume.minValue, Volume.maxValue);
            audioSource.pitch = Range(Pitch.minValue, Pitch.maxValue);
            audioSource.Play();
        }
        
        public static void PlayDing(AudioSource audioSource = null)
        {
            if (audioSource == null)
            {
                audioSource = Previewer;
            }
            
            audioSource.clip = DingAudioClip;
            audioSource.volume = Range(Volume.minValue, Volume.maxValue);
            audioSource.pitch = Range(Pitch.minValue, Pitch.maxValue);
            audioSource.Play();
        }

        #region Base64 Audio Decoding
        
        [Serializable]
        public struct RangedFloat
        {
            public float minValue;
            public float maxValue;

            public RangedFloat(float minValue, float maxValue)
            {
                this.minValue = minValue;
                this.maxValue = maxValue;
            }
        }
        
        private static AudioClip DingAudioClip => _dingAudioClip ?? CreateAudioClip();
        private static AudioClip _dingAudioClip;
    
        private static readonly RangedFloat Volume = new RangedFloat(0.9f, 1f);

        private static readonly RangedFloat Pitch = new RangedFloat(1f, 1.1f);
    
        private static AudioClip CreateAudioClip()
        {
            byte[] __bytes = System.Convert.FromBase64String(base64Ding);
            float[] __decryptedData = ConvertByteToFloat(__bytes);

            AudioClip __audioClip = AudioClip.Create("dingAudio", __decryptedData.Length, 2, 44100, false);
            __audioClip.SetData(__decryptedData, 0);

            _dingAudioClip = __audioClip;

            return __audioClip;
        }

        private static float[] ConvertByteToFloat(byte[] array)
        {
            float[] floatArr = new float[array.Length / 2];

            for (int i = 0; i < floatArr.Length; i++)
            {
                floatArr[i] = ((float) BitConverter.ToInt16(array, i * 2))/32768.0f;
            }

            return floatArr;
        }
        
        #endregion
    }
}