using System;
using Cysharp.Threading.Tasks;
using Rocket.API;
using RocketExtensions.Models;
using RocketExtensions.Plugins;

namespace SherbetVaults.Commands
{
    [CommandName("Vaults")]
    [CommandInfo("Lists your available vaults")]
    [AllowedCaller(AllowedCaller.Player)]
    public class VaultsCommand : RocketCommand
    {
        public override async UniTask Execute(CommandContext context)
        {

        }
    }
}