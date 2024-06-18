using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TMPro
{
    [RequireComponent(typeof(TMP_Text))]
    [ExecuteInEditMode]
    public class TextMeshProEffect : MonoBehaviour
    {
        public EffectType Type;
        public float      DurationInSeconds = 0.5f;
        public float      Amplitude         = 1f;

        [Space][Range(0f, 1f)] public float CharacterDurationRatio = 0f;
        public                 int   CharactersPerDuration  = 0;

        [Space] public Gradient Gradient = new Gradient();
        public         MixType  Mix      = MixType.Multiply;

        [Space] public bool AutoPlay = true;
        public         bool Repeat;

        public string ForWords;

        private readonly List<(int from, int to)> _intervals = new List<(int from, int to)>();
        public           List<(int from, int to)> Intervals => _intervals;

        [HideInInspector] public bool IsFinished;

        private float        _startedAt;
        private TMP_Text     _textMesh;
        private EffectType   _type;
        private bool         _oneShot;
        private bool         _tickable;
        private bool         _started;
        private float        _nextTick;
        private string       _text;
        private ushort       _ticks;
        private float[]      _rand = new float[10];
        private SharedState  _sharedState;
        private float        _fullAnimationDuration;
        private TMP_TextInfo _textInfo;
        private string       _forWords;

        private SharedState SharedStateProp
        {
            get
            {
                if (_sharedState != null) return _sharedState;
                _sharedState = GetComponent<SharedState>();
                if (_sharedState == null)
                {
                    _sharedState = gameObject.AddComponent<SharedState>();
                    _sharedState.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector | HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
                }

                return _sharedState;
            }
        }
        

        public void CopyTo(TextMeshProEffect effect)
        {
            effect.Type = Type;
            effect.DurationInSeconds = DurationInSeconds;
            effect.Amplitude = Amplitude;
            effect.CharacterDurationRatio = CharacterDurationRatio;
            effect.CharactersPerDuration = CharactersPerDuration;
            effect.Gradient = Gradient;
            effect.Mix = Mix;
            effect.AutoPlay = AutoPlay;
            effect.Repeat = Repeat;
            effect.ForWords = ForWords;
        }
        public void Apply()
        {
            _textMesh = GetComponent<TMP_Text>();
            _type = Type;
            _oneShot = _type == EffectType.Unfold || _type == EffectType.Grow || _type == EffectType.Bounce;
            _tickable = _type == EffectType.Sketch;
            _started = false;
            _nextTick = -1f;
            //BuildIntervals();
        }

        private void OnEnable()
        {
            if (AutoPlay) Play();
        }

        private void OnDestroy()
        {
            _sharedState = GetComponent<SharedState>();
            if (_sharedState == null) return;
            
            var other = gameObject.GetComponents<TextMeshProEffect>();
            if (other.Length == 0 || other.All(p => p == null || !p.enabled))
            {
                Destroy(_sharedState);
            }
        }

        private void OnValidate()
        {
            if (AutoPlay) Play();
            else Apply();
        }

        private void LateUpdate()
        {
            if (_textMesh == null) return;
            if (DurationInSeconds <= 0) return;
            if (!_started) return;

            if (Repeat && IsFinished) Play();
            
            if (!SharedStateProp.TextMeshIsUpdated)
            {
                _textMesh.ForceMeshUpdate();
                SharedStateProp.TextMeshIsUpdated = true;
            }

            _textInfo = _textMesh.textInfo;
            var meshData = _textInfo.CopyMeshInfoVertexData();
            var n = _textInfo.characterCount;

            if (n == 0)
            {
                IsFinished = true;
                return;
            }

            var timeSpent = Time.realtimeSinceStartup - _startedAt;
            if (_text != _textMesh.text || ForWords != _forWords)
            {
                _nextTick = -1;
                _text = _textMesh.text;
                _forWords = ForWords;
                BuildIntervals();
            }

            if (CharactersPerDuration > 0)
            {
                _fullAnimationDuration = (DurationInSeconds * _text.Length) / (float) CharactersPerDuration;
            }
            else
            {
                _fullAnimationDuration = DurationInSeconds;
            }
            

            if (_tickable)
            {
                if (timeSpent >= _nextTick)
                {
                    _nextTick = timeSpent + _fullAnimationDuration;
                    unchecked
                    {
                        _ticks++;
                    }

                    if (_rand.Length < n * 2)
                    {
                        _rand = new float[n * 2];
                    }

                    for (int i = 0; i < _rand.Length; i++)
                    {
                        _rand[i] = UnityEngine.Random.value;
                    }
                }
            }

            if (_oneShot && timeSpent > _fullAnimationDuration)
            {
                timeSpent = _fullAnimationDuration;
                IsFinished = true;
            }

            var part = timeSpent / _fullAnimationDuration;

            if (!_oneShot)
            {
                part = part % 1;
            }

            var ratio = CharacterDurationRatio;
            var charDuration = Mathf.Lerp(1f / n, 1, ratio);

            int ic = 0;
            int nc = n;
            
            if (_intervals.Count > 0 || !string.IsNullOrEmpty(ForWords))
            {
                nc = 0;
                for (int j = 0; j < _intervals.Count; j++)
                {
                    var interval = _intervals[j];
                    nc += interval.to - interval.from + 1;
                }
            }

            for (int i = 0; i < n; i++)
            {
                if (_intervals.Count > 0 || !string.IsNullOrEmpty(ForWords))
                {
                    bool hit = false;
                    for (int j = 0; j < _intervals.Count; j++)
                    {
                        var interval = _intervals[j];
                        if (i >= interval.from && i <= interval.to)
                        {
                            hit = true;
                        }
                    }

                    if (!hit) continue;
                }
                
                TMP_CharacterInfo charInfo = _textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                    continue;

                var charStart = Mathf.Lerp(ic * 1f /nc, 0f, ratio);
                var charPart = (part - charStart) / charDuration;
                charPart = Mathf.Clamp01(charPart);

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;
                var colors = _textInfo.meshInfo[materialIndex].colors32;
               
                Vector3[] sourceVertices = meshData[materialIndex].vertices;
                Vector3[] destinationVertices = _textInfo.meshInfo[materialIndex].vertices;

                ApplyEffect(_textInfo,
                    charInfo,
                    vertexIndex,
                    colors,
                    destinationVertices,
                    sourceVertices,
                    part,
                    charPart,
                    _ticks);
                
                ic++;
            }

            for (int i = 0; i < _textInfo.meshInfo.Length; i++)
            {
                if (i >= _textInfo.materialCount) continue;
                _textInfo.meshInfo[i].mesh.vertices = _textInfo.meshInfo[i].vertices;
                _textMesh.UpdateGeometry(_textInfo.meshInfo[i].mesh, i);
            }

            _textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }

        private void BuildIntervals()
        {
            _intervals.Clear();
            if (!string.IsNullOrWhiteSpace(ForWords) && _text != null)
            {
                var sb = new StringBuilder(_textInfo.characterCount);
                for (int i = 0; i < _textInfo.characterCount; i++)
                {
                    sb.Append(_textInfo.characterInfo[i].character);
                }
                
                bool ignoreCase = (_textMesh.fontStyle & (FontStyles.LowerCase | FontStyles.SmallCaps | FontStyles.UpperCase)) != 0;
                var geometryText = sb.ToString();
                
                var words = ForWords.Split(new[] {'\t', ' '}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    for (int index = 0;;index += word.Length) 
                    {
                        index = geometryText.IndexOf(
                            word,
                            index,
                            ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                        
                        if (index == -1) break;
                        if (index > 0 && !char.IsWhiteSpace(geometryText[index - 1])) continue;
                        var end = index + word.Length;
                        if (end < geometryText.Length && !char.IsWhiteSpace(geometryText[end])) continue;
                        
                        _intervals.Add((from: index, to: index + word.Length - 1));
                    }
                }
            }
        }

        private void ApplyEffect(TMP_TextInfo      textInfo,
                                 TMP_CharacterInfo charInfo,
                                 int               vertexIndex,
                                 Color32[]         colors,
                                 Vector3[]         destinationVertices,
                                 Vector3[]         sourceVertices,
                                 float             part,
                                 float             charPart,
                                 ushort            ticks)
        {
            if (_tickable)
            {
                ColorCharacter(charInfo, vertexIndex, colors, GetRand(_ticks + charInfo.index));
            }
            else
            {
                ColorCharacter(charInfo, vertexIndex, colors, charPart);
            }

            switch (Type)
            {
                case EffectType.Waves:
                    Waves(charInfo, vertexIndex, destinationVertices, sourceVertices, part);
                    break;
                case EffectType.Grow:
                    Grow(charInfo, vertexIndex, destinationVertices, sourceVertices, charPart);
                    break;
                case EffectType.Unfold:
                    Unfold(charInfo, vertexIndex, destinationVertices, sourceVertices, charPart);
                    break;
                case EffectType.UnfoldAndWaves:
                    Unfold(charInfo, vertexIndex, destinationVertices, sourceVertices, charPart);
                    Waves(charInfo, vertexIndex, destinationVertices, destinationVertices, part);
                    break;
                case EffectType.Sketch:
                    Sketch(charInfo, vertexIndex, destinationVertices, sourceVertices, charPart, ticks);
                    break;
                case EffectType.Bounce:
                    Bounce(charInfo, vertexIndex, destinationVertices, sourceVertices, charPart);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private float GetRand(int seed)
        {
            var index1 = Mathf.Abs(seed % _rand.Length);
            return _rand[index1];
        }

        private void Sketch(TMP_CharacterInfo charInfo, int i, Vector3[] dst, Vector3[] src, float charPart, int tick)
        {
            Vector3 OffsetF(int index, int tickShift)
            {
                var shift = charInfo.pointSize * 0.1f * Amplitude;
                var rand = GetRand(index << tickShift);
                var rand2 = GetRand(index << tickShift >> 5);

                return new Vector3(rand * shift, rand2 * shift, 0);
            }

            dst[i + 0] = src[i + 0] - OffsetF(i + 0, tick);
            dst[i + 1] = src[i + 1] - OffsetF(i + 1, tick);
            dst[i + 2] = src[i + 2] - OffsetF(i + 2, tick);
            dst[i + 3] = src[i + 3] - OffsetF(i + 3, tick);
        }

        private void ColorCharacter(TMP_CharacterInfo charInfo, int i, Color32[] colors, float charPart)
        {
            var color = Gradient.Evaluate(charPart);

            if (Mix == MixType.Multiply)
            {
                colors[i + 0] *= color;
                colors[i + 1] *= color;
                colors[i + 2] *= color;
                colors[i + 3] *= color;
            }
            else
            {
                for (int j = 0; j < 4; j++)
                {
                    var tmp = colors[i + j] + color;
                    tmp.a *= color.a;
                    colors[i + j] = tmp;
                }
            }
        }

        private void Waves(TMP_CharacterInfo charInfo, int i, Vector3[] dst, Vector3[] src, float part)
        {
            Vector3 OffsetF(int index)
            {
                var rads = -Mathf.PI * 2 * part + (index / 4) * 0.3f;
                return new Vector3(0, Mathf.Cos(rads) * charInfo.pointSize * 0.3f * Amplitude, 0);
            }

            dst[i + 0] = src[i + 0] - OffsetF(i + 0);
            dst[i + 1] = src[i + 1] - OffsetF(i + 1);
            dst[i + 2] = src[i + 2] - OffsetF(i + 2);
            dst[i + 3] = src[i + 3] - OffsetF(i + 3);
        }
        
        private void Bounce(TMP_CharacterInfo charInfo, int i, Vector3[] dst, Vector3[] src, float part)
        {
            Vector3 OffsetF(int index)
            {
                var rads = -Mathf.PI * 2 * part;
                return new Vector3(0, Mathf.Cos(rads) * charInfo.pointSize * 0.3f * Amplitude, 0);
            }

            dst[i + 0] = src[i + 0] - OffsetF(i + 0);
            dst[i + 1] = src[i + 1] - OffsetF(i + 1);
            dst[i + 2] = src[i + 2] - OffsetF(i + 2);
            dst[i + 3] = src[i + 3] - OffsetF(i + 3);
        }

        private void Grow(TMP_CharacterInfo charInfo, int i, Vector3[] dst, Vector3[] src, float part)
        {
            dst[i + 0] = src[i + 0];
            dst[i + 3] = src[i + 3];

            dst[i + 1] = Vector3.Lerp(src[i + 0], src[i + 1], part);
            dst[i + 2] = Vector3.Lerp(src[i + 3], src[i + 2], part);

            dst[i + 0] = Vector3.LerpUnclamped(src[i + 0], dst[i + 0], Amplitude);
            dst[i + 1] = Vector3.LerpUnclamped(src[i + 1], dst[i + 1], Amplitude);
            dst[i + 2] = Vector3.LerpUnclamped(src[i + 2], dst[i + 2], Amplitude);
            dst[i + 3] = Vector3.LerpUnclamped(src[i + 3], dst[i + 3], Amplitude);
        }

        private void Unfold(TMP_CharacterInfo charInfo, int i, Vector3[] dst, Vector3[] src, float part)
        {
            var midA = (src[i + 0] + src[i + 1]) * 0.5f;
            var midB = (src[i + 3] + src[i + 2]) * 0.5f; 

            dst[i + 0] = Vector3.Lerp(midA, src[i + 0], part);
            dst[i + 3] = Vector3.Lerp(midB, src[i + 3], part);

            dst[i + 1] = Vector3.Lerp(midA, src[i + 1], part);
            dst[i + 2] = Vector3.Lerp(midB, src[i + 2], part);

            dst[i + 0] = Vector3.LerpUnclamped(src[i + 0], dst[i + 0], Amplitude);
            dst[i + 1] = Vector3.LerpUnclamped(src[i + 1], dst[i + 1], Amplitude);
            dst[i + 2] = Vector3.LerpUnclamped(src[i + 2], dst[i + 2], Amplitude);
            dst[i + 3] = Vector3.LerpUnclamped(src[i + 3], dst[i + 3], Amplitude);
        }

        [ContextMenu("Play")]
        public void Play()
        {
            Apply();
            IsFinished = false;
            _startedAt = Time.realtimeSinceStartup;
            _started = true;
        }

        [ContextMenu("Finish")]
        public void Finish()
        {
            _startedAt = Single.MinValue;
        }

        public enum EffectType : byte
        {
            Waves,
            Grow,
            Unfold,
            UnfoldAndWaves,
            Sketch,
            Bounce
        }

        public enum MixType : byte
        {
            Multiply,
            Add
        }
        [ExecuteInEditMode]
        internal class SharedState : MonoBehaviour
        {
            internal bool TextMeshIsUpdated;

            private void LateUpdate()
            {
                TextMeshIsUpdated = false;
            }
        }
    }
}