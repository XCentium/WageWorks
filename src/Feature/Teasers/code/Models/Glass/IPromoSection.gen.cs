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
    

	/// <summary>
	/// Represents a mapped type for item {9574A71F-AF6D-4B02-A30A-6184110F88E2} in Sitecore.
	/// Path: /sitecore/templates/Feature/Teasers/_PromoSection
	/// </summary>
	[SitecoreType(TemplateId = "{9574A71F-AF6D-4B02-A30A-6184110F88E2}")]
	public partial interface IPromoSection
	{
		#region Content

	    [SitecoreField(FieldId = "{8C69C046-E4D2-43CB-B9FC-B8D630B853B0}")]
		Image BackgroundImage { get; set; }

	    [SitecoreField(FieldId = "{75994DFA-60A1-48B8-B705-27A12C96937E}")]
		string Description { get; set; }

	    [SitecoreField(FieldId = "{C9480841-E01C-441D-81CF-1EF0AA3EB963}")]
		Link Link { get; set; }

	    [SitecoreField(FieldId = "{B2B3422B-8FEB-4225-8465-D602463E91BF}")]
		string Name { get; set; }

		#endregion

	}
}
