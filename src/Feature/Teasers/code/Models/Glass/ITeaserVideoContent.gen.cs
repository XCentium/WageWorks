﻿// <autogenerated>
//   This file was generated by T4 code generator Generate.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

namespace WageWorks.Feature.Teasers.Models.Glass
{
	using System;
    using System.Collections.Generic;
	using System.Collections.Specialized;
    using global::Glass.Mapper.Sc.Configuration;
    using global::Glass.Mapper.Sc.Configuration.Attributes;
	using global::Glass.Mapper.Sc.Fields;
    using Teasers;


	/// <summary>
	/// Represents a mapped type for item {04075EB6-6D94-4BF2-9AEB-D29A89CDBA00} in Sitecore.
	/// Path: /sitecore/templates/Feature/Teasers/_TeaserVideoContent
	/// </summary>
	[SitecoreType(TemplateId = "{04075EB6-6D94-4BF2-9AEB-D29A89CDBA00}")]
	public partial interface ITeaserVideoContent : ITeaserContent
	{
		#region Content

		/// <summary>
		/// ビデオリンク
		/// </summary>
	    [SitecoreField(FieldId = "{AC846A16-FD3F-4243-A21F-668A21010C44}")]
		Link VideoLink { get; set; }

		#endregion

	}
}
