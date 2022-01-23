using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Act3Cards {
	public static class ListFileManager {

		public static string ConfigDir {
			get {
				return BepInEx.Paths.ConfigPath;
				//return MainPlugin.instance.Info.Location.Replace($"{MainPlugin.Name}.dll", "");
				//return "./";
			}
		}

		public static void SetList(string name, List<string> values) {
			string path = Path.Combine(ConfigDir,name + ".txt");
			MainPlugin.logger.LogMessage($"Writing to {path}");

			if (File.Exists(path)) File.Delete(path);
			using (var sw = File.CreateText(ConfigDir)) {
				foreach(string val in values) {
					sw.WriteLine(val);
				}
			}
		}

		public static List<string> LoadList(string name, List<string> defaults) {
			string path = Path.Combine(ConfigDir, name + ".txt");
			MainPlugin.logger.LogMessage($"Reading from {path}");

			if (!File.Exists(path)) {
				MainPlugin.logger.LogInfo("File does not exist");
				SetList(name, defaults);
			}

			string[] values = File.ReadAllLines(path);
			return values.ToList();
		}

	}
}
