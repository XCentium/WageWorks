﻿// <autogenerated>
//   This file was generated by T4 code generator Generate.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

namespace WageWorks.Feature.Navigation.Models.Glass
{
	using System;
    using System.Collections.Generic;
	using System.Collections.Specialized;
    using global::Glass.Mapper.Sc.Configuration;
    using global::Glass.Mapper.Sc.Configuration.Attributes;
	using global::Glass.Mapper.Sc.Fields;
    

	/// <summary>
	/// Represents a mapped type for item {A16B74E9-01B8-439C-B44E-42B3FB2EE14B} in Sitecore.
	/// Path: /sitecore/templates/Feature/Navigation/_Link
	/// </summary>
	[SitecoreType(TemplateId = "{A16B74E9-01B8-439C-B44E-42B3FB2EE14B}")]
	public partial interface ILink
	{
        [SitecoreId]
        Guid Id { get; set; }
		#region Navigation
		/// <summary>
		/// リンク
		/// </summary>
	    [SitecoreField(FieldId = "{FE71C30E-F07D-4052-8594-C3028CD76E1F}")]
		Link Link { get; set; }

		#endregion

	}
}
