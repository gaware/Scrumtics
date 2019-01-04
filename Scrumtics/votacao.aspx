<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="votacao.aspx.cs" Inherits="Scrumtics.votacao" %>

<%@ Register TagPrefix="uc" TagName="TopNavbar" Src="barra_usuario.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="pt-br">
<head runat="server">

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- As 3 meta tags acima *devem* vir em primeiro lugar dentro do `head`; qualquer outro conteúdo deve vir *após* essas tags -->
    <meta name="description" content="Scrumtics" />
    <meta name="author" content="Roberto Gauer" />
    <link rel="icon" href="favicon.ico" />
    <title>Scrumtics</title>

    <!-- Bootstrap -->
    <link href="bs/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="bs/dist/css/bootstrap-theme.min.css" rel="stylesheet" />

    <!-- Estilo personalizado para esta página -->
    <link href="estilos.css" rel="stylesheet" />

</head>
<body>

    <uc:TopNavbar runat="server" ID="TopNavbar1" />

    <div class="container theme-showcase" role="main">
        <div class="row form-signin">
            <div runat="server" id="Div1" class="col-md-6">
                <div class="form-signin-heading">
                    <h3>Votação <%=Votacao%></h3>
                </div>
                <form runat="server" id="Form1">
                    <input type="hidden" name="votacao" value="<%=Votacao%>" />
                    <asp:Button runat="server" ID="Button1" Text="1" OnClick="ButtonVotacao_Click" CssClass="btn btn-lg btn-primary btn-votacao" />
                    <asp:Button runat="server" ID="Button2" Text="2" OnClick="ButtonVotacao_Click" CssClass="btn btn-lg btn-info btn-votacao" />
                    <asp:Button runat="server" ID="Button4" Text="4" OnClick="ButtonVotacao_Click" CssClass="btn btn-lg btn-success btn-votacao" />
                    <asp:Button runat="server" ID="Button8" Text="8" OnClick="ButtonVotacao_Click" CssClass="btn btn-lg btn-default btn-votacao" />
                    <asp:Button runat="server" ID="Button12" Text="12" OnClick="ButtonVotacao_Click" CssClass="btn btn-lg btn-warning btn-votacao" />
                    <asp:Button runat="server" ID="Button16" Text="16" OnClick="ButtonVotacao_Click" CssClass="btn btn-lg btn-danger btn-votacao" />
                    <asp:Button runat="server" ID="Button20" Text="20" OnClick="ButtonVotacao_Click" CssClass="btn btn-lg btn-primary btn-votacao" />
                    <asp:Button runat="server" ID="Button24" Text="24" OnClick="ButtonVotacao_Click" CssClass="btn btn-lg btn-info btn-votacao" />
                    <asp:Button runat="server" ID="Button32" Text="32" OnClick="ButtonVotacao_Click" CssClass="btn btn-lg btn-success btn-votacao" />
                    <asp:Button runat="server" ID="Button40" Text="40" OnClick="ButtonVotacao_Click" CssClass="btn btn-lg btn-default btn-votacao" />
                    <asp:Button runat="server" ID="Button48" Text="48" OnClick="ButtonVotacao_Click" CssClass="btn btn-lg btn-warning btn-votacao" />
                    <asp:Button runat="server" ID="Button60" Text="60" OnClick="ButtonVotacao_Click" CssClass="btn btn-lg btn-danger btn-votacao" />
                    <asp:Button runat="server" ID="ButtonNone" Text="#" OnClick="ButtonVotacao_Click" CssClass="btn btn-lg btn-primary btn-votacao" />
                </form>
            </div>
            <div runat="server" id="Div2" class="col-md-6">
                <div class="form-signin-heading">
                    <h3>&nbsp;</h3>
                </div>
                <div style="height:300px">
                    <img src="http://chart.apis.google.com/chart?cht=qr&chs=300x300&chl=<%=Qrcode%>&chld=H|0" />
                </div>
                <div>
                    <h3><label class="label label-default"><%=Qrcode%></label></h3>
                </div>
            </div>
        </div>
    </div>

    <!-- jQuery (obrigatório para plugins JavaScript do Bootstrap) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <!-- Inclui todos os plugins compilados (abaixo), ou inclua arquivos separadados se necessário -->
    <script src="bs/dist/js/bootstrap.min.js"></script>

</body>
</html>
