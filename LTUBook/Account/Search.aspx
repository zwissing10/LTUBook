<%@ Page Title="Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="LTUBook.Account.Search" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>
    <div>
        <div class="form-horizontal">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="SearchBox" CssClass="col-xl-4 control-label">Search for someone...</asp:Label>
                <div class="col-xl-10">
                    <asp:TextBox runat="server" ID="SearchBox" TextMode="SingleLine" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="SearchBox" CssClass="text-danger" ErrorMessage="Enter a name to search for" />
                </div>
            </div>
            <asp:Button runat="server" Text="Search" CssClass="btn btn-default" />
        </div>
    </div>
    <br />
    <div class="col-md-12">
        <asp:Table runat="server" ID="SearchTable" CssClass="table table-hover">
        </asp:Table>
    </div>
</asp:Content>