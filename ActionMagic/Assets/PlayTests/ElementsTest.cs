using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.Entities;

using Elements.Components;
using Elements.Systems;
using Elements.Data;

[TestFixture]
[Category("Unity ECS Elements Tests")]
public class ElementsTest : ECSTestBase
{
    [UnityTest]
    public IEnumerator EcsBasic()
    {
        AddSystemToWorld<ElementsSpawnerSystem>();
        var entity = MakeEntity();
        EntityManager.AddComponentData(entity, new ElementsSpawnerComponent());
        World.Update();
        Debug.LogWarning("finish setup");

        while (true)
        {
            Debug.LogWarning("loop");
            World.Update();
            yield return null;
        }
    }

    [Test]
    public void When_CreateElementWater_Than_ElementTypeWater()
    {
        AddSystemToWorld<ElementsSpawnerSystem>();
        World.Update();

        var entity = EntityManager.CreateEntity(typeof(BaseElementComponent));
        EntityManager.SetComponentData(entity, new BaseElementComponent { Type = ElementTypes.Water });
        Assert.AreEqual(ElementTypes.Water, EntityManager.GetComponentData<BaseElementComponent>(entity).Type);
    }

    [Test]
    public void When_CreateElements_Than_CountEqualTwo()
    {
        AddSystemToWorld<ElementsSpawnerSystem>();      
        World.Update();
       
        var query = GetEntityQuery(typeof(BaseElementComponent));
        //foreach (var enitity in query.) { 

        //} 
        int count = query.CalculateEntityCount();
        //int count = query.;
        Debug.Log(count + " count");
        Assert.AreEqual(5, count);
    }


}
