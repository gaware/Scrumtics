<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sobre.aspx.cs" Inherits="Scrumtics.sobre" %>
<%@ Register TagPrefix="uc" TagName="TopNavbar" Src="barra_inicio.ascx" %>

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
        <div class="jumbotron">
            <h1>Sobre</h1>
            <p>Projeto tecnológico em sistemas para internet</p>
            <p>Autor: Roberto Gauer</p>
            <p>E-mail: <a href="mailto:gawarez@gmail.com">gawarez@gmail.com</a></p>
            <p>Servidor: <%=Servidor%></p>
        </div>
    </div>
    
    <!-- jQuery (obrigatório para plugins JavaScript do Bootstrap) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <!-- Inclui todos os plugins compilados (abaixo), ou inclua arquivos separadados se necessário -->
    <script src="bs/dist/js/bootstrap.min.js"></script>

</body>
</html>