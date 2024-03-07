using NUnit.Framework;
using Unity.Entities;
using Unity.Collections;


public class ECSTestBase
{
    protected World World { get; private set; }
    protected EntityManager EntityManager => World.EntityManager;

    protected EntityQuery CreateEntityQuery(params ComponentType[] components) => EntityManager.CreateEntityQuery(components);
    protected EntityQuery GetEntityQuery(params ComponentType[] components) =>
        CreateEntityQuery(components);

    [SetUp]
    public void SetUpBase()
    {
        DefaultWorldInitialization.DefaultLazyEditModeInitialize();
        World = World.DefaultGameObjectInjectionWorld;
        World.Update();
    }

    [TearDown]
    public void TearDown()
    {
        World.Dispose();
    }

    protected void AddSystemToWorld<T>() where T : ISystem
    {
        DefaultWorldInitialization.AddSystemsToRootLevelSystemGroups(
            World,
            typeof(T));
    }

    public Entity MakeEntity()
    {        
        var ent = EntityManager.CreateEntity();
        return ent;
    }

}
