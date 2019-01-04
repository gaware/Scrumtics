<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tarefa_edicao.aspx.cs" Inherits="Scrumtics.tarefa_edicao" %>
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
                <h3>Cadastro da tarefa <%=Tarefa%></h3>
            </div>
            <label for="NewStatus" class="control-label">Status</label>
            <asp:DropDownList runat="server" ID="NewStatus" CssClass="form-control"> 
            </asp:DropDownList>
            <label for="NewDescription" class="control-label">Descrição</label>
            <asp:TextBox runat="server" ID="NewDescription" TextMode="multiline" Rows="6" CssClass="form-control" MaxLength="2000" placeholder="Descrição" required="required" />
            <label for="NewRealized" class="control-label">Realizado</label>
            <asp:TextBox runat="server" ID="NewRealized" TextMode="Number" CssClass="form-control" min="0" max="100" step="0.25" placeholder="0" />
            <input type="hidden" name="pacote" value="<%=Pacote%>" />
            <input type="hidden" name="atividade" value="<%=Atividade%>" />
            <input type="hidden" name="id" value="<%=Tarefa%>" />
            <br />
            <asp:Button runat="server" ID="ButtonConfirm1" Text="Modificar tarefa" OnClick="ButtonConfirm1_Click" CssClass="btn btn-lg btn-success" />
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