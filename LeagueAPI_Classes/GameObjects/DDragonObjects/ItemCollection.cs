using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueAPI_Classes
{
    public class ItemCollection
    {
        public string type { get; set; }
        public string version { get; set; }
        public ItemCollection_Basic basic { get; set; }
        public Dictionary<int, Item> data { get; set; }
        public List<ItemCollection_Group> groups { get; set; }
        public List<ItemCollection_Tree> tree { get; set; }
    }

    public class ItemCollection_Tree
    {
        public string header { get; set; }
        public List<string> tags { get; set; }
    }

    public class ItemCollection_Group
    {
        public string id { get; set; }
        public string MaxGroupOwnable { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public string description { get; set; }
        public string colloq { get; set; }
        public string plaintext { get; set; }
        public int? specialRecipe { get; set; }
        public int? stacks { get; set; }
        public bool? consumed { get; set; }
        public bool? consumeOnFull { get; set; }
        public bool? inStore { get; set; }
        public string requiredChampion { get; set; }
        public string requiredAlly { get; set; }
        public bool? hideFromAll { get; set; }
        public List<string> from { get; set; }
        public List<string> into { get; set; }
        public Item_Image image { get; set; }
        public Item_Gold gold { get; set; }
        public List<string> tags { get; set; }
        public Dictionary<int, bool> maps { get; set; }
        public Item_Stats stats { get; set; }
        public Item_Effect effect { get; set; }
        public int? depth { get; set; }
    }

    public class Item_Effect
    {
        public string Effect1Amount { get; set; }
        public string Effect2Amount { get; set; }
        public string Effect3Amount { get; set; }
        public string Effect4Amount { get; set; }
        public string Effect5Amount { get; set; }
        public string Effect6Amount { get; set; }
        public string Effect7Amount { get; set; }
        public string Effect8Amount { get; set; }
        public string Effect9Amount { get; set; }
        public string Effect10Amount { get; set; }
        public string Effect11Amount { get; set; }
        public string Effect12Amount { get; set; }
        public string Effect13Amount { get; set; }
        public string Effect14Amount { get; set; }
        public string Effect15Amount { get; set; }
        public string Effect16Amount { get; set; }
        public string Effect17Amount { get; set; }
        public string Effect18Amount { get; set; }
    }

    public class Item_Gold
    {
        public int baseCost { get; set; }
        public bool purchasable { get; set; }
        public int total { get; set; }
        public int sell { get; set; }
    }

    public class Item_Image
    {
        public string full { get; set; }
        public string sprite { get; set; }
        public string group { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
    }

    public class Item_Stats
    {
        public double? FlatMovementSpeedMod { get; set; }
        public double? FlatHPPoolMod { get; set; }
        public double? FlatCritChanceMod { get; set; }
        public double? FlatMagicDamageMod { get; set; }
        public double? FlatMPPoolMod { get; set; }
        public double? FlatArmorMod { get; set; }
        public double? FlatSpellBlockMod { get; set; }
        public double? FlatPhysicalDamageMod { get; set; }
        public double? PercentAttackSpeedMod { get; set; }
        public double? PercentLifeStealMod { get; set; }
        public double? FlatHPRegenMod { get; set; }
        public double? PercentMovementSpeedMod { get; set; }
    }

    public class ItemCollection_Basic
    {
        public string name { get; set; }
        public Basic_Rune rune { get; set; }
        public Basic_Gold gold { get; set; }
        public string group { get; set; }
        public string description { get; set; }
        public string colloq { get; set; }
        public string plaintext { get; set; }
        public bool? consumed { get; set; }
        public int? stacks { get; set; }
        public int? depth { get; set; }
        public bool? consumeOnFull { get; set; }
        public List<string> from { get; set; }
        public List<string> into { get; set; }
        public int? specialRecipe { get; set; }
        public bool? inStore { get; set; }
        public bool? hideFromAll { get; set; }
        public string requiredChampion { get; set; }
        public string requiredAlly { get; set; }
        public Basic_Stats stats { get; set; }
        public List<string> tags { get; set; }
        public Dictionary<int, bool> maps { get; set; }
    }

    public class Basic_Stats
    {
        public int FlatHPPoolMod { get; set; }
        public int rFlatHPModPerLevel { get; set; }
        public int FlatMPPoolMod { get; set; }
        public int rFlatMPModPerLevel { get; set; }
        public int PercentHPPoolMod { get; set; }
        public int PercentMPPoolMod { get; set; }
        public int FlatHPRegenMod { get; set; }
        public int rFlatHPRegenModPerLevel { get; set; }
        public int PercentHPRegenMod { get; set; }
        public int FlatMPRegenMod { get; set; }
        public int rFlatMPRegenModPerLevel { get; set; }
        public int PercentMPRegenMod { get; set; }
        public int FlatArmorMod { get; set; }
        public int rFlatArmorModPerLevel { get; set; }
        public int PercentArmorMod { get; set; }
        public int rFlatArmorPenetrationMod { get; set; }
        public int rFlatArmorPenetrationModPerLevel { get; set; }
        public int rPercentArmorPenetrationMod { get; set; }
        public int rPercentArmorPenetrationModPerLevel { get; set; }
        public int FlatPhysicalDamageMod { get; set; }
        public int rFlatPhysicalDamageModPerLevel { get; set; }
        public int PercentPhysicalDamageMod { get; set; }
        public int FlatMagicDamageMod { get; set; }
        public int rFlatMagicDamageModPerLevel { get; set; }
        public int PercentMagicDamageMod { get; set; }
        public int FlatMovementSpeedMod { get; set; }
        public int rFlatMovementSpeedModPerLevel { get; set; }
        public int PercentMovementSpeedMod { get; set; }
        public int rPercentMovementSpeedModPerLevel { get; set; }
        public int FlatAttackSpeedMod { get; set; }
        public int PercentAttackSpeedMod { get; set; }
        public int rPercentAttackSpeedModPerLevel { get; set; }
        public int rFlatDodgeMod { get; set; }
        public int rFlatDodgeModPerLevel { get; set; }
        public int PercentDodgeMod { get; set; }
        public int FlatCritChanceMod { get; set; }
        public int rFlatCritChanceModPerLevel { get; set; }
        public int PercentCritChanceMod { get; set; }
        public int FlatCritDamageMod { get; set; }
        public int rFlatCritDamageModPerLevel { get; set; }
        public int PercentCritDamageMod { get; set; }
        public int FlatBlockMod { get; set; }
        public int PercentBlockMod { get; set; }
        public int FlatSpellBlockMod { get; set; }
        public int rFlatSpellBlockModPerLevel { get; set; }
        public int PercentSpellBlockMod { get; set; }
        public int FlatEXPBonus { get; set; }
        public int PercentEXPBonus { get; set; }
        public int rPercentCooldownMod { get; set; }
        public int rPercentCooldownModPerLevel { get; set; }
        public int rFlatTimeDeadMod { get; set; }
        public int rFlatTimeDeadModPerLevel { get; set; }
        public int rPercentTimeDeadMod { get; set; }
        public int rPercentTimeDeadModPerLevel { get; set; }
        public int rFlatGoldPer10Mod { get; set; }
        public int rFlatMagicPenetrationMod { get; set; }
        public int rFlatMagicPenetrationModPerLevel { get; set; }
        public int rPercentMagicPenetrationMod { get; set; }
        public int rPercentMagicPenetrationModPerLevel { get; set; }
        public int FlatEnergyRegenMod { get; set; }
        public int rFlatEnergyRegenModPerLevel { get; set; }
        public int FlatEnergyPoolMod { get; set; }
        public int rFlatEnergyModPerLevel { get; set; }
        public int PercentLifeStealMod { get; set; }
        public int PercentSpellVampMod { get; set; }
    }

    public class Basic_Gold
    {
        public int basee { get; set; }
        public int total { get; set; }
        public int sell { get; set; }
        public bool purchasable { get; set; }
    }

    public class Basic_Rune
    {
        public bool isrune { get; set; }
        public int tier { get; set; }
        public string type { get; set; }
    }
}
