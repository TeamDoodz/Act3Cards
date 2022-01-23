using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BepInEx;
using UnityEngine;

namespace Act3Cards {
    public class AssetManager {

        public AssetManager(PluginInfo plugin) {
            string path = plugin.Location.Replace($"{plugin.GetType().Name}.dll", "");
            MainPlugin.logger.LogDebug($"Root directory for mod {plugin.GetType().Name} is {path}");
            RootDir = path;
        }

        public AssetManager(string RootDir) {
            this.RootDir = RootDir;
        }

        public string RootDir = "";

        public string PathFor(string name, string extension) {
            return Path.Combine(RootDir, "assets", name) + $".{extension}";
        }

        /// <summary>
        /// Loads a PNG file from disk. <seealso href="https://answers.unity.com/questions/432655/loading-texture-file-from-pngjpg-file-on-disk.html"/>
        /// </summary>
        /// <param name="name">The name of the image, not including the path and the extension.</param>
        /// <returns>The image, as a Texture2D.</returns>
        public Texture2D LoadPNG(string name) {
            string path = PathFor(name, "png");
            MainPlugin.logger.LogInfo($"Loading texture {name} ({path})");

            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(path)) {
                fileData = File.ReadAllBytes(path);
                tex = new Texture2D(2, 2, TextureFormat.BGRA32, false);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                tex.filterMode = FilterMode.Point;
            } else {
                MainPlugin.logger.LogWarning($"Texture {name} ({path}) does not exist!");
            }
            return tex;
        }

    }

}
