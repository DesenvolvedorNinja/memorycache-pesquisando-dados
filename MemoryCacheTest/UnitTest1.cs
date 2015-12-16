using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Caching;
using MemoryCacheDemo;
using System.Threading;
using System.Linq;

namespace MemoryCacheTest
{
    [TestClass]
    public class UnitTest1
    {
        private MemoryCache memoryCache;

        [TestInitialize]
        public void Initialize()
        {
            memoryCache = MemoryCache.Default;
        }

        [TestMethod]
        public void TestAdd()
        {
            Desenvolvedor dev = new Desenvolvedor();
            dev.Nome = "Ninja";
            dev.Linguagem = "C#";
            dev.AnosExperiencia = 15;

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(60);

            Assert.IsTrue(memoryCache.Add("dev", dev, policy));
        }

        [TestMethod]
        public void TestGet()
        {
            Desenvolvedor dev = new Desenvolvedor();
            dev.Nome = "Ninja";
            dev.Linguagem = "C#";
            dev.AnosExperiencia = 15;

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(60);

            memoryCache.Add("devGet", dev, policy);

            Desenvolvedor devGet = (Desenvolvedor)memoryCache["devGet"];
            
            Assert.AreEqual(dev.ToString(), devGet.ToString());
        }

        [TestMethod]
        public void TestGetExpired()
        {
            Desenvolvedor dev = new Desenvolvedor();
            dev.Nome = "Ninja";
            dev.Linguagem = "C#";
            dev.AnosExperiencia = 15;

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(1);

            memoryCache.Add("devExpired", dev, policy);
            Thread.Sleep(3000);

            Desenvolvedor devGet = (Desenvolvedor)memoryCache["devExpired"];
            
            Assert.IsNull(devGet);
        }

        [TestMethod]
        public void TestSliding()
        {
            Desenvolvedor dev = new Desenvolvedor();
            dev.Nome = "Ninja";
            dev.Linguagem = "C#";
            dev.AnosExperiencia = 15;

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.SlidingExpiration = TimeSpan.FromSeconds(3);

            memoryCache.Add("devSliding", dev, policy);

            Thread.Sleep(2000);
            Desenvolvedor devGet = (Desenvolvedor)memoryCache["devSliding"];
            Assert.AreEqual(dev.ToString(), devGet.ToString());

            Thread.Sleep(2000);
            devGet = (Desenvolvedor)memoryCache["devSliding"];
            Assert.AreEqual(dev.ToString(), devGet.ToString());

            Thread.Sleep(4000);
            devGet = (Desenvolvedor)memoryCache["devSliding"];
            Assert.IsNull(devGet);
        }

        [TestMethod]
        public void TestRemove()
        {
            Desenvolvedor dev = new Desenvolvedor();
            dev.Nome = "Ninja";
            dev.Linguagem = "C#";
            dev.AnosExperiencia = 15;

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(60);
            
            memoryCache.Add("devRemove", dev, policy);

            Desenvolvedor devGet = (Desenvolvedor)memoryCache["devRemove"];
            Assert.AreEqual(dev.ToString(), devGet.ToString());

            memoryCache.Remove("devRemove");

            devGet = (Desenvolvedor)memoryCache["devRemove"];
            Assert.IsNull(devGet);
        }

        [TestMethod]
        public void TestCount()
        {
            MemoryCache memoryCount = new MemoryCache("count");

            Desenvolvedor dev = new Desenvolvedor();
            dev.Nome = "Ninja";
            dev.Linguagem = "C#";
            dev.AnosExperiencia = 15;

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(2);

            memoryCount.Add("devRemove", dev, policy);

            Assert.AreEqual(1, memoryCount.GetCount());
        }

        [TestMethod]
        public void TestSearchName()
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(60);

            Desenvolvedor devNinja = new Desenvolvedor();
            devNinja.Nome = "Ninja";
            devNinja.Linguagem = "C#";
            devNinja.AnosExperiencia = 15;
            memoryCache.Add("devNinja", devNinja, policy);

            Desenvolvedor devSamurai = new Desenvolvedor();
            devSamurai.Nome = "Samurai";
            devSamurai.Linguagem = "Java";
            devSamurai.AnosExperiencia = 10;
            memoryCache.Add("devSamurai", devSamurai, policy);

            Desenvolvedor devRonin = new Desenvolvedor();
            devRonin.Nome = "Ronin";
            devRonin.Linguagem = "Java";
            devRonin.AnosExperiencia = 2;
            memoryCache.Add("devRonin", devRonin, policy);

            Desenvolvedor devMestre = new Desenvolvedor();
            devMestre.Nome = "Mestre";
            devMestre.Linguagem = "JavaScript";
            devMestre.AnosExperiencia = 15;
            memoryCache.Add("devMestre", devMestre, policy);

            var devGet = memoryCache.Where(dev => dev.Value is Desenvolvedor && (dev.Value as Desenvolvedor).Nome == "Ninja").Select(dev => (Desenvolvedor)dev.Value).First();
            Assert.AreEqual(devNinja, devGet);
        }

        [TestMethod]
        public void TestSearchLanguage()
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(60);

            Desenvolvedor devNinja = new Desenvolvedor();
            devNinja.Nome = "Ninja";
            devNinja.Linguagem = "C#";
            devNinja.AnosExperiencia = 15;
            memoryCache.Add("devNinja", devNinja, policy);

            Desenvolvedor devSamurai = new Desenvolvedor();
            devSamurai.Nome = "Samurai";
            devSamurai.Linguagem = "Java";
            devSamurai.AnosExperiencia = 10;
            memoryCache.Add("devSamurai", devSamurai, policy);

            Desenvolvedor devRonin = new Desenvolvedor();
            devRonin.Nome = "Ronin";
            devRonin.Linguagem = "Java";
            devRonin.AnosExperiencia = 2;
            memoryCache.Add("devRonin", devRonin, policy);

            Desenvolvedor devMestre = new Desenvolvedor();
            devMestre.Nome = "Mestre";
            devMestre.Linguagem = "JavaScript";
            devMestre.AnosExperiencia = 15;
            memoryCache.Add("devMestre", devMestre, policy);

            var listDevs = memoryCache.Where(dev => dev.Value is Desenvolvedor && (dev.Value as Desenvolvedor).Linguagem == "Java").Select(dev => (Desenvolvedor)dev.Value).ToList();
            Assert.AreEqual(2, listDevs.Count);
        }
    }
}
