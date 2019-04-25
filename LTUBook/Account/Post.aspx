<%@ Page Title="Post" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Post.aspx.cs" Inherits="LTUBook.Account.Post" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %></h2>
    <div class="form-horizontal">
        <div class="form-group">
            <asp:Label runat="server" AssociatedControllerID="PostContent" CssClass="col-xl-4 control-label">Share Something</asp:Label>
            <div class="col-xl-10">
                <asp:TextBox runat="server" ID="PostContent" TextMode="MultiLine" Rows="6" CssClass="form-control"/>
            </div>
        </div>
        <asp:Button runat="server" OnClick="post" Text="Post" CssClass="btn btn-default" />
    </div>
</asp:Content>