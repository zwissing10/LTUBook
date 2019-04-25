<%@ Page Title="UserPage" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Page.aspx.cs" Inherits="LTUBook.Account.Page" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>
    <div class="jumbotron">
        <asp:Label runat="server" ID="userLabel" />
    </div>
    <div class="col-md-12">
        <asp:Table runat="server" ID="NotifTable" CssClass="table table-bordered">
        </asp:Table>
    </div>
</asp:Content>