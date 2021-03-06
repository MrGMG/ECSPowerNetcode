using Plugins.ECSPowerNetcode.Server.Components;
using Plugins.ECSPowerNetcode.Server.Groups;
using Plugins.UnityExtras.Logs;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;

namespace Plugins.ECSPowerNetcode.Server.Lifecycle
{
    [UpdateInGroup(typeof(ServerConnectionSystemGroup))]
    public class ServerStartSystem : ComponentSystem
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<StartServer>();
        }

        protected override void OnUpdate()
        {
            var startServer = GetSingleton<StartServer>();
            EntityManager.DestroyEntity(GetSingletonEntity<StartServer>());

            var network = World.GetExistingSystem<NetworkStreamReceiveSystem>();
            NetworkEndPoint ep = NetworkEndPoint.AnyIpv4;
            ep.Port = startServer.port;
            network.Listen(ep);
            UnityLogger.Info($"Server started on {ep.Port} port in {World.Name} world");
        }
    }
}