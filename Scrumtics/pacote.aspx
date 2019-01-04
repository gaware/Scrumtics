<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pacote.aspx.cs" Inherits="Scrumtics.pacote" %>
<%@ Register TagPrefix="uc" TagName="TopNavbar" Src="barra_usuario.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="pt-br">
<head runat="server">

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- As 3 meta tags acima *devem* vir em primeiro lugar dentro do `head`; qualquer outro conte�do deve vir *ap�s* essas tags -->
    <meta name="description" content="Scrumtics" />
    <meta name="author" content="Roberto Gauer" />
    <link rel="icon" href="favicon.ico" />
    <title>Scrumtics</title>

    <!-- Bootstrap -->
    <link href="bs/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="bs/dist/css/bootstrap-theme.min.css" rel="stylesheet" />

    <!-- Estilo personalizado para esta p�gina -->
    <link href="estilos.css" rel="stylesheet" />

</head>
<body>

    <uc:TopNavbar runat="server" ID="TopNavbar1" />

    <div class="container theme-showcase" role="main">
        <form runat="server" id="Form1" class="form-signin">
            <div class="form-signin-heading">
                <h3>Cadastro de pacote</h3>
            </div>
            <label for="NewDescription" class="control-label">Descri��o</label>
            <asp:TextBox runat="server" ID="NewDescription" TextMode="multiline" Rows="5" CssClass="form-control" MaxLenght="500" placeholder="Descri��o" required="required" autofocus="autofocus" />
            <br />
            <asp:Button runat="server" ID="ButtonConfirm1" Text="Criar pacote" OnClick="ButtonConfirm1_Click" CssClass="btn btn-lg btn-success" />
        </form>
        <div class="form-signin">
            <p><%=Mensagem%></p>
        </div>
    </div> 
    
    <!-- jQuery (obrigat�rio para plugins JavaScript do Bootstrap) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <!-- Inclui todos os plugins compilados (abaixo), ou inclua arquivos separadados se necess�rio -->
    <script src="bs/dist/js/bootstrap.min.js"></script>

</body>
</html>