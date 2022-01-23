using System;
using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using UnityEngine;

namespace Act3Cards {
	public static class CardInfoExtensions {

		/// <summary>
		/// Returns this card's high-res portrait. If none exists (and GBCFallback is true) it will use an upscaled version of the card's GBC portrait.
		/// </summary>
		/// <param name="card">The card in question.</param>
		/// <param name="GBCFallback">If this is true, if a card has no high-res portrait it will return an upscaled version of the card's GBC portrait.</param>
		/// <param name="allWizardsAreGBC">Treat all Wizard cards as if they have no high-res portrait.</param>
		internal static Texture2D GetPortrait(this CardInfo card, bool GBCFallback = true, bool allWizardsAreGBC = true) {
			if (card.portraitTex == null || (card.temple == CardTemple.Wizard && allWizardsAreGBC)) {
				//Card is native to Act 2 or is a wizard card, so we will use the GBC portrait
				return GBCFallback ? GetUpscaledGBCPortrait(card) : null;
			} else return card.portraitTex.texture;
		}

		/// <summary>
		/// Returns a card's GBC portrait, upscaled to 114x94. If no such portrait exists it will return null.
		/// </summary>
		/// <param name="card">The card in question.</param>
		private static Texture2D GetUpscaledGBCPortrait(this CardInfo card) {
			//For some reason GBC portraits are unreadable, and since we do not have
			//access to the game assets to change that we will need to do some sketchy stuff to read the pixels
			//Credit to https://support.unity.com/hc/en-us/articles/206486626-How-can-I-get-pixels-from-unreadable-textures-

			MainPlugin.logger.LogDebug($"Card {card.name} has no high-res portrait");

			if (card.pixelPortrait == null) {
				MainPlugin.logger.LogDebug($"Card {card.name} has no GBC portrait");
				return null;
			}

			Texture2D texture = card.pixelPortrait.texture;

			// Create a temporary RenderTexture of the same size as the texture
			RenderTexture tmp = RenderTexture.GetTemporary(
								114,
								94,
								0,
								RenderTextureFormat.Default,
								RenderTextureReadWrite.Linear);


			// Blit the pixels on texture to the RenderTexture
			Graphics.Blit(texture, tmp);


			// Backup the currently set RenderTexture
			RenderTexture previous = RenderTexture.active;


			// Set the current RenderTexture to the temporary one we created
			RenderTexture.active = tmp;


			// Create a new readable Texture2D to copy the pixels to it
			Texture2D myTexture2D = new Texture2D(114, 94);


			// Copy the pixels from the RenderTexture to the new Texture
			myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
			myTexture2D.Apply();


			// Reset the active RenderTexture
			RenderTexture.active = previous;


			// Release the temporary RenderTexture
			RenderTexture.ReleaseTemporary(tmp);


			// "myTexture2D" now has the same pixels from "texture"
			return myTexture2D;
		}
	}
}
