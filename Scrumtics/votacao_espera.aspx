<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="votacao_espera.aspx.cs" Inherits="Scrumtics.votacao_espera" %>

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
        <form runat="server" id="Form1" class="form-signin" action="votacao_espera.aspx" method="post">
            <div class="form-signin-heading">
                <h3>Aguarde, esperando uma nova votação...</h3>
            </div>
            <div style="height:300px">
                <img src="http://chart.apis.google.com/chart?cht=qr&chs=300x300&chl=<%=Qrcode%>&chld=H|0" />
            </div>
            <div runat="server" id="Div1">
                <h3><label class="label label-default"><%=Qrcode%></label></h3>
            </div>
            <input type="hidden" name="votacao" value="<%=Votacao%>" />
            <br />
            <div class="progress">
                <div class="progress-bar progress-bar-striped" role="progressbar" aria-valuenow="<%=Espera1%>" aria-valuemin="0" aria-valuemax="60" style="<%=Espera2%>">
                </div>
            </div>
            <asp:Timer runat="server" ID="Timer1" Interval="5000"></asp:Timer>
            <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
        </form>
    </div>

    <!-- jQuery (obrigatório para plugins JavaScript do Bootstrap) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <!-- Inclui todos os plugins compilados (abaixo), ou inclua arquivos separadados se necessário -->
    <script src="bs/dist/js/bootstrap.min.js"></script>

</body>
</html>
