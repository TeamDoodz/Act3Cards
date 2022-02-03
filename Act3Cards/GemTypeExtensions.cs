using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiskCardGame;
using TDLib.Collections;

namespace Act3Cards {
	public static class GemTypeExtensions {

		private static readonly Dictionary<GemType, string> ids = new Dictionary<GemType, string>() {
			{GemType.Blue, "MoxSapphire" },
			{GemType.Green, "MoxEmerald" },
			{GemType.Orange, "MoxRuby" }
		};

		public static string MoxID(this GemType self, string prefix = "", int seed = 0) {
			if (seed == 0) seed = SaveManager.SaveFile.GetCurrentRandomSeed();
			string id;
			if (self == MainPlugin.RandomGem) id = ids.Values.ToList().GetRandom(seed);
			else id = ids[self];
			return prefix + id;
		}

	}
}
