<%@ Page Title="Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="LTUBook.Account.Search" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>
    <div>
        <div runat="server" class="form-group">
            <asp:Label runat="server" AssociatedControlID="SearchBox" CssClass="col-md-6">Search for someone...</asp:Label>
            <asp:TextBox runat="server" ID="SearchBox" TextMode="SingleLine" CssClass="col-md-6 form-control" />
        </div>
        <asp:Button runat="server" OnClick="userSearch" Text="Search" CssClass="btn btn-default" />
    </div>
    <div class="col-md-12">
        <asp:Table runat="server" ID="SearchTable" CssClass="table table-hover">
        </asp:Table>
    </div>
</asp:Content>