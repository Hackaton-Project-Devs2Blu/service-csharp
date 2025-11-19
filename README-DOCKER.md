# Arquitetura de Containerização (.NET / C#)

Este documento detalha a estratégia de build e segurança adotada para o serviço C#.

## Decisões Técnicas

### 1. Multi-Stage Build com Alpine
Utilizamos uma abordagem de dois estágios para otimizar o tamanho da imagem final:
* **Builder:** Utilizamos a imagem `dotnet/sdk:8.0-alpine` para ter acesso ao compilador e ferramentas de build.
* **Runtime:** Utilizamos a imagem `dotnet/aspnet:8.0-alpine` para a execução. Esta versão contém apenas o runtime necessário, sem o SDK pesado.
* **Resultado:** Imagens finais compactas (~100MB) e mais seguras.

### 2. Otimização de Cache (Restore)
Copiamos os arquivos `.csproj` separadamente antes de copiar o restante do código-fonte.
* **Motivo:** O Docker utiliza cache de camadas. Se não houver adição de novas bibliotecas NuGet, o comando `dotnet restore` (que consome tempo de rede) é ignorado nas builds subsequentes, acelerando o processo de CI/CD.

### 3. Segurança (Usuário Não-Root)
A aplicação não é executada com o usuário `root` (padrão do Docker), mas sim com um usuário restrito chamado `appuser`.
* **Configuração:** Definimos a variável `ASPNETCORE_URLS=http://+:8080` pois portas abaixo de 1024 exigem privilégios elevados. A aplicação escutará na porta 8080 para compatibilidade total com o usuário de baixos privilégios.

### 4. Sistema Operacional (Alpine vs Chiseled)
Mantivemos o uso do **Alpine Linux** como base.
* **Motivo:** Embora existam imagens menores (Chiseled/Distroless), o Alpine mantém o acesso a um shell (`sh`). Isso é essencial para permitir diagnósticos de rede e verificação de arquivos dentro do container durante o hackathon.

## Ajuste Necessário
⚠️ **Atenção Equipe:** No arquivo `Dockerfile`, verifiquem a última linha `ENTRYPOINT`. O nome `NomeDoProjeto.dll` deve ser alterado para corresponder ao nome real do assembly gerado pelo projeto (verifiquem o arquivo .csproj).