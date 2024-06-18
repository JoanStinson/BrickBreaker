using System.Collections;
using UnityEngine;
using NUnit.Framework;
using Moq;
using JGM.Game;
using UnityEngine.TestTools;

namespace JGM.GameTests
{
    public class AudioServiceTest
    {
        private AudioService m_audioService;
        private AudioLibrary m_mockAudioLibrary;
        private Mock<ICoroutineService> m_mockCoroutineService;
        private Mock<ComponentPool<AudioSource>> m_mockPool;

        [SetUp]
        public void SetUp()
        {
            m_mockCoroutineService = new Mock<ICoroutineService>();
            m_mockPool = new Mock<ComponentPool<AudioSource>>();
            m_audioService = new GameObject().AddComponent<AudioService>();
            m_mockAudioLibrary = ScriptableObject.CreateInstance<AudioLibrary>();
            m_mockAudioLibrary.SetAssets(new AudioClip[0]);
            m_audioService.SetDependencies(m_mockAudioLibrary, m_mockCoroutineService.Object, m_mockPool.Object);
            m_audioService.Initialize();
        }

        [Test]
        public void When_PlayWithNonExistingAudio_Expect_WarningLogged()
        {
            string audioFileName = "NonExistingAudio";

            m_audioService.Play(audioFileName);

            LogAssert.Expect(LogType.Warning, "Trying to play a clip that doesn't exist!");
            m_mockPool.Verify(p => p.Get(), Times.Never());
            m_mockCoroutineService.Verify(c => c.StartExternalCoroutine(It.IsAny<IEnumerator>()), Times.Never());
        }
    }
}
