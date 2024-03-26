using System;
using System.Text.RegularExpressions;

namespace CredECard.Common.BusinessService
{
	/// <summary>
	/// Summary description for SafeHtml.
	/// </summary>
	/// 
	public enum MailBodyFormat
	{
		PlainText  = 0,
		HtmlText ,
		MultiPart_OR_Alternative 
	}

	[Serializable()]
	public class SafeHtml
	{
		private string _unSafeHtml = string.Empty;
		private string _safeHtml = string.Empty;
		private string _textOnly = string.Empty;

        /// <summary>
        /// Initialises object with htmlText
        /// </summary>
        /// <param name="htmlText"></param>
		public SafeHtml(string htmlText) : this(htmlText, false)
		{
		}

        /// <summary>
        /// Initializes htmlText 
        /// </summary>
        /// <param name="htmlText">string</param>
        /// <param name="safe">bool</param>
		public SafeHtml(string htmlText, bool safe)
		{
			_unSafeHtml = htmlText;
			if (safe) _safeHtml = htmlText.Trim();
		}

        /// <summary>
        /// REmoves unsafe tags 
        /// </summary>
        /// <param name="source">string</param>
        /// <param name="tag">string</param>
        /// <param name="removeContent">bool</param>
        /// <returns>string</returns>
		public static string RemoveTag(string source, string tag, bool removeContent)
		{
			string tag1 = tag.Replace("*", "");
			string tag2 = tag.Replace("*", "[^>]*");
			string startTag = "<[\\s]*" + tag1 + "[^>]*>";
			string endTag = "</" + tag2 + ">";

			string result = string.Empty;
			if (removeContent)
			{
				string allRemove = startTag + "[^<]*" + endTag;
				result = Regex.Replace(source, allRemove, "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			}
			else
			{
				result = Regex.Replace(source, startTag, "", RegexOptions.IgnoreCase);
				result = Regex.Replace(result, endTag, "", RegexOptions.IgnoreCase);
			}
			
			return result;
		}

        /// <summary>
        /// Removes attributes
        /// </summary>
        /// <param name="source">string</param>
        /// <param name="attrib">string</param>
        /// <returns>string</returns>
		public static string RemoveAttrib(string source, string attrib)
		{
			string attribType1 = attrib.Replace("*", "[^\"]*");
			string attribType2 = attrib.Replace("*", "[^']*");
			string attribType3 = attrib.Replace("*", "[^]*");
			string expAttType1 = attribType1 + "[\\s]*\"[^\"]*\"";
			string expAttType2 = attribType2 + "[\\s]*'[^']*'";
			string expAttType3 = attribType3 + "[\\s]*[^]*";
			string result = Regex.Replace(source, expAttType1, "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			result = Regex.Replace(result, expAttType2, "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			return result;
		}
        
        /// <summary>
        /// Removes unsafe tags and attributes  
        /// </summary>
		private void MakeSafe()
		{
			string cleanDetails = _unSafeHtml;
			cleanDetails = SafeHtml.RemoveTag(cleanDetails, "script", true);
			cleanDetails = SafeHtml.RemoveTag(cleanDetails, "object", true);
			cleanDetails = SafeHtml.RemoveTag(cleanDetails, "frameset", false);
			cleanDetails = SafeHtml.RemoveTag(cleanDetails, "layer", true);
			cleanDetails = SafeHtml.RemoveTag(cleanDetails, "applet", true);
			cleanDetails = SafeHtml.RemoveTag(cleanDetails, "embed", true);
			cleanDetails = SafeHtml.RemoveTag(cleanDetails, "noscript", true);
			cleanDetails = SafeHtml.RemoveTag(cleanDetails, "noframes", true);
			//cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, " on*=");
			//code added by monal on 5thoct 2005 to avoid text truncation in the emails...
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "OnDataBinding*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "OnDisposed*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "OnInit*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "OnLoad*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "OnPreRender*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "OnUnload*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onAbort*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onBlur*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onChange*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onClick*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onDbClick*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onDragDrop*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onError*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onFocus*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onKeyDown*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onKeyPress*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onKeyUp*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onload*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onMouseDown*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onMouseOut*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onMouseOver*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onMouseUp*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onMousemove*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onMove*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onReset*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onResize*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onSelect*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onSubmit*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onfocus*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onblur*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "oncontextmenu*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onresize*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onscroll*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onmouseenter*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onmouseout*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onchange*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onsubtreemodified*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onSize*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "OnTextChanged*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "OnServerValidate*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onAfterUpdate*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onBeforeUnload*="); 
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onBeforeUpdate*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onBounce*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onDataAvailable*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onDataSetChanged*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onDataSetComplete*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onErrorUpdate*="); 
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onFilterChange*="); 
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onHelp*="); 
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onReadyStateChange*="); 
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onRowEnter*="); 
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onRowExit*="); 
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onSelectStart*="); 
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onStart*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onstop*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "onexecute*=");
			cleanDetails = SafeHtml.RemoveAttrib(cleanDetails, "OnSelectedIndexChanged*=");
			_safeHtml = cleanDetails.Trim();
		}

        /// <summary>
        /// Removes HTML tags 
        /// </summary>
		private void MakeText()
		{
			string cleanDetails = _unSafeHtml;
			if (this.IsHtml)
			{
				cleanDetails = cleanDetails.Replace("\r", "");
				cleanDetails = cleanDetails.Replace("\n", "");
				
				cleanDetails = cleanDetails.Replace("&nbsp;", " ");
				cleanDetails = cleanDetails.Replace("</P>", "\r\n");
				cleanDetails = cleanDetails.Replace("<br />", "\r\n");
				cleanDetails = cleanDetails.Replace("</TD>", "\t");
				cleanDetails = cleanDetails.Replace("</TR>", "\r\n");
				
				cleanDetails=SafeHtml.RemoveTag(cleanDetails, "*", false);
			}
			this._textOnly = cleanDetails.Trim();
		}

        /// <summary>
        /// Checks if the string content is HTML
        /// </summary>
		public bool IsHtml
		{
			get
			{
				return (Regex.IsMatch(_unSafeHtml, "<[\\s]*[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Multiline));
			}
		}

        /// <summary>
        /// returns string for the given HTML
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return this.ToString(HtmlOutputType.SafeHtml);
		}

        /// <summary>
        /// Creates string of required out put type
        /// </summary>
        /// <param name="htmlType">HtmlOutputType - RawHTML,TextOnly,SAfeHTL</param>
        /// <returns>string</returns>
		public string ToString(HtmlOutputType htmlType)
		{
			string outputHtml = string.Empty;
			switch (htmlType)
			{
				case HtmlOutputType.RawHtml:
					outputHtml = this._unSafeHtml;
					break;
				case HtmlOutputType.TextOnly:
					if (this._textOnly == string.Empty) this.MakeText();
					outputHtml = this._textOnly;
					break;
				case HtmlOutputType.SafeHtml:
				default:
					if (this._safeHtml == string.Empty) this.MakeSafe();
					outputHtml = this._safeHtml;
					break;
			}
			return outputHtml;
		}
	}

	public enum HtmlOutputType
	{
		SafeHtml = 0,
		TextOnly = 1,
		RawHtml = 2
	}
}