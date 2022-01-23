using System;
using System.Collections.Generic;
using System.Text;

namespace Act3Cards {
	public static class StringExtensions {

		/// <summary>
		/// Turns a list of keywords into a regex.
		/// </summary>
		/// <param name="matches">The keywords to match for.</param>
		/// <returns></returns>
		public static StringBuilder AsRegex(this string[] matches) {
			StringBuilder outp = new StringBuilder();
			foreach (var match in matches) {
				outp.Append("(");
				outp.Append(match.ToUpper());
				outp.Append(")|");
			}
			outp.Remove(outp.Length - 1, 1);
			return outp;
		}

	}
}
