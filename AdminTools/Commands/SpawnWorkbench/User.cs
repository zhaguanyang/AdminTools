﻿using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using UnityEngine;

namespace AdminTools.Commands.SpawnWorkbench
{
    public class User : ICommand
    {
        public string Command { get; } = "user";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Spawns a workbench on a specified user";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            EventHandlers.LogCommandUsed((CommandSender)sender, EventHandlers.FormatArguments(arguments, 0));
            if (!((CommandSender)sender).CheckPermission("at.benches"))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count != 4)
            {
                response = "Usage: spawnworkbench user (player id / name) (x value) (y value) (z value)";
                return false;
            }

            Player Ply = Player.Get(arguments.At(0));
            if (Ply == null)
            {
                response = $"Player not found: {arguments.At(0)}";
                return false;
            }
            else if (Ply.Role == RoleType.Spectator || Ply.Role == RoleType.None)
            {
                response = $"This player is not a valid class to spawn a workbench on";
                return false;
            }

            if (!float.TryParse(arguments.At(1), out float xval))
            {
                response = $"Invalid value for x size: {arguments.At(1)}";
                return false;
            }

            if (!float.TryParse(arguments.At(2), out float yval))
            {
                response = $"Invalid value for y size: {arguments.At(2)}";
                return false;
            }

            if (!float.TryParse(arguments.At(3), out float zval))
            {
                response = $"Invalid value for z size: {arguments.At(3)}";
                return false;
            }

            EventHandlers.SpawnWorkbench(Ply.Position + Ply.ReferenceHub.PlayerCameraReference.forward * 2, Ply.GameObject.transform.rotation.eulerAngles, new Vector3(xval, yval, zval));
            response = $"A workbench has spawned on Player {Ply.Nickname}";
            return true;
        }
    }
}