﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="equipe.aspx.cs" Inherits="Scrumtics.equipe" %>
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
            <h3>Equipe do pacote <%=Pacote%> - <%=Descricao%></h3> 
        </div>
        <div class="row">
            <div class="col-lg-12">
                <table class="table table-striped">
                <thead>
                    <tr>
                    <th>Usuário</th>
                    <th>Nome</th>
                    <th>E-mail</th>
                    <th>&nbsp;</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder runat="server" ID="PlaceHolder1" >
                    </asp:PlaceHolder>
                </tbody>
                </table>
                <form runat="server" id="Form1" class="form-signin">
                <table class="table table-striped">
                <thead>
                    <tr>
                    <th>Novo usuário</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                    <td>
                    <label for="NewLogin" class="control-label">Usuário</label>
                    <asp:TextBox runat="server" ID="NewLogin" CssClass="form-control" MaxLength="10" placeholder="usuario" required="required" autofocus="autofocus" style="text-transform:lowercase" />
                    <label for="NewName" class="control-label">Nome</label>
                    <asp:TextBox runat="server" ID="NewName" CssClass="form-control"  MaxLength="60" placeholder="Nome Completo" required="required" />                           
                    <label for="NewEmail" class="control-label">E-mail</label>
                    <asp:TextBox runat="server" ID="NewEmail" TextMode="Email" CssClass="form-control" MaxLength="120" placeholder="conta@servidor.com.br" required="required" />
                    <input type="hidden" name="pacote" value="<%=Pacote%>" />
                    <br />
                    <asp:Button runat="server" ID="ButtonConfirm1" Text="Criar usuário" OnClick="ButtonConfirm1_Click" CssClass="btn-link" />
                    </td>
                    </tr>
                </tbody>
                </table>
                </form>
            </div>
        </div>
        <div>
            <form id="Form2" class="navbar-form navbar-left" action="atividade.aspx" method="post">
                <input type="hidden" name="pacote" value="<%=Pacote%>" />
                <button id="ButtonConfirm2" type="submit" class="btn btn-lg btn-success">Atividades do pacote <%=Pacote%></button>
            </form>
        </div>
    </div>
    <div class="container theme-showcase" role="main">
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