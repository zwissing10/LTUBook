<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="LTUBook.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Contact Us</h3>
    <address>
        21000 W Ten Mile Rd<br />
        Southfield, MI 48075<br />
        <!--<abbr title="Phone">P:</abbr>
        111-222-3333-->
    </address>

    <address>
        <strong>Questions?</strong>   <a href="mailto:zwissing@ltu.edu">zwissing@ltu.edu</a><br />
                                      <a href="mailto:cwithun@ltu.edu">cwithun@ltu.edu</a><br />
    </address>
</asp:Content>
