using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using APIPlugin;
using DiskCardGame;
using TDLib.Collections;
using UnityEngine;

namespace Act3Cards.MoxCard {
	public class RandomMox : AbilityBehaviour {

		public override Ability Ability => ability;
		public static Ability ability;
		public static AbilityIdentifier id;

		public static readonly string[] Moxes = new string[] { 
			"Act3Cards_MoxEmerald",
			"Act3Cards_MoxSapphire",
			"Act3Cards_MoxRuby",
		};

		public static void Init() {
			AbilityInfo info = AbilityInfoUtils.CreateInfoWithDefaultSettings("Random Mox", "When [creature] is drawn, it is replaced with a random Mox card.");
			info.canStack = true;
			info.opponentUsable = false;
			id = AbilityIdentifier.GetAbilityIdentifier(MainPlugin.GUID, nameof(RandomMox));
			NewAbility a = new NewAbility(info, typeof(RandomMox), new Texture2D(49,49), id: id);
			ability = a.ability;
		}

		public override bool RespondsToDrawn() => true;

		public override IEnumerator OnDrawn() {
			Card.ClearAppearanceBehaviours();
			Card.RenderCard();
			Card.SetInfo(CardLoader.GetCardByName(Moxes.GetRandom(SaveManager.SaveFile.GetCurrentRandomSeed())));
			SaveManager.SaveFile.randomSeed++;
			yield return null;
		}

	}
}
