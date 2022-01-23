using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using System.Collections.Generic;
using DiskCardGame;
using APIPlugin;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System;
using System.IO;

namespace Act3Cards {
	[BepInPlugin(GUID, Name, Version)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class MainPlugin : BaseUnityPlugin {

		internal const string GUID = "io.github.TeamDoodz." + Name;
		internal const string Name = nameof(Act3Cards);
		internal const string Version = "0.2.0";
		internal static ManualLogSource logger;

		internal static string[] Blacklist = new string[] {
			"!CORRUPTED", // Corrupted card that P03 throws at you at the end of act 2
			"!INSPECTOR", // Inspector card that P03 uses during his fight
			"!MELTER", // Melter card that P03 uses during his fight
			"!DREDGER", // Dredger card that P03 uses during his fight
			"!BOUNTYHUNTER_BASE", // Act 3 bounty hunter
			"!BUILDACARD_BASE", // Player-built card from act 3
			"!FRIENDCARD_BASE", // Card with the name and portrait of one of the player's steam friends, used in G0LLY's fight
			"!MYCO_OLD_DATA", // Something to do with the mycologist boss fight
			"!MYCOCARD_BASE", // Card you get after defeating the mycologists
			"Angler_Fish_Bad", // One of the cards you can get when the angler (from act 3) perishes
			"Angler_Fish_Good", // One of the cards you can get when the angler (from act 3) perishes
			"Angler_Fish_More", // One of the cards you can get when the angler (from act 3) perishes
			"Angler_Talking", // The angler from act 3
			"BlueMage_Talking", // The lonely mage from act 3
			"DUMMY_5-5", // IDK
			"Mole_Telegrapher", // Mole card that G0LLY uses
			"Mummy_Telegrapher", // Mummy card that G0LLY uses
			"NullConduit", // Null conduit. Empty vessels already have that sigil so this card is pretty useles
			"CaptiveFile", // Card you get during the archivist boss fight with the "Hostage File" sigil
			"Librarian", // Librarian cards that the archivist spams
			"EmptyVessel", // The side deck used in Act 3
			"Skeleton", // Obtainable card in Act 2, used as a side deck in Grimora's finale sequence
			"EmptyVessel_BlueGem", // Blue gem version of the Empty Vessel
			"EmptyVessel_GreenGem", // Green gem version of the Empty Vessel
			"EmptyVessel_OrangeGem", // Orange gem version of the Empty Vessel
			"AnnoyTower", // Annoy tower used in one of the Act 3 fights
			"BrokenBot", // Broken bots that appear sometimes in Act 3 fights
			"ConduitTower", // Conduit towers that appear in some Act 3 fights
			"DeadTree", // Card found in some Act 3 fights
			"TombStone", // Tomb stone from some act 3 fights
			"CXformerAdder", // Adder beast mode bot
			"CXformerElk", // Elk beast mode bot
			"CXformerRaven", // Raven beast mode bot
			"CXformerWolf", // Wolf beast mode bot
			"XformerBatBeast", // S0N1A
			"XformerGrizzlyBeast", // GR1ZZ
			"XformerPorcupineBeast", // QU177
			"BlueMage", // Regular blue mage. Sigil is busted and does nothing ATM
			"BlueMage_Fused",
			"CagedWolf", // The caged wolf you get from the wardrobe. Since there is no normal way to get it the program thinks its not from Act 1
			"CatUndead", // Undead form of the regular cat. Traders_Menagerie_of_Tokens already adds this as a tradeable card
			"DefaultTail", "SkinkTail", "Tail_Bird", "Tail_Furry", "Tail_Insect", // Tail from the tail sigil
			"Mule", // The pack mule from the prospector boss. Since there is no normal way to get it the program thinks its not from Act 1
			"Rabbit", // Rabbits from the Warren. Since there is no normal way to get it the program thinks its not from Act 1
			"Bee", // Bees from the Beehive
			"Squirrel", // duh
			"Mothman_Stage2", "Mothman_Stage3", // Stages 2 and 3 of the mothman
			"!DEATHCARD_BASE", // Act 1 deathcards
			"!DEATHCARD_LESHY", // IDK, I think this is the card you see at the end of act 1
			"!DEATHCARD_VICTORY", // Deathcard that appears on the backroom door in act 1
			"!GIANTCARD_MOON", // The moon card seen in the final battle of act 1
			"BaitBucket", // Bait bucket used by the angler
			"CardMergeStones", // Cut card
			"Dam", // Used by the beaver
			"DausBell", // Used by, you guessed it, the daus
			"GoldNugget", // Used by the prospector
			"PeltGolden", "PeltHare", "PeltWolf", // Pelts
			"Smoke", "Smoke_Improved", "Smoke_NoBones", // Smoke cards you get during a boss
			"Trap", "TrapFrog", // Used by the trapper/trader
			"Starvation", // Played when the player has no cards. Pretty OP when in the players hand
			"!DEATHCARD_PIXEL_BASE", // IDK
			"Boulder",
			"FrozenOpossum",
			"Stump",
			"Tree",
			"Tree_Hologram",
			"Tree_SnowCovered"
		};

		internal static readonly string[] SideDeckCards = new string[] { 
			"EmptyVessel",
			"Skeleton"
		};

		// having a blacklist for this would be impossible because more mods are made all the time
		// so instead we use a whitelist
		internal static List<string> ModdedAct2Cards = new List<string>() {
			//AraAct2
			"MoxFox",
			"Colormagus",
			"Ghostviking",
			"submarine",
			"sniper mage",
			"wall",
			"waldroid",
			"cryptskeleton",
			"MrBomb",
			"Snail",
			"Bloodcultist",
			"Hyena",
			"antbot",
			"antbotqueen",
			"Phoenix"
		};
		internal static List<string> ModdedAct2Spells = new List<string>() {
			//AraAct2
			"BuffSpell",
			"MeteorSpell",
			"TransformSpell",
			"ColorSpell",
			"ChaosSpell"
		};

		internal static readonly string[] HoovedKeywords = new string[] { 
			"horse",
			"elk",
			"fawn",
			"donkey",
			"mule"
		};

		internal static readonly string[] SquirrelKeywords = new string[] {
			"squirrel"
		};

		internal static readonly string[] ReptileKeywords = new string[] {
			"frog",
			"lizard",
			"hrokkal",
			"mantis"
		};

		internal static readonly string[] InsectKeywords = new string[] {
			"bug",
			"insect",
			"ant",
			"worm"
		};

		internal static readonly string[] AirborneKeywords = new string[] {
			"drone",
			"bird",
			"raven",
			"hawk"
		};

		internal static readonly string[] CanineKeywords = new string[] {
			"hound"
		};

		internal static readonly List<CardMetaCategory> defaultMeta = new List<CardMetaCategory> { CardMetaCategory.ChoiceNode, CardMetaCategory.TraderOffer };

		internal static List<CardTemple> TemplesToAdd = new List<CardTemple>();

		internal static bool UseInternalNames = false;
		internal static bool UseBlankCards = false;
		internal static bool EmptyVesselConduit = true;
		internal static bool ApplyTribes = false;
		internal static bool DoTextureReplace = false;
		internal static bool DoSpells = true;
		internal static bool DoModded = true;

		private const Trait SideDeckTrait = (Trait)5103;
		private const string EmptyVessel = "EmptyVessel";

		public static MainPlugin instance;

		internal AssetManager assets;

		private static List<CardInfo> allValidCards;
		internal static List<CardInfo> AllValidCards { 
			get {
				if (allValidCards != null) return allValidCards;
				List<CardInfo> AllCards = new List<CardInfo>();
				AllCards.AddRange(CardLoader.AllData); // all vanilla cards
				{
					List<CardInfo> act2cards = GetAllModdedAct2Cards();
					if (act2cards != null) AllCards.AddRange(act2cards); // all modded cards
				}

				allValidCards =  AllCards.FindAll((card) => {
					bool InAct1 = card.temple == CardTemple.Nature && (card.metaCategories.Contains(CardMetaCategory.ChoiceNode) || card.metaCategories.Contains(CardMetaCategory.TraderOffer) || card.metaCategories.Contains(CardMetaCategory.Rare));
					if (InAct1) {
						logger.LogInfo($"{card.name} is already in act 1");
						return false; // dont do these other calculations
					}
					bool InBlacklist = Blacklist.Contains(card.name);
					bool IsValidTemple = TemplesToAdd.Contains(card.temple);
					bool IsSideDeck = SideDeckCards.Contains(card.name);
					return IsValidTemple && (!InBlacklist || IsSideDeck);
				});
				return allValidCards;
			} 
		}

		private static List<CardInfo> GetAllModdedAct2Cards() {
			var outp = new List<CardInfo>();
			var cards = ModdedAct2Cards;
			if (DoSpells) cards.AddRange(ModdedAct2Spells);
			foreach(var name in cards) {
				try {
					var card = NewCard.cards.Find((moddedCard) => moddedCard.name == name);
					if (card != null) {
						outp.Add(card);
					}
				} catch(NullReferenceException) {
					logger.LogWarning($"Null ref when trying to access card {name}");
					continue;
				}
			}
			return outp;
		}

		private void Awake() {
			logger = Logger;
			logger.LogMessage($"{Name} v{Version} Loaded!");

			assets = new AssetManager(Info);

			instance = this;

			GetConfig();
			EnableValidCards();
		}

		private void GetConfig() {
			UseBlankCards = Config.Bind("Blacklisting", "UseBlankCards", true, "Allow cards with no high-res portrait to be used in Act 1. The GBC portrait will be used instead. Remeber that none of the Wizard cards have a valid portrait.").Value;
			UseInternalNames = Config.Bind("Dev", "UseInternalNames", false, "Use the internal names of cards instead of their display name.").Value;
			DoTextureReplace = Config.Bind("Dev", "UseTextureReplace", false, "(EXPERIMENTAL) You can now specify custom textures for cards. To do so, create a 114x94 texture with the card's name as the file name and place it in the assets folder. For example, if I wanted to create a custom texture for the Steambot card I would create a 114x94 image and save it as \"Steambot.png\" in the assets folder.").Value;
			EmptyVesselConduit = Config.Bind("SideDecks", "EmptyVesselConduit", true, "Gives Empty Vessels the \"Null Conduit\" sigil.").Value;
			//ApplyTribes = Config.Bind("Tribes", "ApplyTribes", false, "(Not a finished feature) Gives most of the cards their own tribes.").Value;
			if (Config.Bind("Temples", "AddNatureTemple", true, "Add all cards from the Nature temple to Act 1.").Value    ) TemplesToAdd.Add(CardTemple.Nature);
			if (Config.Bind("Temples", "AddTechTemple",   true, "Add all cards from the Technology temple to Act 1.").Value) TemplesToAdd.Add(  CardTemple.Tech);
			if (Config.Bind("Temples", "AddUndeadTemple", true, "Add all cards from the Undead temple to Act 1.").Value    ) TemplesToAdd.Add(CardTemple.Undead);
			if (Config.Bind("Temples", "AddWizardTemple", true, "Add all cards from the Wizard temple to Act 1.").Value    ) TemplesToAdd.Add(CardTemple.Wizard);
			DoModded = Config.Bind("Misc", "DoModded", true, "Make modded cards usable as well.").Value;
			DoSpells = Config.Bind("Spells", "DoSpells", true, "Make modded spells usable as well.").Value;

			//TODO: Make ModdedAct2Cards configurable
			/*
			try {
				ModdedAct2Cards = ListFileManager.LoadList("ModdedAct2Cards", ModdedAct2Cards);
			} catch(UnauthorizedAccessException) {
				logger.LogWarning($"Could not access {nameof(ModdedAct2Cards)}.txt due to permission issues. Please set the permissions of the plugin folder propely.");
			}
			*/
		}

		private void EnableValidCards() {
			logger.LogMessage($"--------Begin making cards---------");
			int cardsMade = 0;
			List<CardInfo> allValidCards = AllValidCards;
			foreach (var card in allValidCards) { //CardLoader.AllData is readonly, which is good since we dont want to accidentally write to it
				EnableCard(card, SideDeckCards.Contains(card.name));
				cardsMade++;
			}
			logger.LogMessage($"----Successfully made {cardsMade} cards----");
		}

		private void EnableCard(CardInfo card, bool sideDeck) {
			if (!UseBlankCards && card.portraitTex == null) return;

			Texture2D defaultTex = File.Exists(assets.PathFor(card.name,"png")) ? assets.LoadPNG(card.name) :  card.GetPortrait(true, true);

			// this line prevents the texture from being blurry. It should already be set to this for GBC/normal cards, but not for textures on disk
			defaultTex.filterMode = FilterMode.Point;

			string cardName = card.name;
			logger.LogMessage($"Making card {cardName} {(sideDeck ? "a side deck choice" : "usable in act 1")}");

			var meta = card.metaCategories;
			var appear = card.appearanceBehaviour;
			var traits = card.traits;
			var evolvesInto = card.evolveParams;
			var melted = card.iceCubeParams;
			var sigils = card.DefaultAbilities;
			var staticon = card.SpecialStatIcon;

			if (!sideDeck && !meta.Contains(CardMetaCategory.Rare)) meta.AddRange(defaultMeta);
			if (meta.Contains(CardMetaCategory.Rare) && !appear.Contains(CardAppearanceBehaviour.Appearance.RareCardBackground)) appear.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);
			if (!sideDeck && appear.Contains(CardAppearanceBehaviour.Appearance.RareCardBackground) && !meta.Contains(CardMetaCategory.Rare)) meta.Add(CardMetaCategory.Rare);
			if (sideDeck) traits.Add(SideDeckTrait);
			if (cardName == EmptyVessel && EmptyVesselConduit) sigils.Add(Ability.ConduitNull);
			if (card.traits.Contains(Trait.Terrain)) appear.Add(CardAppearanceBehaviour.Appearance.TerrainBackground);

			if (meta.Contains(CardMetaCategory.Rare)) logger.LogDebug($"Card {cardName} is a rare");

			NewCard.Add(
				Name + "_" + cardName,
				displayedName: UseInternalNames ? cardName : card.DisplayedNameLocalized,
				baseAttack: card.Attack,
				baseHealth: card.Health,
				cardComplexity: card.cardComplexity,
				metaCategories: meta,
				temple: CardTemple.Nature,
				abilities: sigils,
				defaultTex: defaultTex,
				energyCost: card.EnergyCost,
				bonesCost: card.BonesCost,
				bloodCost: card.BloodCost,
				gemsCost: card.GemsCost,
				appearanceBehaviour: appear,
				evolveParams: evolvesInto,
				iceCubeParams: melted,
				specialStatIcon: staticon
			);

			logger.LogInfo(""); // newline for readability
		}
	}
}
