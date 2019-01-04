<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="barra_usuario.ascx.cs" Inherits="Scrumtics.barra_usuario" %>

<nav class="navbar navbar-inverse navbar-fixed-top">
    <div class="container">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
            <span class="sr-only">Toggle navigation</span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            </button>
            <a class="navbar-brand" href="inicio.aspx">Scrumtics</a>
        </div>
        <div id="navbar" class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
            <li class="active dropdown">
                <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Usuário <%=Usuario%><%=SM%> <span class="caret"></span></a>
                <ul class="dropdown-menu">
                <li>&nbsp;<img src="imagem.ashx?usuario=<%=Imagem%>" class="img-circle navbar-left" style="height:40px;margin-top:5px;margin-bottom:5px;margin-left:15px"/></li>
                <li><a href="usuario.aspx">Minha conta</a></li>
                <li><a href="inicio.aspx">Sair</a></li>
                </ul>
            </li>
            <li><a href="pacote.aspx">Novo pacote</a></li>
            </ul>
            <form class="navbar-form navbar-left" action="atividade.aspx" method="post">
            <div class="form-group">
                <input name="pacote" type="text" placeholder="Pacote" class="form-control" required="required" value="<%=Pacote%>" />
            </div>
            <button type="submit" class="btn btn-success">Abrir</button>
            </form>
            <form class="navbar-form navbar-left" action="votacao.aspx" method="post">
            <div class="form-group">
                <input name="votacao" type="text" placeholder="Votação" class="form-control" required="required" value="<%=Votacao%>" />
            </div>
            <button type="submit" class="btn btn-success">Acessar</button>
            </form>
        </div>
    </div>
</nav>