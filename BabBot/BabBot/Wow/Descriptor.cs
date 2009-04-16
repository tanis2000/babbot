namespace BabBot.Wow
{
#pragma warning disable 1591
    public static class Descriptor
    {
        // Version: 3.0.9  Build number: 9551  Build date: Feb  3 2009

        /*----------------------------------
        WoW Offset Dumper 0.1 - IDC Script
        by kynox

        Credits:
        bobbysing, Patrick, Dominik, Azorbix
        -----------------------------------*/

        // Descriptors: 0x00FC3C98
        public enum eObjectFields : uint
        {
            OBJECT_FIELD_GUID = 0x0,
            OBJECT_FIELD_TYPE = 0x2,
            OBJECT_FIELD_ENTRY = 0x3,
            OBJECT_FIELD_SCALE_X = 0x4,
            OBJECT_FIELD_PADDING = 0x5,
            TOTAL_OBJECT_FIELDS = 0x5
        }

        // Descriptors: 0x00FC4030
        public enum eUnitFields : uint
        {
            UNIT_FIELD_CHARM = 0x6,
            UNIT_FIELD_SUMMON = 0x8,
            UNIT_FIELD_CRITTER = 0xA,
            UNIT_FIELD_CHARMEDBY = 0xC,
            UNIT_FIELD_SUMMONEDBY = 0xE,
            UNIT_FIELD_CREATEDBY = 0x10,
            UNIT_FIELD_TARGET = 0x12,
            UNIT_FIELD_CHANNEL_OBJECT = 0x14,
            UNIT_FIELD_BYTES_0 = 0x16,
            UNIT_FIELD_HEALTH = 0x17,
            UNIT_FIELD_POWER1 = 0x18,
            UNIT_FIELD_POWER2 = 0x19,
            UNIT_FIELD_POWER3 = 0x1A,
            UNIT_FIELD_POWER4 = 0x1B,
            UNIT_FIELD_POWER5 = 0x1C,
            UNIT_FIELD_POWER6 = 0x1D,
            UNIT_FIELD_POWER7 = 0x1E,
            UNIT_FIELD_MAXHEALTH = 0x1F,
            UNIT_FIELD_MAXPOWER1 = 0x20,
            UNIT_FIELD_MAXPOWER2 = 0x21,
            UNIT_FIELD_MAXPOWER3 = 0x22,
            UNIT_FIELD_MAXPOWER4 = 0x23,
            UNIT_FIELD_MAXPOWER5 = 0x24,
            UNIT_FIELD_MAXPOWER6 = 0x25,
            UNIT_FIELD_MAXPOWER7 = 0x26,
            UNIT_FIELD_POWER_REGEN_FLAT_MODIFIER = 0x27,
            UNIT_FIELD_POWER_REGEN_INTERRUPTED_FLAT_MODIFIER = 0x2E,
            UNIT_FIELD_LEVEL = 0x35,
            UNIT_FIELD_FACTIONTEMPLATE = 0x36,
            UNIT_VIRTUAL_ITEM_SLOT_ID = 0x37,
            UNIT_FIELD_FLAGS = 0x3A,
            UNIT_FIELD_FLAGS_2 = 0x3B,
            UNIT_FIELD_AURASTATE = 0x3C,
            UNIT_FIELD_BASEATTACKTIME = 0x3D,
            UNIT_FIELD_RANGEDATTACKTIME = 0x3F,
            UNIT_FIELD_BOUNDINGRADIUS = 0x40,
            UNIT_FIELD_COMBATREACH = 0x41,
            UNIT_FIELD_DISPLAYID = 0x42,
            UNIT_FIELD_NATIVEDISPLAYID = 0x43,
            UNIT_FIELD_MOUNTDISPLAYID = 0x44,
            UNIT_FIELD_MINDAMAGE = 0x45,
            UNIT_FIELD_MAXDAMAGE = 0x46,
            UNIT_FIELD_MINOFFHANDDAMAGE = 0x47,
            UNIT_FIELD_MAXOFFHANDDAMAGE = 0x48,
            UNIT_FIELD_BYTES_1 = 0x49,
            UNIT_FIELD_PETNUMBER = 0x4A,
            UNIT_FIELD_PET_NAME_TIMESTAMP = 0x4B,
            UNIT_FIELD_PETEXPERIENCE = 0x4C,
            UNIT_FIELD_PETNEXTLEVELEXP = 0x4D,
            UNIT_DYNAMIC_FLAGS = 0x4E,
            UNIT_CHANNEL_SPELL = 0x4F,
            UNIT_MOD_CAST_SPEED = 0x50,
            UNIT_CREATED_BY_SPELL = 0x51,
            UNIT_NPC_FLAGS = 0x52,
            UNIT_NPC_EMOTESTATE = 0x53,
            UNIT_FIELD_STAT0 = 0x54,
            UNIT_FIELD_STAT1 = 0x55,
            UNIT_FIELD_STAT2 = 0x56,
            UNIT_FIELD_STAT3 = 0x57,
            UNIT_FIELD_STAT4 = 0x58,
            UNIT_FIELD_POSSTAT0 = 0x59,
            UNIT_FIELD_POSSTAT1 = 0x5A,
            UNIT_FIELD_POSSTAT2 = 0x5B,
            UNIT_FIELD_POSSTAT3 = 0x5C,
            UNIT_FIELD_POSSTAT4 = 0x5D,
            UNIT_FIELD_NEGSTAT0 = 0x5E,
            UNIT_FIELD_NEGSTAT1 = 0x5F,
            UNIT_FIELD_NEGSTAT2 = 0x60,
            UNIT_FIELD_NEGSTAT3 = 0x61,
            UNIT_FIELD_NEGSTAT4 = 0x62,
            UNIT_FIELD_RESISTANCES = 0x63,
            UNIT_FIELD_RESISTANCEBUFFMODSPOSITIVE = 0x6A,
            UNIT_FIELD_RESISTANCEBUFFMODSNEGATIVE = 0x71,
            UNIT_FIELD_BASE_MANA = 0x78,
            UNIT_FIELD_BASE_HEALTH = 0x79,
            UNIT_FIELD_BYTES_2 = 0x7A,
            UNIT_FIELD_ATTACK_POWER = 0x7B,
            UNIT_FIELD_ATTACK_POWER_MODS = 0x7C,
            UNIT_FIELD_ATTACK_POWER_MULTIPLIER = 0x7D,
            UNIT_FIELD_RANGED_ATTACK_POWER = 0x7E,
            UNIT_FIELD_RANGED_ATTACK_POWER_MODS = 0x7F,
            UNIT_FIELD_RANGED_ATTACK_POWER_MULTIPLIER = 0x80,
            UNIT_FIELD_MINRANGEDDAMAGE = 0x81,
            UNIT_FIELD_MAXRANGEDDAMAGE = 0x82,
            UNIT_FIELD_POWER_COST_MODIFIER = 0x83,
            UNIT_FIELD_POWER_COST_MULTIPLIER = 0x8A,
            UNIT_FIELD_MAXHEALTHMODIFIER = 0x91,
            UNIT_FIELD_HOVERHEIGHT = 0x92,
            UNIT_FIELD_PADDING = 0x93,
            TOTAL_UNIT_FIELDS = 0x59
        }

        // Descriptors: 0x00FC3D38
        public enum eItemFields : uint
        {
            ITEM_FIELD_OWNER = 0x6,
            ITEM_FIELD_CONTAINED = 0x8,
            ITEM_FIELD_CREATOR = 0xA,
            ITEM_FIELD_GIFTCREATOR = 0xC,
            ITEM_FIELD_STACK_COUNT = 0xE,
            ITEM_FIELD_DURATION = 0xF,
            ITEM_FIELD_SPELL_CHARGES = 0x10,
            ITEM_FIELD_FLAGS = 0x15,
            ITEM_FIELD_ENCHANTMENT_1_1 = 0x16,
            ITEM_FIELD_ENCHANTMENT_1_3 = 0x18,
            ITEM_FIELD_ENCHANTMENT_2_1 = 0x19,
            ITEM_FIELD_ENCHANTMENT_2_3 = 0x1B,
            ITEM_FIELD_ENCHANTMENT_3_1 = 0x1C,
            ITEM_FIELD_ENCHANTMENT_3_3 = 0x1E,
            ITEM_FIELD_ENCHANTMENT_4_1 = 0x1F,
            ITEM_FIELD_ENCHANTMENT_4_3 = 0x21,
            ITEM_FIELD_ENCHANTMENT_5_1 = 0x22,
            ITEM_FIELD_ENCHANTMENT_5_3 = 0x24,
            ITEM_FIELD_ENCHANTMENT_6_1 = 0x25,
            ITEM_FIELD_ENCHANTMENT_6_3 = 0x27,
            ITEM_FIELD_ENCHANTMENT_7_1 = 0x28,
            ITEM_FIELD_ENCHANTMENT_7_3 = 0x2A,
            ITEM_FIELD_ENCHANTMENT_8_1 = 0x2B,
            ITEM_FIELD_ENCHANTMENT_8_3 = 0x2D,
            ITEM_FIELD_ENCHANTMENT_9_1 = 0x2E,
            ITEM_FIELD_ENCHANTMENT_9_3 = 0x30,
            ITEM_FIELD_ENCHANTMENT_10_1 = 0x31,
            ITEM_FIELD_ENCHANTMENT_10_3 = 0x33,
            ITEM_FIELD_ENCHANTMENT_11_1 = 0x34,
            ITEM_FIELD_ENCHANTMENT_11_3 = 0x36,
            ITEM_FIELD_ENCHANTMENT_12_1 = 0x37,
            ITEM_FIELD_ENCHANTMENT_12_3 = 0x39,
            ITEM_FIELD_PROPERTY_SEED = 0x3A,
            ITEM_FIELD_RANDOM_PROPERTIES_ID = 0x3B,
            ITEM_FIELD_ITEM_TEXT_ID = 0x3C,
            ITEM_FIELD_DURABILITY = 0x3D,
            ITEM_FIELD_MAXDURABILITY = 0x3E,
            ITEM_FIELD_PAD = 0x3F,
            TOTAL_ITEM_FIELDS = 0x26
        }

        // Descriptors: 0x00FC4728
        public enum ePlayerFields : uint
        {
            PLAYER_DUEL_ARBITER = 0x94,
            PLAYER_FLAGS = 0x96,
            PLAYER_GUILDID = 0x97,
            PLAYER_GUILDRANK = 0x98,
            PLAYER_BYTES = 0x99,
            PLAYER_BYTES_2 = 0x9A,
            PLAYER_BYTES_3 = 0x9B,
            PLAYER_DUEL_TEAM = 0x9C,
            PLAYER_GUILD_TIMESTAMP = 0x9D,
            PLAYER_QUEST_LOG_1_1 = 0x9E,
            PLAYER_QUEST_LOG_1_2 = 0x9F,
            PLAYER_QUEST_LOG_1_3 = 0xA0,
            PLAYER_QUEST_LOG_1_4 = 0xA1,
            PLAYER_QUEST_LOG_2_1 = 0xA2,
            PLAYER_QUEST_LOG_2_2 = 0xA3,
            PLAYER_QUEST_LOG_2_3 = 0xA4,
            PLAYER_QUEST_LOG_2_4 = 0xA5,
            PLAYER_QUEST_LOG_3_1 = 0xA6,
            PLAYER_QUEST_LOG_3_2 = 0xA7,
            PLAYER_QUEST_LOG_3_3 = 0xA8,
            PLAYER_QUEST_LOG_3_4 = 0xA9,
            PLAYER_QUEST_LOG_4_1 = 0xAA,
            PLAYER_QUEST_LOG_4_2 = 0xAB,
            PLAYER_QUEST_LOG_4_3 = 0xAC,
            PLAYER_QUEST_LOG_4_4 = 0xAD,
            PLAYER_QUEST_LOG_5_1 = 0xAE,
            PLAYER_QUEST_LOG_5_2 = 0xAF,
            PLAYER_QUEST_LOG_5_3 = 0xB0,
            PLAYER_QUEST_LOG_5_4 = 0xB1,
            PLAYER_QUEST_LOG_6_1 = 0xB2,
            PLAYER_QUEST_LOG_6_2 = 0xB3,
            PLAYER_QUEST_LOG_6_3 = 0xB4,
            PLAYER_QUEST_LOG_6_4 = 0xB5,
            PLAYER_QUEST_LOG_7_1 = 0xB6,
            PLAYER_QUEST_LOG_7_2 = 0xB7,
            PLAYER_QUEST_LOG_7_3 = 0xB8,
            PLAYER_QUEST_LOG_7_4 = 0xB9,
            PLAYER_QUEST_LOG_8_1 = 0xBA,
            PLAYER_QUEST_LOG_8_2 = 0xBB,
            PLAYER_QUEST_LOG_8_3 = 0xBC,
            PLAYER_QUEST_LOG_8_4 = 0xBD,
            PLAYER_QUEST_LOG_9_1 = 0xBE,
            PLAYER_QUEST_LOG_9_2 = 0xBF,
            PLAYER_QUEST_LOG_9_3 = 0xC0,
            PLAYER_QUEST_LOG_9_4 = 0xC1,
            PLAYER_QUEST_LOG_10_1 = 0xC2,
            PLAYER_QUEST_LOG_10_2 = 0xC3,
            PLAYER_QUEST_LOG_10_3 = 0xC4,
            PLAYER_QUEST_LOG_10_4 = 0xC5,
            PLAYER_QUEST_LOG_11_1 = 0xC6,
            PLAYER_QUEST_LOG_11_2 = 0xC7,
            PLAYER_QUEST_LOG_11_3 = 0xC8,
            PLAYER_QUEST_LOG_11_4 = 0xC9,
            PLAYER_QUEST_LOG_12_1 = 0xCA,
            PLAYER_QUEST_LOG_12_2 = 0xCB,
            PLAYER_QUEST_LOG_12_3 = 0xCC,
            PLAYER_QUEST_LOG_12_4 = 0xCD,
            PLAYER_QUEST_LOG_13_1 = 0xCE,
            PLAYER_QUEST_LOG_13_2 = 0xCF,
            PLAYER_QUEST_LOG_13_3 = 0xD0,
            PLAYER_QUEST_LOG_13_4 = 0xD1,
            PLAYER_QUEST_LOG_14_1 = 0xD2,
            PLAYER_QUEST_LOG_14_2 = 0xD3,
            PLAYER_QUEST_LOG_14_3 = 0xD4,
            PLAYER_QUEST_LOG_14_4 = 0xD5,
            PLAYER_QUEST_LOG_15_1 = 0xD6,
            PLAYER_QUEST_LOG_15_2 = 0xD7,
            PLAYER_QUEST_LOG_15_3 = 0xD8,
            PLAYER_QUEST_LOG_15_4 = 0xD9,
            PLAYER_QUEST_LOG_16_1 = 0xDA,
            PLAYER_QUEST_LOG_16_2 = 0xDB,
            PLAYER_QUEST_LOG_16_3 = 0xDC,
            PLAYER_QUEST_LOG_16_4 = 0xDD,
            PLAYER_QUEST_LOG_17_1 = 0xDE,
            PLAYER_QUEST_LOG_17_2 = 0xDF,
            PLAYER_QUEST_LOG_17_3 = 0xE0,
            PLAYER_QUEST_LOG_17_4 = 0xE1,
            PLAYER_QUEST_LOG_18_1 = 0xE2,
            PLAYER_QUEST_LOG_18_2 = 0xE3,
            PLAYER_QUEST_LOG_18_3 = 0xE4,
            PLAYER_QUEST_LOG_18_4 = 0xE5,
            PLAYER_QUEST_LOG_19_1 = 0xE6,
            PLAYER_QUEST_LOG_19_2 = 0xE7,
            PLAYER_QUEST_LOG_19_3 = 0xE8,
            PLAYER_QUEST_LOG_19_4 = 0xE9,
            PLAYER_QUEST_LOG_20_1 = 0xEA,
            PLAYER_QUEST_LOG_20_2 = 0xEB,
            PLAYER_QUEST_LOG_20_3 = 0xEC,
            PLAYER_QUEST_LOG_20_4 = 0xED,
            PLAYER_QUEST_LOG_21_1 = 0xEE,
            PLAYER_QUEST_LOG_21_2 = 0xEF,
            PLAYER_QUEST_LOG_21_3 = 0xF0,
            PLAYER_QUEST_LOG_21_4 = 0xF1,
            PLAYER_QUEST_LOG_22_1 = 0xF2,
            PLAYER_QUEST_LOG_22_2 = 0xF3,
            PLAYER_QUEST_LOG_22_3 = 0xF4,
            PLAYER_QUEST_LOG_22_4 = 0xF5,
            PLAYER_QUEST_LOG_23_1 = 0xF6,
            PLAYER_QUEST_LOG_23_2 = 0xF7,
            PLAYER_QUEST_LOG_23_3 = 0xF8,
            PLAYER_QUEST_LOG_23_4 = 0xF9,
            PLAYER_QUEST_LOG_24_1 = 0xFA,
            PLAYER_QUEST_LOG_24_2 = 0xFB,
            PLAYER_QUEST_LOG_24_3 = 0xFC,
            PLAYER_QUEST_LOG_24_4 = 0xFD,
            PLAYER_QUEST_LOG_25_1 = 0xFE,
            PLAYER_QUEST_LOG_25_2 = 0xFF,
            PLAYER_QUEST_LOG_25_3 = 0x100,
            PLAYER_QUEST_LOG_25_4 = 0x101,
            PLAYER_VISIBLE_ITEM_1_ENTRYID = 0x102,
            PLAYER_VISIBLE_ITEM_1_ENCHANTMENT = 0x103,
            PLAYER_VISIBLE_ITEM_2_ENTRYID = 0x104,
            PLAYER_VISIBLE_ITEM_2_ENCHANTMENT = 0x105,
            PLAYER_VISIBLE_ITEM_3_ENTRYID = 0x106,
            PLAYER_VISIBLE_ITEM_3_ENCHANTMENT = 0x107,
            PLAYER_VISIBLE_ITEM_4_ENTRYID = 0x108,
            PLAYER_VISIBLE_ITEM_4_ENCHANTMENT = 0x109,
            PLAYER_VISIBLE_ITEM_5_ENTRYID = 0x10A,
            PLAYER_VISIBLE_ITEM_5_ENCHANTMENT = 0x10B,
            PLAYER_VISIBLE_ITEM_6_ENTRYID = 0x10C,
            PLAYER_VISIBLE_ITEM_6_ENCHANTMENT = 0x10D,
            PLAYER_VISIBLE_ITEM_7_ENTRYID = 0x10E,
            PLAYER_VISIBLE_ITEM_7_ENCHANTMENT = 0x10F,
            PLAYER_VISIBLE_ITEM_8_ENTRYID = 0x110,
            PLAYER_VISIBLE_ITEM_8_ENCHANTMENT = 0x111,
            PLAYER_VISIBLE_ITEM_9_ENTRYID = 0x112,
            PLAYER_VISIBLE_ITEM_9_ENCHANTMENT = 0x113,
            PLAYER_VISIBLE_ITEM_10_ENTRYID = 0x114,
            PLAYER_VISIBLE_ITEM_10_ENCHANTMENT = 0x115,
            PLAYER_VISIBLE_ITEM_11_ENTRYID = 0x116,
            PLAYER_VISIBLE_ITEM_11_ENCHANTMENT = 0x117,
            PLAYER_VISIBLE_ITEM_12_ENTRYID = 0x118,
            PLAYER_VISIBLE_ITEM_12_ENCHANTMENT = 0x119,
            PLAYER_VISIBLE_ITEM_13_ENTRYID = 0x11A,
            PLAYER_VISIBLE_ITEM_13_ENCHANTMENT = 0x11B,
            PLAYER_VISIBLE_ITEM_14_ENTRYID = 0x11C,
            PLAYER_VISIBLE_ITEM_14_ENCHANTMENT = 0x11D,
            PLAYER_VISIBLE_ITEM_15_ENTRYID = 0x11E,
            PLAYER_VISIBLE_ITEM_15_ENCHANTMENT = 0x11F,
            PLAYER_VISIBLE_ITEM_16_ENTRYID = 0x120,
            PLAYER_VISIBLE_ITEM_16_ENCHANTMENT = 0x121,
            PLAYER_VISIBLE_ITEM_17_ENTRYID = 0x122,
            PLAYER_VISIBLE_ITEM_17_ENCHANTMENT = 0x123,
            PLAYER_VISIBLE_ITEM_18_ENTRYID = 0x124,
            PLAYER_VISIBLE_ITEM_18_ENCHANTMENT = 0x125,
            PLAYER_VISIBLE_ITEM_19_ENTRYID = 0x126,
            PLAYER_VISIBLE_ITEM_19_ENCHANTMENT = 0x127,
            PLAYER_CHOSEN_TITLE = 0x128,
            PLAYER_FIELD_PAD_0 = 0x129,
            PLAYER_FIELD_INV_SLOT_HEAD = 0x12A,
            PLAYER_FIELD_PACK_SLOT_1 = 0x158,
            PLAYER_FIELD_BANK_SLOT_1 = 0x178,
            PLAYER_FIELD_BANKBAG_SLOT_1 = 0x1B0,
            PLAYER_FIELD_VENDORBUYBACK_SLOT_1 = 0x1BE,
            PLAYER_FIELD_KEYRING_SLOT_1 = 0x1D6,
            PLAYER_FIELD_CURRENCYTOKEN_SLOT_1 = 0x216,
            PLAYER_FARSIGHT = 0x256,
            PLAYER__FIELD_KNOWN_TITLES = 0x258,
            PLAYER__FIELD_KNOWN_TITLES1 = 0x25A,
            PLAYER__FIELD_KNOWN_TITLES2 = 0x25C,
            PLAYER_FIELD_KNOWN_CURRENCIES = 0x25E,
            PLAYER_XP = 0x260,
            PLAYER_NEXT_LEVEL_XP = 0x261,
            PLAYER_SKILL_INFO_1_1 = 0x262,
            PLAYER_CHARACTER_POINTS1 = 0x3E2,
            PLAYER_CHARACTER_POINTS2 = 0x3E3,
            PLAYER_TRACK_CREATURES = 0x3E4,
            PLAYER_TRACK_RESOURCES = 0x3E5,
            PLAYER_BLOCK_PERCENTAGE = 0x3E6,
            PLAYER_DODGE_PERCENTAGE = 0x3E7,
            PLAYER_PARRY_PERCENTAGE = 0x3E8,
            PLAYER_EXPERTISE = 0x3E9,
            PLAYER_OFFHAND_EXPERTISE = 0x3EA,
            PLAYER_CRIT_PERCENTAGE = 0x3EB,
            PLAYER_RANGED_CRIT_PERCENTAGE = 0x3EC,
            PLAYER_OFFHAND_CRIT_PERCENTAGE = 0x3ED,
            PLAYER_SPELL_CRIT_PERCENTAGE1 = 0x3EE,
            PLAYER_SHIELD_BLOCK = 0x3F5,
            PLAYER_SHIELD_BLOCK_CRIT_PERCENTAGE = 0x3F6,
            PLAYER_EXPLORED_ZONES_1 = 0x3F7,
            PLAYER_REST_STATE_EXPERIENCE = 0x477,
            PLAYER_FIELD_COINAGE = 0x478,
            PLAYER_FIELD_MOD_DAMAGE_DONE_POS = 0x479,
            PLAYER_FIELD_MOD_DAMAGE_DONE_NEG = 0x480,
            PLAYER_FIELD_MOD_DAMAGE_DONE_PCT = 0x487,
            PLAYER_FIELD_MOD_HEALING_DONE_POS = 0x48E,
            PLAYER_FIELD_MOD_TARGET_RESISTANCE = 0x48F,
            PLAYER_FIELD_MOD_TARGET_PHYSICAL_RESISTANCE = 0x490,
            PLAYER_FIELD_BYTES = 0x491,
            PLAYER_AMMO_ID = 0x492,
            PLAYER_SELF_RES_SPELL = 0x493,
            PLAYER_FIELD_PVP_MEDALS = 0x494,
            PLAYER_FIELD_BUYBACK_PRICE_1 = 0x495,
            PLAYER_FIELD_BUYBACK_TIMESTAMP_1 = 0x4A1,
            PLAYER_FIELD_KILLS = 0x4AD,
            PLAYER_FIELD_TODAY_CONTRIBUTION = 0x4AE,
            PLAYER_FIELD_YESTERDAY_CONTRIBUTION = 0x4AF,
            PLAYER_FIELD_LIFETIME_HONORBALE_KILLS = 0x4B0,
            PLAYER_FIELD_BYTES2 = 0x4B1,
            PLAYER_FIELD_WATCHED_FACTION_INDEX = 0x4B2,
            PLAYER_FIELD_COMBAT_RATING_1 = 0x4B3,
            PLAYER_FIELD_ARENA_TEAM_INFO_1_1 = 0x4CC,
            PLAYER_FIELD_HONOR_CURRENCY = 0x4DE,
            PLAYER_FIELD_ARENA_CURRENCY = 0x4DF,
            PLAYER_FIELD_MAX_LEVEL = 0x4E0,
            PLAYER_FIELD_DAILY_QUESTS_1 = 0x4E1,
            PLAYER_RUNE_REGEN_1 = 0x4FA,
            PLAYER_NO_REAGENT_COST_1 = 0x4FE,
            PLAYER_FIELD_GLYPH_SLOTS_1 = 0x501,
            PLAYER_FIELD_GLYPHS_1 = 0x507,
            PLAYER_GLYPHS_ENABLED = 0x50D,
            TOTAL_PLAYER_FIELDS = 0xD3
        }

        // Descriptors: 0x00FC3CFC
        public enum eContainerFields : uint
        {
            CONTAINER_FIELD_NUM_SLOTS = 0x6,
            CONTAINER_ALIGN_PAD = 0x7,
            CONTAINER_FIELD_SLOT_1 = 0x8,
            TOTAL_CONTAINER_FIELDS = 0x3
        }

        // Descriptors: 0x00FC5C30
        public enum eGameObjectFields : uint
        {
            OBJECT_FIELD_CREATED_BY = 0x6,
            GAMEOBJECT_DISPLAYID = 0x8,
            GAMEOBJECT_FLAGS = 0x9,
            GAMEOBJECT_PARENTROTATION = 0xA,
            GAMEOBJECT_DYNAMIC = 0xE,
            GAMEOBJECT_FACTION = 0xF,
            GAMEOBJECT_LEVEL = 0x10,
            GAMEOBJECT_BYTES_1 = 0x11,
            TOTAL_GAMEOBJECT_FIELDS = 0x8
        }

        // Descriptors: 0x00FC5D38
        public enum eDynamicObjectFields : uint
        {
            DYNAMICOBJECT_CASTER = 0x6,
            DYNAMICOBJECT_BYTES = 0x8,
            DYNAMICOBJECT_SPELLID = 0x9,
            DYNAMICOBJECT_RADIUS = 0xA,
            DYNAMICOBJECT_POS_X = 0xB,
            DYNAMICOBJECT_POS_Y = 0xC,
            DYNAMICOBJECT_POS_Z = 0xD,
            DYNAMICOBJECT_FACING = 0xE,
            DYNAMICOBJECT_CASTTIME = 0xF,
            TOTAL_DYNAMICOBJECT_FIELDS = 0x9
        }

        // Descriptors: 0x00FC5DF0
        public enum eCorpseFields : uint
        {
            CORPSE_FIELD_OWNER = 0x6,
            CORPSE_FIELD_PARTY = 0x8,
            CORPSE_FIELD_DISPLAY_ID = 0xA,
            CORPSE_FIELD_ITEM = 0xB,
            CORPSE_FIELD_BYTES_1 = 0x1E,
            CORPSE_FIELD_BYTES_2 = 0x1F,
            CORPSE_FIELD_GUILD = 0x20,
            CORPSE_FIELD_FLAGS = 0x21,
            CORPSE_FIELD_DYNAMIC_FLAGS = 0x22,
            CORPSE_FIELD_PAD = 0x23,
            TOTAL_CORPSE_FIELDS = 0xA
        }

        public enum eObjType : uint
        {
            OT_NONE = 0,
            OT_ITEM = 1,
            OT_CONTAINER = 2,
            OT_UNIT = 3,
            OT_PLAYER = 4,
            OT_GAMEOBJ = 5,
            OT_DYNOBJ = 6,
            OT_CORPSE = 7,
            OT_FORCEDWORD = 0xFFFFFFFF
        } // Credit ISXWoW

        public enum eGObjType : uint
        {
            GO_MISC = 0,
            GO_HERB = 0x9260000, //0x110
            GO_MINERAL = 0x92A1000//0x1001
        };

        public enum ePlayerClass : uint
        {
            CLASS_NONE = 0,
            CLASS_WARRIOR = 1,
            CLASS_PALADIN = 2,
            CLASS_HUNTER = 3,
            CLASS_ROGUE = 4,
            CLASS_PRIEST = 5,
            CLASS_SHAMAN = 7,
            CLASS_MAGE = 8,
            CLASS_WARLOCK = 9,
            CLASS_DRUID = 11
        }; // Credit ISXWoW

        public enum eUnitClassification : uint
        {
            UC_NORMAL = 0,
            UC_ELITE = 1,
            UC_RAREELITE = 2,
            UC_WORLDBOSS = 3,
            UC_RARE = 4
        }; // Credit ISXWoW

        public enum ePlayerRace : uint
        {
            RACE_NONE = 0,
            RACE_HUMAN = 1,
            RACE_ORC = 2,
            RACE_DWARF = 3,
            RACE_NIGHTELF = 4,
            RACE_UNDEAD = 5,
            RACE_TAUREN = 6,
            RACE_GNOME = 7,
            RACE_TROLL = 8,
            RACE_BLOODELF = 10,
            RACE_DRAENEI = 11
        }; // Credit ISXWoW

        public enum eUnitNPCFlags : uint
        {
            NPC_FLAG_CHAT = 0x1, // 1
            // 2
            NPC_FLAG_MERCHANT = 0x4,
            NPC_FLAG_GRIFFON_MASTER = 0x8,
            NPC_FLAG_SPIRIT_HEALER = 0x20,
            NPC_FLAG_INNKEEPER = 0x80,
            NPC_FLAG_BANKER = 0x100,
            NPC_FLAG_AUCTIONEER = 0x1000,
            NPC_FLAG_CAN_REPAIR = 0x4000
        }; // Credit ISXWoW

        public enum eUnitFlags : uint
        {
            UF_SITTING = 0x01,
            UF_ELITE = 0x40,
            UF_DEAD = 0x40000,
            UF_FLYING = 0x100000
        }; // Credit ISXWoW

        public enum eUnitTypes : uint
        {
            UT_UNKNOWN = 0,
            UT_CRITTER,
            UT_DRAGONKIN,
            UT_DEMON,
            UT_ELEMENTAL,
            UT_GIANT,
            UT_UNDEAD,
            UT_HUMANOID,
            UT_BEAST,
            UT_MECHANIC
        };

        public enum eUnitRelation : uint
        {
            UR_UNKNOWN1 = 0,
            UR_ENEMY,
            UR_UNKNOWN2,
            UR_NEUTRAL,
            UR_FRIEND
        };

        public enum eClientDB : uint
        {
            DB_Achievement = 0xE7, // 0x00A2FAD4
            DB_Achievement_Criteria = 0xE8, // 0x00A2FAF8
            DB_Achievement_Category = 0xE9, // 0x00A2FB1C
            DB_AnimationData = 0xEA, // 0x00A2FB40
            DB_AreaGroup = 0xEB, // 0x00A2FB64
            DB_AreaPOI = 0xEC, // 0x00A2FB88
            DB_AreaTable = 0xED, // 0x00A2FBAC
            DB_AreaTrigger = 0xEE, // 0x00A2FBD0
            DB_AttackAnimKits = 0xEF, // 0x00A2FBF4
            DB_AttackAnimTypes = 0xF0, // 0x00A2FC18
            DB_AuctionHouse = 0xF1, // 0x00A2FC3C
            DB_BankBagSlotPrices = 0xF2, // 0x00A2FC60
            DB_BannedAddOns = 0xF3, // 0x00A2FC84
            DB_BarberShopStyle = 0xF4, // 0x00A2FCA8
            DB_BattlemasterList = 0xF5, // 0x00A2FCCC
            DB_CameraShakes = 0xF6, // 0x00A2FCF0
            DB_Cfg_Categories = 0xF7, // 0x00A2FD14
            DB_Cfg_Configs = 0xF8, // 0x00A2FD38
            DB_CharBaseInfo = 0xF9, // 0x00A2FD5C
            DB_CharHairGeosets = 0xFA, // 0x00A2FD80
            DB_CharSections = 0xFB, // 0x00A2FDA4
            DB_CharStartOutfit = 0xFC, // 0x00A2FDC8
            DB_CharTitles = 0xFD, // 0x00A2FDEC
            DB_CharacterFacialHairStyles = 0xFE, // 0x00A2FE10
            DB_ChatChannels = 0xFF, // 0x00A2FE34
            DB_ChatProfanity = 0x100, // 0x00A2FE58
            DB_ChrClasses = 0x101, // 0x00A2FE7C
            DB_ChrRaces = 0x102, // 0x00A2FEA0
            DB_CinematicCamera = 0x103, // 0x00A2FEC4
            DB_CinematicSequences = 0x104, // 0x00A2FEE8
            DB_CreatureDisplayInfo = 0x105, // 0x00A2FF30
            DB_CreatureDisplayInfoExtra = 0x106, // 0x00A2FF0C
            DB_CreatureFamily = 0x107, // 0x00A2FF54
            DB_CreatureModelData = 0x108, // 0x00A2FF78
            DB_CreatureMovementInfo = 0x109, // 0x00A2FF9C
            DB_CreatureSoundData = 0x10A, // 0x00A2FFC0
            DB_CreatureSpellData = 0x10B, // 0x00A2FFE4
            DB_CreatureType = 0x10C, // 0x00A30008
            DB_CurrencyTypes = 0x10D, // 0x00A3002C
            DB_CurrencyCategory = 0x10E, // 0x00A30050
            DB_DanceMoves = 0x10F, // 0x00A30074
            DB_DeathThudLookups = 0x110, // 0x00A30098
            DB_DestructibleModelData = 0x111, // 0x00A30104
            DB_DungeonMap = 0x112, // 0x00A30128
            DB_DungeonMapChunk = 0x113, // 0x00A3014C
            DB_DurabilityCosts = 0x114, // 0x00A30170
            DB_DurabilityQuality = 0x115, // 0x00A30194
            DB_Emotes = 0x116, // 0x00A301B8
            DB_EmotesText = 0x117, // 0x00A30224
            DB_EmotesTextData = 0x118, // 0x00A301DC
            DB_EmotesTextSound = 0x119, // 0x00A30200
            DB_EnvironmentalDamage = 0x11A, // 0x00A30248
            DB_Exhaustion = 0x11B, // 0x00A3026C
            DB_Faction = 0x11C, // 0x00A302B4
            DB_FactionGroup = 0x11D, // 0x00A30290
            DB_FactionTemplate = 0x11E, // 0x00A302D8
            DB_FileData = 0x11F, // 0x00A302FC
            DB_FootprintTextures = 0x120, // 0x00A30320
            DB_FootstepTerrainLookup = 0x121, // 0x00A30344
            DB_GameObjectArtKit = 0x122, // 0x00A30368
            DB_GameObjectDisplayInfo = 0x123, // 0x00A3038C
            DB_GameTables = 0x124, // 0x00A303B0
            DB_GameTips = 0x125, // 0x00A303D4
            DB_GemProperties = 0x126, // 0x00A303F8
            DB_GlyphProperties = 0x127, // 0x00A3041C
            DB_GlyphSlot = 0x128, // 0x00A30440
            DB_GMSurveyAnswers = 0x129, // 0x00A30464
            DB_GMSurveyCurrentSurvey = 0x12A, // 0x00A30488
            DB_GMSurveyQuestions = 0x12B, // 0x00A304AC
            DB_GMSurveySurveys = 0x12C, // 0x00A304D0
            DB_GMTicketCategory = 0x12D, // 0x00A304F4
            DB_GroundEffectDoodad = 0x12E, // 0x00A30518
            DB_GroundEffectTexture = 0x12F, // 0x00A3053C
            DB_gtBarberShopCostBase = 0x130, // 0x00A30560
            DB_gtCombatRatings = 0x131, // 0x00A30584
            DB_gtChanceToMeleeCrit = 0x132, // 0x00A305A8
            DB_gtChanceToMeleeCritBase = 0x133, // 0x00A305CC
            DB_gtChanceToSpellCrit = 0x134, // 0x00A305F0
            DB_gtChanceToSpellCritBase = 0x135, // 0x00A30614
            DB_gtNPCManaCostScaler = 0x136, // 0x00A30638
            DB_gtOCTClassCombatRatingScalar = 0x137, // 0x00A3065C
            DB_gtOCTRegenHP = 0x138, // 0x00A30680
            DB_gtOCTRegenMP = 0x139, // 0x00A306A4
            DB_gtRegenHPPerSpt = 0x13A, // 0x00A306C8
            DB_gtRegenMPPerSpt = 0x13B, // 0x00A306EC
            DB_HelmetGeosetVisData = 0x13C, // 0x00A30710
            DB_HolidayDescriptions = 0x13D, // 0x00A30734
            DB_HolidayNames = 0x13E, // 0x00A30758
            DB_Holidays = 0x13F, // 0x00A3077C
            DB_Item = 0x140, // 0x00A307A0
            DB_ItemBagFamily = 0x141, // 0x00A307C4
            DB_ItemClass = 0x142, // 0x00A307E8
            DB_ItemCondExtCosts = 0x143, // 0x00A3080C
            DB_ItemDisplayInfo = 0x144, // 0x00A30830
            DB_ItemExtendedCost = 0x145, // 0x00A30854
            DB_ItemGroupSounds = 0x146, // 0x00A30878
            DB_ItemLimitCategory = 0x147, // 0x00A3089C
            DB_ItemPetFood = 0x148, // 0x00A308C0
            DB_ItemPurchaseGroup = 0x149, // 0x00A308E4
            DB_ItemRandomProperties = 0x14A, // 0x00A30908
            DB_ItemRandomSuffix = 0x14B, // 0x00A3092C
            DB_ItemSet = 0x14C, // 0x00A30950
            DB_ItemSubClass = 0x14D, // 0x00A30998
            DB_ItemSubClassMask = 0x14E, // 0x00A30974
            DB_ItemVisualEffects = 0x14F, // 0x00A309BC
            DB_ItemVisuals = 0x150, // 0x00A309E0
            DB_LanguageWords = 0x151, // 0x00A30A04
            DB_Languages = 0x152, // 0x00A30A28
            DB_LfgDungeons = 0x153, // 0x00A30A4C
            DB_Light = 0x154, // 0x00A16E10
            DB_LightFloatBand = 0x155, // 0x00A16DC8
            DB_LightIntBand = 0x156, // 0x00A16DA4
            DB_LightParams = 0x157, // 0x00A16DEC
            DB_LightSkybox = 0x158, // 0x00A16D80
            DB_LiquidType = 0x159, // 0x00A30A70
            DB_LiquidMaterial = 0x15A, // 0x00A30A94
            DB_LoadingScreens = 0x15B, // 0x00A30AB8
            DB_LoadingScreenTaxiSplines = 0x15C, // 0x00A30ADC
            DB_Lock = 0x15D, // 0x00A30B00
            DB_LockType = 0x15E, // 0x00A30B24
            DB_MailTemplate = 0x15F, // 0x00A30B48
            DB_Map = 0x160, // 0x00A30B6C
            DB_Material = 0x161, // 0x00A30B90
            DB_Movie = 0x162, // 0x00A30BB4
            DB_MovieFileData = 0x163, // 0x00A30BD8
            DB_MovieVariation = 0x164, // 0x00A30BFC
            DB_NameGen = 0x165, // 0x00A30C20
            DB_NPCSounds = 0x166, // 0x00A30C44
            DB_NamesProfanity = 0x167, // 0x00A30C68
            DB_NamesReserved = 0x168, // 0x00A30C8C
            DB_OverrideSpellData = 0x169, // 0x00A30CB0
            DB_Package = 0x16A, // 0x00A30CD4
            DB_PageTextMaterial = 0x16B, // 0x00A30CF8
            DB_PaperDollItemFrame = 0x16C, // 0x00A30D1C
            DB_ParticleColor = 0x16D, // 0x00A30D40
            DB_PetPersonality = 0x16E, // 0x00A30D64
            DB_PowerDisplay = 0x16F, // 0x00A30D88
            DB_QuestInfo = 0x170, // 0x00A30DAC
            DB_QuestSort = 0x171, // 0x00A30DD0
            DB_Resistances = 0x172, // 0x00A30DF4
            DB_RandPropPoints = 0x173, // 0x00A30E18
            DB_ScalingStatDistribution = 0x174, // 0x00A30E3C
            DB_ScalingStatValues = 0x175, // 0x00A30E60
            DB_ScreenEffect = 0x176, // 0x00A30E84
            DB_ServerMessages = 0x177, // 0x00A30EA8
            DB_SheatheSoundLookups = 0x178, // 0x00A30ECC
            DB_SkillCostsData = 0x179, // 0x00A30EF0
            DB_SkillLineAbility = 0x17A, // 0x00A30F14
            DB_SkillLineCategory = 0x17B, // 0x00A30F38
            DB_SkillLine = 0x17C, // 0x00A30F5C
            DB_SkillRaceClassInfo = 0x17D, // 0x00A30F80
            DB_SkillTiers = 0x17E, // 0x00A30FA4
            DB_SoundAmbience = 0x17F, // 0x00A30FC8
            DB_SoundEmitters = 0x180, // 0x00A31010
            DB_SoundEntries = 0x181, // 0x00A30FEC
            DB_SoundProviderPreferences = 0x182, // 0x00A31034
            DB_SoundSamplePreferences = 0x183, // 0x00A31058
            DB_SoundWaterType = 0x184, // 0x00A3107C
            DB_SpamMessages = 0x185, // 0x00A310A0
            DB_SpellCastTimes = 0x186, // 0x00A310C4
            DB_SpellCategory = 0x187, // 0x00A310E8
            DB_SpellChainEffects = 0x188, // 0x00A3110C
            DB_Spell = 0x189, // 0x00A31304
            DB_SpellDispelType = 0x18A, // 0x00A31130
            DB_SpellDuration = 0x18B, // 0x00A31154
            DB_SpellEffectCameraShakes = 0x18C, // 0x00A31178
            DB_SpellFocusObject = 0x18D, // 0x00A3119C
            DB_SpellIcon = 0x18E, // 0x00A311C0
            DB_SpellItemEnchantment = 0x18F, // 0x00A311E4
            DB_SpellItemEnchantmentCondition = 0x190, // 0x00A31208
            DB_SpellMechanic = 0x191, // 0x00A3122C
            DB_SpellMissile = 0x192, // 0x00A31250
            DB_SpellMissileMotion = 0x193, // 0x00A31274
            DB_SpellRadius = 0x194, // 0x00A31298
            DB_SpellRange = 0x195, // 0x00A312BC
            DB_SpellRuneCost = 0x196, // 0x00A312E0
            DB_SpellShapeshiftForm = 0x197, // 0x00A31328
            DB_SpellVisual = 0x198, // 0x00A313B8
            DB_SpellVisualEffectName = 0x199, // 0x00A3134C
            DB_SpellVisualKit = 0x19A, // 0x00A31370
            DB_SpellVisualKitAreaModel = 0x19B, // 0x00A31394
            DB_StableSlotPrices = 0x19C, // 0x00A313DC
            DB_Stationery = 0x19D, // 0x00A31400
            DB_StringLookups = 0x19E, // 0x00A31424
            DB_SummonProperties = 0x19F, // 0x00A31448
            DB_Talent = 0x1A0, // 0x00A3146C
            DB_TalentTab = 0x1A1, // 0x00A31490
            DB_TaxiNodes = 0x1A2, // 0x00A314B4
            DB_TaxiPath = 0x1A3, // 0x00A314FC
            DB_TaxiPathNode = 0x1A4, // 0x00A314D8
            DB_TerrainType = 0x1A5, // 0x00A31520
            DB_TerrainTypeSounds = 0x1A6, // 0x00A31544
            DB_TotemCategory = 0x1A7, // 0x00A31568
            DB_TransportAnimation = 0x1A8, // 0x00A3158C
            DB_TransportPhysics = 0x1A9, // 0x00A315B0
            DB_TransportRotation = 0x1AA, // 0x00A315D4
            DB_UISoundLookups = 0x1AB, // 0x00A315F8
            DB_UnitBlood = 0x1AC, // 0x00A31640
            DB_UnitBloodLevels = 0x1AD, // 0x00A3161C
            DB_Vehicle = 0x1AE, // 0x00A31664
            DB_VehicleSeat = 0x1AF, // 0x00A31688
            DB_VocalUISounds = 0x1B0, // 0x00A316AC
            DB_WMOAreaTable = 0x1B1, // 0x00A316D0
            DB_WeaponImpactSounds = 0x1B2, // 0x00A316F4
            DB_WeaponSwingSounds2 = 0x1B3, // 0x00A31718
            DB_Weather = 0x1B4, // 0x00A3173C
            DB_WorldMapArea = 0x1B5, // 0x00A31760
            DB_WorldMapTransforms = 0x1B6, // 0x00A317CC
            DB_WorldMapContinent = 0x1B7, // 0x00A31784
            DB_WorldMapOverlay = 0x1B8, // 0x00A317A8
            DB_WorldSafeLocs = 0x1B9, // 0x00A317F0
            DB_WorldStateUI = 0x1BA, // 0x00A31814
            DB_ZoneIntroMusicTable = 0x1BB, // 0x00A31838
            DB_ZoneMusic = 0x1BC, // 0x00A3185C
            DB_WorldStateZoneSounds = 0x1BD, // 0x00A31880
            DB_WorldChunkSounds = 0x1BE, // 0x00A318A4
            DB_SoundEntriesAdvanced = 0x1BF, // 0x00A318C8
            DB_ObjectEffect = 0x1C0, // 0x00A318EC
            DB_ObjectEffectGroup = 0x1C1, // 0x00A31910
            DB_ObjectEffectModifier = 0x1C2, // 0x00A31934
            DB_ObjectEffectPackage = 0x1C3, // 0x00A31958
            DB_ObjectEffectPackageElem = 0x1C4, // 0x00A3197C
            DB_SoundFilter = 0x1C5, // 0x00A319A0
            DB_SoundFilterElem = 0x1C6, // 0x00A319C4

            DB_COUNT = 0xE0
        };


    }
#pragma warning restore 1591
}
