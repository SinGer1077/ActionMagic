using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Tests;

using Elements.Components;
using Elements.Systems;
using Elements.Data;

[TestFixture]
[Category("Unity ECS Elements Tests")]
public class ElementsTest : ECSTestsFixture
{
    //[UnityTest]
    //public IEnumerator EcsBasic()
    //{
    //    AddSystemToWorld<ElementsSpawnerSystem>();
    //    var entity = MakeEntity();
    //    EntityManager.AddComponentData(entity, new ElementsSpawnerComponent());
    //    World.Update();
    //    Debug.LogWarning("finish setup");

    //    while (true)
    //    {
    //        Debug.LogWarning("loop");
    //        World.Update();
    //        yield return null;
    //    }
    //}    

    [Test]
    public void When_CreateEntityWithComponent_Than_SystemRunsIt()
    {
        var entity = m_Manager.CreateEntity(
            typeof(ElementsSpawnerComponent));
        m_Manager.SetComponentData(entity, new ElementsSpawnerComponent { Count = 5 });
      
        var isExist = m_Manager.Exists(entity);
        Assert.AreEqual(true, isExist);
    }

    [Test]
    public void When_CreateElements_Than_ElementsExistInWorld()
    {                
        int caseValue = 5;
        var entity = m_Manager.CreateEntity(
            typeof(ElementsSpawnerComponent));
        m_Manager.SetComponentData(entity, new ElementsSpawnerComponent { Count = caseValue });
        World.GetOrCreateSystem<ElementsSpawnerSystem>().Update(m_Manager.WorldUnmanaged);
        int temp = EmptySystem.GetEntityQuery(typeof(BaseElementComponent)).CalculateEntityCount();
        Assert.AreEqual(true, temp == caseValue);
    }
}
