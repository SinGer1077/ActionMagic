using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Tests;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

using Elements.Components;
using Elements.Systems;
using Elements.Data;
using Universal.Components;

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
        Assert.IsTrue(isExist);
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
        Assert.IsTrue(temp == caseValue);
    }

    [Test]
    public void When_ElementsEntitiesCollide_Than_ElementsConnectEachOther()
    {
        var first = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Fire, 10);
        var second = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Water, 10);

        BaseElementComponent firstElement = m_Manager.GetComponentData<BaseElementComponent> (first);
        BaseElementComponent secondElement = m_Manager.GetComponentData<BaseElementComponent>(second);
        ElementsConnectionSystem.ConnectElements(m_Manager, first, second);       

        var firstBuffer = m_Manager.GetBuffer<ElementConnection>(first);
        var secondBuffer = m_Manager.GetBuffer<ElementConnection>(second);
        bool flag = false;        
        if (!firstBuffer.IsEmpty && !secondBuffer.IsEmpty)
        {
            if (firstBuffer[0].ConnectedElement.id == secondElement.id && secondBuffer[0].ConnectedElement.id == firstElement.id)
                flag = true;
        }
        Assert.IsTrue(flag);
    }

    [Test]
    public void When_Elements—onnectEachOther_And_TheyHasConnection_Than_TheirSizeValueChanges()
    {
        var first = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Fire, 10);
        var second = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Water, 10);
        var firstWeightBefore = m_Manager.GetComponentData<WeightComponent>(first);
        var secondWeightBefore = m_Manager.GetComponentData<WeightComponent>(second);    
        ElementsConnectionSystem.ConnectElements(m_Manager, first, second);

        var firstWeightAfter = m_Manager.GetComponentData<WeightComponent>(first);
        var secondWeightAfter = m_Manager.GetComponentData<WeightComponent>(second);

        Assert.AreNotEqual(firstWeightBefore.WeightValue, firstWeightAfter.WeightValue);
        Assert.AreNotEqual(secondWeightBefore.WeightValue, secondWeightAfter.WeightValue);
    }

    [Test]
    public void When_WaterElementConnectFireElement_And_SizesIs_10_And_10_Than_SizesWillBe_5_And_0()
    {
        var first = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Water, 10.0f);
        var second = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Fire, 10.0f);     
        ElementsConnectionSystem.ConnectElements(m_Manager, first, second);

        var firstWeightAfter = m_Manager.GetComponentData<WeightComponent>(first);
        var secondWeightAfter = m_Manager.GetComponentData<WeightComponent>(second);
        float expected1 = 5.0f;
        float expected2 = 0.0f;

        Assert.AreEqual(firstWeightAfter.WeightValue, expected1);
        Assert.AreEqual(secondWeightAfter.WeightValue, expected2);
    }

    [Test]
    public void When_WaterElementConnectFireElement_And_SizesIs_10_And_20_Than_SizesWillBe_0_And_0()
    {
        var first = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Water, 10.0f);
        var second = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Fire, 20.0f);
        ElementsConnectionSystem.ConnectElements(m_Manager, first, second);

        var firstWeightAfter = m_Manager.GetComponentData<WeightComponent>(first);
        var secondWeightAfter = m_Manager.GetComponentData<WeightComponent>(second);
        float expected1 = 0.0f;
        float expected2 = 0.0f;

        Assert.AreEqual(firstWeightAfter.WeightValue, expected1);
        Assert.AreEqual(secondWeightAfter.WeightValue, expected2);
    }

    [Test]
    public void When_WaterElementConnectFireElement_And_SizesIs_10_And_30_Than_SizesWillBe_0_And_10()
    {
        var first = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Water, 10.0f);
        var second = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Fire, 30.0f);
        ElementsConnectionSystem.ConnectElements(m_Manager, first, second);

        var firstWeightAfter = m_Manager.GetComponentData<WeightComponent>(first);
        var secondWeightAfter = m_Manager.GetComponentData<WeightComponent>(second);
        float expected1 = 0.0f;
        float expected2 = 10.0f;

        Assert.AreEqual(firstWeightAfter.WeightValue, expected1);
        Assert.AreEqual(secondWeightAfter.WeightValue, expected2);
    }

    [Test]
    public void When_WaterElementConnectFireElement_And_SizesIs_10_And_5_Than_SizesWillBe_7_5_And_0()
    {
        var first = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Water, 10.0f);
        var second = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Fire, 5.0f);
        ElementsConnectionSystem.ConnectElements(m_Manager, first, second);

        var firstWeightAfter = m_Manager.GetComponentData<WeightComponent>(first);
        var secondWeightAfter = m_Manager.GetComponentData<WeightComponent>(second);
        float expected1 = 7.5f;
        float expected2 = 0.0f;

        Assert.AreEqual(firstWeightAfter.WeightValue, expected1);
        Assert.AreEqual(secondWeightAfter.WeightValue, expected2);
    }

    [Test]
    public void When_ElementsSizeIs_0_Than_ElementShouldBeDeleted()
    {
        var first = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Water, 10.0f);
        var second = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Fire, 30.0f);
        ElementsConnectionSystem.ConnectElements(m_Manager, first, second);

        
    }

    [Test]
    public void When_WaterElementAndFireElementConnects_Than_VaporReactionShouldBeCreated()
    {

    }

    //[UnityTest]
    //public IEnumerator When_OnSceneTwoElementWithPhysics_Than_AfterUpdateTheyConnectChanges()
    //{
    //    var spawnerEntity = m_Manager.CreateEntity(typeof(PhysicsElementsSpawnerComponent));
    //    PhysicsElementsSpawnerComponent spawnerComponent = m_Manager.GetComponentData<PhysicsElementsSpawnerComponent>(spawnerEntity);
        

    //    var typesBuffer = m_Manager.AddBuffer<ElementBuffer>(spawnerComponent.Spawner);
    //    typesBuffer.Add(new ElementBuffer { type = ElementTypes.Water });
    //    typesBuffer.Add(new ElementBuffer { type = ElementTypes.Fire });

    //    var prefabsBuffer = m_Manager.AddBuffer<ElementPrefab>(spawnerComponent.Spawner);
    //    prefabsBuffer.Add(new ElementPrefab { Prefab = (GameObject)Resources.Load("Assets / Prefabs / WaterElement.prefab")), type = ElementTypes.Water);

    //    while (true)
    //    {
    //        World.Update();
    //        var entities = m_Manager.GetAllEntities(Allocator.Persistent);
    //        foreach (var entity in entities)
    //        {
    //            if (m_Manager.HasComponent<PhysicsElementsSpawnerComponent>(entity))
    //            {
    //                Debug.Log("Finded");
    //            }
    //        }
    //        Debug.Log("Updating");
            
    //        yield return null;
    //    }
    //}
    
}
