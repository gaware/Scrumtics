<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="barra_inicio.ascx.cs" Inherits="Scrumtics.barra_inicio" %>

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
            <li class="active"><a href="sobre.aspx">Sobre</a></li>
            <li><a href="usuario.aspx">Novo usuário</a></li>
            </ul>
            <form class="navbar-form navbar-left" action="<%=Acao%>" method="post">
            <div class="form-group">
                <input name="usuario" type="text" placeholder="Usuário" class="form-control" required="required" />
            </div>
            <div class="form-group">
                <input name="senha" type="password" placeholder="Senha" class="form-control" required="required" />
            </div>
            <input name="votacao" type="hidden" value="<%=Votacao%>" />
            <button type="submit" class="btn btn-success">Entrar</button>
            </form>
        </div>
    </div>
</nav>