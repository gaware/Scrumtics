<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="votacao_resultado.aspx.cs" Inherits="Scrumtics.votacao_resultado" %>

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
        <form runat="server" id="Form1" class="form-signin">
            <div class="form-signin-heading">
                <h3>Resultado da votação <%=Votacao%></h3>
            </div>
            <div>
                <h3>Média dos votos: <asp:Label runat="server" ID="LabelMedia" /></h3>
            </div>
            <div>
                <h3>Total de votantes: <asp:Label runat="server" ID="LabelTotal" /></h3>
            </div>
            <div>
                <h3>Maioria dos votos: <asp:Label runat="server" ID="LabelMaioria" /></h3>
            </div>
            <div>
                <h3>Quantidade da maioria: <asp:Label runat="server" ID="LabelQuantidade" /></h3>
            </div>            
            <label for="NewProvided" class="control-label">Previsto</label>
            <asp:TextBox runat="server" ID="NewProvided" TextMode="Number" CssClass="form-control" min="0" max="100" step="0.25" placeholder="0" />
            <input type="hidden" name="pacote" value="<%=Pacote%>" />
            <input type="hidden" name="atividade" value="<%=Atividade%>" />
            <input type="hidden" name="tarefa" value="<%=Tarefa%>" />
            <input type="hidden" name="votacao" value="<%=Votacao%>" />
            <br />
            <asp:Button runat="server" ID="ButtonConfirm1" Text="Confirmar votação" OnClick="ButtonConfirm1_Click" CssClass="btn btn-lg btn-success" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button runat="server" ID="ButtonRepeat1" Text="Repetir votação" OnClick="ButtonRepeat1_Click" CssClass="btn btn-lg btn-success" />
        </form>
    </div>

    <!-- jQuery (obrigatório para plugins JavaScript do Bootstrap) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <!-- Inclui todos os plugins compilados (abaixo), ou inclua arquivos separadados se necessário -->
    <script src="bs/dist/js/bootstrap.min.js"></script>

</body>
</html>
