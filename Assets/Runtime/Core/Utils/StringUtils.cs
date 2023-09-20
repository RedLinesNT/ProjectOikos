using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oikos.Core {

	public static class StringUtils {

		/// <summary>
		/// Will return the string given but with the first letter set as uppercase.
		/// </summary>
		/// <param name="_stringContent">The string to use</param>
		public static string UppercaseFirstLetter(this string _stringContent) {
			if (string.IsNullOrEmpty(_stringContent)) return string.Empty; //Return a empty string if null or already empty

			char[] _chars = _stringContent.ToCharArray(); //Get the string given to a char array
			_chars[0] = char.ToUpper(_chars[0]); //Uppercase the first letter

			return new string(_chars);  //Then return the result
		}
		
		/// <summary>
		/// Will return the string given but with the first letter set as lowercase.
		/// </summary>
		/// <param name="_stringContent">The string to use</param>
		public static string LowercaseFirstLetter(this string _stringContent) {
			if (string.IsNullOrEmpty(_stringContent)) return string.Empty; //Return a empty string if null or already empty

			char[] _chars = _stringContent.ToCharArray(); //Get the string given to a char array
			_chars[0] = char.ToLower(_chars[0]); //Lowercase the first letter

			return new string(_chars);  //Then return the result
		}

	}
    
}