<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="usuario.aspx.cs" Inherits="Scrumtics.usuario" %>
<%@ Register TagPrefix="uc" TagName="TopNavbarIni" Src="barra_inicio.ascx" %>
<%@ Register TagPrefix="uc" TagName="TopNavbarUsu" Src="barra_usuario.ascx" %>

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

    <uc:TopNavbarIni runat="server" ID="TopNavbarIni1" />
    <uc:TopNavbarUsu runat="server" ID="TopNavbarUsu1" Visible="false" />

    <div class="container theme-showcase" role="main">
        <form runat="server" id="Form1" class="form-signin">
            <div class="form-signin-heading">
                <h3>Cadastro de usuário</h3>
            </div>
            <label for="NewLogin" class="control-label">Usuário</label>
            <asp:TextBox runat="server" ID="NewLogin" CssClass="form-control" MaxLength="10" placeholder="usuario" required="required" autofocus="autofocus" style="text-transform:lowercase" />
            <label for="NewPassword" class="control-label">Senha</label>
            <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" CssClass="form-control" MaxLength="30" placeholder="********" required="required" />
            <label for="NewName" class="control-label">Nome</label>
            <asp:TextBox runat="server" ID="NewName" CssClass="form-control" MaxLength="60" placeholder="Nome Completo" required="required" />
            <label for="NewEmail" class="control-label">E-mail</label>
            <asp:TextBox runat="server" ID="NewEmail" TextMode="Email" CssClass="form-control" MaxLength="120" placeholder="conta@servidor.com.br" required="required" />
            <label for="NewImage" class="control-label">Imagem (PNG/JPG)</label>
            <asp:FileUpload runat="server" ID="NewImage" CssClass="btn btn-lg btn-default btn-block" UseSubmitBehavior="false" />
            <br /> 
            <asp:Button runat="server" ID="ButtonConfirm1" Text="Criar usuário" OnClick="ButtonConfirm1_Click" CssClass="btn btn-lg btn-success" />
        </form>
        <div class="form-signin">
            <p><%=Mensagem%></p>
        </div>
    </div> 

    <!-- jQuery (obrigatório para plugins JavaScript do Bootstrap) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <!-- Inclui todos os plugins compilados (abaixo), ou inclua arquivos separadados se necessário -->
    <script src="bs/dist/js/bootstrap.min.js"></script>

</body>
</html>