using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using Object = UnityEngine.Object;

namespace Broccoli.Base
{
	/// <summary>
	/// Broccoli extension information holder.
	/// </summary>
	public static class BroccoliExtensionInfo
	{
		#region Vars
		/// <summary>
		/// The name of the extension.
		/// </summary>
		public static string extensionName = "Broccoli Tree Creator";
		/// <summary>
		/// The major version.
		/// </summary>
		public static string majorVersion = "1";
		/// <summary>
		/// The minor version.
		/// </summary>
		public static string minorVersion = "5";
		/// <summary>
		/// The patch version.
		/// </summary>
		public static string patchVersion = "0";
		#endregion

		#region Methods
		public static string GetVersion () {
			return majorVersion + "." + minorVersion + "." + patchVersion;
		}
		#endregion
	}
}