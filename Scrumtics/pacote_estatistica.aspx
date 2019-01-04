<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pacote_estatistica.aspx.cs" Inherits="Scrumtics.pacote_estatistica" %>
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
        <div class="page-header">
            <h3>Estatísticas do pacote <%=Pacote%> - <%=Descricao%></h3> 
        </div>
        <div class="page-header">
            <h2>Avaliação por tempo previsto</h2> 
        </div>
        <div class="row">
            <div class="col-lg-12">
                <table class="table table-striped">
                <thead>
                    <tr>
                    <th>Atividade / Tarefa</th>
                    <th>Usuário</th>
                    <th>Votação / Diferença</th>
                    <th>Previsto</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder runat="server" ID="PlaceHolder1" >
                    </asp:PlaceHolder>
                </tbody>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <table class="table table-striped">
                <thead>
                    <tr>
                    <th>Usuário</th>
                    <th>Quantidade de atividades/tarefas em melhor posição por tempo previsto</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder runat="server" ID="PlaceHolder2" >
                    </asp:PlaceHolder>
                </tbody>
                </table>
            </div>
        </div>
        <div class="page-header">
            <h2>Avaliação por tempo realizado</h2> 
        </div>
        <div class="row">
            <div class="col-lg-12">
                <table class="table table-striped">
                <thead>
                    <tr>
                    <th>Atividade / Tarefa</th>
                    <th>Usuário</th>
                    <th>Votação / Diferença</th>
                    <th>Realizado</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder runat="server" ID="PlaceHolder3" >
                    </asp:PlaceHolder>
                </tbody>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <table class="table table-striped">
                <thead>
                    <tr>
                    <th>Usuário</th>
                    <th>Quantidade de atividades/tarefas em melhor posição por tempo realizado</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder runat="server" ID="PlaceHolder4" >
                    </asp:PlaceHolder>
                </tbody>
                </table>
            </div>
        </div>
        <div>
            <form id="Form2" class="navbar-form navbar-left" action="atividade.aspx" method="post">
                <input type="hidden" name="pacote" value="<%=Pacote%>" />
                <button id="ButtonConfirm2" type="submit" class="btn btn-lg btn-success">Atividades do pacote <%=Pacote%></button>
            </form>
        </div>
    </div>

    <!-- jQuery (obrigatório para plugins JavaScript do Bootstrap) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <!-- Inclui todos os plugins compilados (abaixo), ou inclua arquivos separadados se necessário -->
    <script src="bs/dist/js/bootstrap.min.js"></script>

</body>
</html>