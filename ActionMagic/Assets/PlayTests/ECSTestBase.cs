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

    //public EntityQuery GetEntityQuery<T>(ComponentType component) where T : IComponentData
    //{
    //    var allEntities = EntityManager.GetAllEntities(Allocator.Persistent);
    //    EntityQuery query = new EntityQuery();
    //    NativeList<typeof(T)> list = new NativeList<typeof(T)>(100);
    //    foreach (var entity in allEntities)
    //    {
    //        if (EntityManager.HasComponent(entity, component))
    //        {
    //            query.
    //        }
    //    }
    //}
}
