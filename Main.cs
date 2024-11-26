using BTD_Mod_Helper.Api.ModOptions;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using Il2CppSystem;
using UnityEngine;
using UpgradeAllTowers;

[assembly: MelonInfo(typeof(UpgradeAllTowers.Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace UpgradeAllTowers;

[HarmonyPatch]
public class Main : BloonsTD6Mod
{
    internal static MelonLogger.Instance Logger;

    public override void OnInitialize()
    {
        Logger = LoggerInstance;
    }

    private static readonly ModSettingHotkey UpgradeAllTowersPath1Hotkey = new(KeyCode.LeftBracket);
    private static readonly ModSettingHotkey UpgradeAllTowersPath2Hotkey = new(KeyCode.RightBracket);
    private static readonly ModSettingHotkey UpgradeAllTowersPath3Hotkey = new(KeyCode.Backslash);

    /// <inheritdoc />
    public override void OnUpdate()
    {
        if(InGame.instance == null) return;
        if (UpgradeAllTowersPath1Hotkey.JustPressed())
        {
            UpgradeAllTowers(1);
        }
        if (UpgradeAllTowersPath2Hotkey.JustPressed())
        {
            UpgradeAllTowers(2);
        }
        if (UpgradeAllTowersPath3Hotkey.JustPressed())
        {
            UpgradeAllTowers(3);
        }
    }

    private static void UpgradeAllTowers(int path)
    {
        foreach (var tower in InGame.instance.GetAllTowerToSim())
        {
            var towerModel = tower.Def;
            var newTower = InGame.instance.GetGameModel().GetTower(towerModel.baseId, towerModel.tiers[0] + (path-1 == 0 ? 1 : 0), towerModel.tiers[1] + (path-1 == 1 ? 1 : 0), towerModel.tiers[2] + (path-1 == 2 ? 1 : 0));
            if (newTower == null)
            {
                Logger.Error($"Failed to upgrade {towerModel.name} on path {path}");
                return;
            }
            InGame.Bridge.simulation.towerManager.UpgradeTower(InGame.Bridge.MyPlayerNumber, tower.tower, newTower, path - 1, 0, 0, true, true, false);
        }
    }
}