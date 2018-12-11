<%@ Language=C# %>
<%@ Import Namespace="Sitecore.Diagnostics" %>
<%@ Import Namespace="Sitecore.Reflection" %>
<%@ Import Namespace="Sitecore.Web" %>
<%@ Import Namespace="Sitecore.Caching" %>
<%@ Import Namespace="Sitecore" %>

<!DOCTYPE html>
<HTML>
<script runat="server" language="C#">


    /// <summary>
    /// Handles the Click event of the 'Clear All' button.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="arguments">
    /// The <see cref="T:System.EventArgs" /> instance containing the event data.
    /// </param>
    private void ClearAllButtonClick(object sender, EventArgs arguments)
    {
        Assert.ArgumentNotNull(sender, "sender");
        Assert.ArgumentNotNull(arguments, "arguments");
        ICacheInfo[] allCaches = CacheManager.GetAllCaches();
        for (int i = 0; i < (int) allCaches.Length; i++)
        {
            allCaches[i].Clear();
        }
        TypeUtil.ClearSizeCache();
        this.ResetCacheList();
    }

    /// <summary>
    /// Initializes the component.
    /// Required method for Designer support -
    /// do not modify the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.c_refresh.Click += new EventHandler(this.RefreshButtonClick);
        this.c_clearAll.Click += new EventHandler(this.ClearAllButtonClick);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event to initialize the page.
    /// </summary>
    /// <param name="arguments">
    /// An <see cref="T:System.EventArgs" /> that contains the event data.
    /// </param>
    protected override void OnInit(EventArgs arguments)
    {
        Assert.ArgumentNotNull(arguments, "arguments");
        this.InitializeComponent();
        base.OnInit(arguments);
    }

    /// <summary>
    /// Handles the Click event of the 'Refresh' button.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="arguments">
    /// The <see cref="T:System.EventArgs" /> instance containing the event data.
    /// </param>
    private void RefreshButtonClick(object sender, EventArgs arguments)
    {
        Assert.ArgumentNotNull(sender, "sender");
        Assert.ArgumentNotNull(arguments, "arguments");
        this.UpdateTotals();
        this.ResetCacheList();
    }

    /// <summary>
    /// Resets the cache list.
    /// </summary>
    private void ResetCacheList()
    {
        ICacheInfo[] allCaches = CacheManager.GetAllCaches();
        Array.Sort(allCaches, new CacheComparer());
        HtmlTable htmlTable = HtmlUtil.CreateTable(0, 0);
        htmlTable.Border = 1;
        htmlTable.CellPadding = 4;
        HtmlUtil.AddRow(htmlTable, new string[] {string.Empty, "Name", "Count", "Size", "Delta", "MaxSize"});
        ICacheInfo[] cacheInfoArray = allCaches;
        for (int i = 0; i < (int) cacheInfoArray.Length; i++)
        {
            ICacheInfo cacheInfo = cacheInfoArray[i];
            string str = string.Concat("size_", cacheInfo.Id.ToShortID());
            long num = MainUtil.GetLong(base.Request.Form[str], (long) 0);
            long count = (long) cacheInfo.Count;
            long size = cacheInfo.Size;
            long maxSize = cacheInfo.MaxSize;
            long num1 = size - num;
            HtmlTableRow htmlTableRow = HtmlUtil.AddRow(htmlTable, new string[] {string.Empty, cacheInfo.Name, count.ToString(), MainUtil.FormatSize(size, false), MainUtil.FormatSize(num1, false), MainUtil.FormatSize(maxSize, false)});
            for (int j = 2; j < htmlTableRow.Cells.Count; j++)
            {
                htmlTableRow.Cells[j].Align = "right";
            }
            htmlTableRow.Cells[htmlTableRow.Cells.Count - 2].Style["color"] = "red";
            htmlTableRow.Cells[htmlTableRow.Cells.Count - 1].Style["color"] = "lightgrey";
            HtmlInputHidden htmlInputHidden = new HtmlInputHidden()
            {
                ID = str,
                Value = size.ToString()
            };
            htmlTableRow.Cells[0].Controls.Add(htmlInputHidden);
        }
        if (this.c_caches.Controls.Count > 0)
        {
            this.c_caches.Controls.RemoveAt(0);
        }
        this.c_caches.Controls.Add(htmlTable);
        this.c_cacheTitle.Text = string.Format("Caches ({0})", (int) allCaches.Length);
    }

    /// <summary>
    /// Updates the totals.
    /// </summary>
    private void UpdateTotals()
    {
        CacheStatistics statistics = CacheManager.GetStatistics();
        this.c_totals.Text = string.Format("Entries: {0}, Size: {1}", statistics.TotalCount, statistics.TotalSize);
    }


</script>
<HEAD>
    <title>Cache Admin</title>
    <link rel="shortcut icon" href="/sitecore/images/favicon.ico" />
    <meta content=C# name=CODE_LANGUAGE>
    <meta content=JavaScript name=vs_defaultClientScript>
    <meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema>
</HEAD>
<body>
<form id="Form1" method=post runat="server">
    <TABLE id=Table1 style="WIDTH: 594px; HEIGHT: 154px" cellSpacing=1 cellPadding=1 
           width=594 border=1>
        <TR>
            <TD><asp:label id="Caches" runat="server" DESIGNTIMEDRAGDROP="79">Actions</asp:label></TD>
            <TD></TD>
            <TD><asp:button id="c_refresh" runat="server" Text="Refresh"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;<asp:button id="c_clearAll" runat="server" Text="Clear all"></asp:button></TD></TR>
        <TR>
            <TD><asp:label id="Label1" runat="server">Totals</asp:label></TD>
            <TD></TD>
            <TD><asp:label id="c_totals" runat="server">[totals]</asp:label></TD></TR>
        <TR>
            <TD style="HEIGHT: 36px"></TD>
            <TD style="HEIGHT: 36px"></TD>
            <TD style="HEIGHT: 36px"></TD></TR>
        <TR>
            <TD vAlign="top"><asp:label id="c_cacheTitle" runat="server">Caches</asp:label></TD>
            <TD></TD>
            <TD><asp:PlaceHolder id="c_caches" runat="server"></asp:PlaceHolder></TD></TR></TABLE></FORM>
</body>
</HTML>
