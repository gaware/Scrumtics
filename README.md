# Scrumtics
Organize o seu planning poker e obtenha as estatísticas da equipe.

[É compatível com o home office...]

Projeto tecnológico em sistemas para internet pela [Ulbra](https://www.ulbra.br/ead/graduacao/ead/sistemas-para-internet/superior-de-tecnologia).

Entre em contato com o [autor](mailto:gawarez@gmail.com).

# A proposta

Este projeto surgiu da experiência do autor atuando como Scrum Master em uma equipe de desenvolvimento ágil. 

Inicialmente, um pacote de atividades é planejado para preencher o tempo de disponibilidade dos técnicos que formam a equipe dentro de um período determinado. Posteriormente é agendado um dia onde a equipe inteira avalia as atividades planejadas. No decorrer desta avaliação é realizada uma votação de estimativa de tempo de realização para cada uma das tarefas que envolvem as atividades. Para esta votação são utilizadas cartas com números que representavam os possíveis tempos a serem considerados para se votar. 

A ideia do projeto é disponibilizar uma ferramenta onde o Scrum Master poderá cadastrar o pacote, as atividades e suas tarefas permitindo abrir as mesmas para votação de tempo. Os técnicos poderão então preencher seus votos e a ferramenta indicará qual foi a maioria de votos. Conforme decisão do Scrum Master a votação será considerada válida ou nula. Caso nula será requisitado repetir a mesma, senão seguirá para uma próxima tarefa a ser votada. Ainda a partir do pacote a qualquer momento poderá ser acessado um painel de estatísticas dos votos da equipe onde se pode medir a assertividade de cada um dentro do que a maioria está indicando, o que também será um indicativo do grau de conhecimento do técnico. Posteriormente poderão ser preenchidos os tempos reais de realização das tarefas para geração de um comparativo de previsto versus realizado. A partir destas informações se espera que o Scrum Master possa conhecer melhor sua equipe.

Para execução deste projeto será utilizado um servidor IIS rodando em um Windows Server. As páginas são escritas com ASP.NET, HTML, JavaScript e CSS. A persistência de dados é realizada em banco de dados SQL Server.
