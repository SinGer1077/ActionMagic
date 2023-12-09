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
        var first = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Fire);
        var second = ElementsSpawnerSystem.CreateElementEntity(m_Manager, ElementTypes.Water);

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

    }

    [Test]
    public void When_WaterElementConnectFireElement_And_SizesIs_10_And_10_Than_SizesWillBe_5_And_0()
    {

    }

    [Test]
    public void When_WaterElementConnectFireElement_And_SizesIs_10_And_20_Than_SizesWillBe_0_And_0()
    {

    }

    [Test]
    public void When_WaterElementConnectFireElement_And_SizesIs_10_And_30_Than_SizesWillBe_0_And_10()
    {

    }

    [Test]
    public void When_WaterElementConnectFireElement_And_SizesIs_10_And_5_Than_SizesWillBe_7_5_And_0()
    {

    }
}
