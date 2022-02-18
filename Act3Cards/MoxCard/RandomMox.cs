using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using TDLib.Collections;
using UnityEngine;

namespace Act3Cards.MoxCard {
	public class RandomMox : AbilityBehaviour {

		public static readonly List<GemType> defaultSideDeck = new List<GemType>() {
			GemType.Blue,
			GemType.Blue,
			GemType.Blue,
			GemType.Orange,
			GemType.Orange,
			GemType.Orange,
			GemType.Green,
			GemType.Green,
			GemType.Green,
			MainPlugin.RandomGem
		};
		public static List<GemType> currentSideDeck = null;

		public override Ability Ability => ability;
		public static Ability ability;
		//public static AbilityIdentifier id;

		public static void Init() {
			AbilityInfo info = AbilityManager.New(MainPlugin.GUID,"Random Mox", "When [creature] is drawn, it is replaced with a random Mox card.", typeof(RandomMox), new Texture2D(49, 49));
			info.canStack = false;
			info.opponentUsable = false;
			ability = info.ability;
		}

		public override bool RespondsToDrawn() => true;

		public override IEnumerator OnDrawn() {
			if (currentSideDeck == null) currentSideDeck = (List<GemType>)defaultSideDeck.Shuffle(SaveManager.SaveFile.GetCurrentRandomSeed());
			GemType chosenGem = currentSideDeck[0];
			currentSideDeck.Remove(chosenGem);

			Card.SetInfo(CardLoader.GetCardByName(chosenGem.MoxID("Act3Cards_", SaveManager.SaveFile.GetCurrentRandomSeed())));
			yield return null;
		}

	}
}
