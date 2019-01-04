<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="atividade.aspx.cs" Inherits="Scrumtics.atividade" %>
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
            <h3>Atividades do pacote <%=Pacote%> - <%=Descricao%></h3>
        </div>
        <div runat="server" id="Div1" class="row">
            <div class="col-lg-12">
                <table class="table table-striped">
                <thead>
                    <tr>
                    <th>Status</th>
                    <th>Descrição</th>
                    <th>&nbsp;Previsto</th>
                    <th>Realizado</th>
                    <th>&nbsp;Tarefas</th>
                    <th>&nbsp;</th>
                    <th>#</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder runat="server" ID="PlaceHolder1" >
                    </asp:PlaceHolder>
                </tbody>
                </table>
                <asp:PlaceHolder runat="server" ID="PlaceHolder2" >
                <form runat="server" id="Form1" class="form-signin">
                <table class="table table-striped">
                <thead>
                    <tr>
                    <th>Nova atividade</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                    <td>
                        <label for="NewDescription" class="control-label">Descrição</label>
                        <asp:TextBox runat="server" ID="NewDescription" TextMode="multiline" Rows="5" CssClass="form-control" MaxLenght="500" placeholder="Descrição" required="required" autofocus="autofocus" />
                        <input type="hidden" name="pacote" value="<%=Pacote%>" />
                        <br />
                        <asp:Button runat="server" ID="ButtonConfirm1" Text="Criar atividade" OnClick="ButtonConfirm1_Click" CssClass="btn-link" />
                    </td>
                    </tr>
                </tbody>
                </table>
                </form>
                </asp:PlaceHolder>
            </div>
        </div>
        <div runat="server" id="Div2">
            <form id="Form2" class="navbar-form navbar-left" action="equipe.aspx" method="post">
                <input name="pacote" type="hidden" value="<%=Pacote%>" />
                <button id="ButtonConfirm2" type="submit" class="btn btn-lg btn-success">Equipe do pacote <%=Pacote%></button>
            </form>
        </div>
        <div runat="server" id="Div3">
            <form id="Form3" class="navbar-form navbar-left" action="pacote_estatistica.aspx" method="post">
                <input name="pacote" type="hidden" value="<%=Pacote%>" />
                <button id="ButtonConfirm3" type="submit" class="btn btn-lg btn-success">Estatísticas do pacote <%=Pacote%></button>
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