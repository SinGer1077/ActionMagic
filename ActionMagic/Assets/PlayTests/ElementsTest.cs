using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.Entities;
using Elements.Components;
using Elements.Systems;

[TestFixture]
[Category("Unity ECS Elements Tests")]
public class ElementsTest : ECSTestBase
{
    [UnityTest]
    public IEnumerator EcsBasic()
    {
        AddSystemToWorld<ElementsSpawnerSystem>();
        var entity = MakeEntity();
        this.EntityManager.AddComponentData(entity, new ElementsSpawnerComponent());
        World.Update();
        Debug.LogWarning("finish setup");

        while (true)
        {
            Debug.LogWarning("loop");
            World.Update();
            yield return null;
        }
    }

    //[Test]
    //public void When_CreateElements_Than_CountEqualTwo()
    //{
    //    var entity =
    //}


}
