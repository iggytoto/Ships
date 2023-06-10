using System;
using Unity.NetCode;
using Utils;

namespace Services.Ecs
{
    [UnityEngine.Scripting.Preserve]
    public class GameBootstrap : ClientServerBootstrap
    {
        public override bool Initialize(string defaultWorldName)
        {
            var runServer = false;
#if DEDICATED
            runServer = true;
#endif
            
            AutoConnectPort = 0;

            // If we are in the editor, we check if the loaded scene is "Frontend",
            // if we are in a player we assume it is in the frontend if FRONTEND_PLAYER_BUILD
            // is set, otherwise we assume it is a single level.
            // The define FRONTEND_PLAYER_BUILD needs to be set in the build config for the frontend player.
            var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            bool isFrontend = sceneName == SceneConstants.MultiplayerMenu;

            if (!isFrontend || runServer)
            {
                // This will enable auto connect. We only enable auto connect if we are not going through frontend.
                // The frontend will parse and validate the address before connecting manually.
                // Using this auto connect feature will deal with the client only connect address from Multiplayer PlayMode Tools

                // Use "-port 8000" when running a build from commandline to specify the port to use
                // Will override the default port
                string commandPort = CommandLineUtils.GetCommandLineValueFromKey("port");
                if (!string.IsNullOrEmpty(commandPort))
                    AutoConnectPort = UInt16.Parse(commandPort);


                // Create the default client and server worlds, depending on build type in a player or the Multiplayer PlayMode Tools in the editor
                //CreateDefaultClientServerWorlds();
                return base.Initialize(defaultWorldName);
            }
            else
            {
                // Disable the autoconnect in the frontend. The reset i necessary in the Editor since we can start the demos directly and
                // (the AutoConnectPort will lose its default value)
                CreateLocalWorld(defaultWorldName);
            }
            return true;
        }
    }
}