<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LTUBook._Default" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:LoginView runat="server" ViewStateMode="Disabled">
        <AnonymousTemplate>
            <div class="jumbotron">
                <h1><b>LTUBook</b></h1>
                <p class="lead">Welcome to <b>LTUBook</b>! Please Login or Create an Account by clicking one of the options in the top right or by selecting one of the buttons below.</p>
                <p>
                    <a runat="server" href="~/Account/Login" class="btn btn-primary btn-lg">Login &raquo;</a>
                    <a runat="server" href="~/Account/Register" class="btn btn-primary btn-lg">Register &raquo;</a>
                </p>
            </div>
        </AnonymousTemplate>
        <LoggedInTemplate>
            <div class="jumbotron text-align-center">
                <h1><b>LTUBook</b></h1>
            </div>
            <div class="col-md-6">
                <a runat="server" href="~/Notifications" class="btn btn-primary btn-lg">View All Notifications</a>
            </div>
            <div class="col-md-6">
                <a runat="server" href="~/UserSearch" class="btn btn-primary btn-lg">Search</a>
            </div>
            </br>
            <div class="col-md-12">
                <a runat="server" href="~/Account/Manage" class="btn btn-primary btn-lg">Manage Account Settings</a>
            </div>
        </LoggedInTemplate>
    </asp:LoginView>
</asp:Content>