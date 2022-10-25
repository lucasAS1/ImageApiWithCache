# Script para analisar qualidade de código no SonarQube localmente
#
# Certifique-se antes de roda-lo que os seguintes requisitos estejam cumpridos:
# -> Git bash esteja instalado;
# -> Docker Desktop for windows esteja instalado;
# -> coverlet.collector adicionado nos projetos de testes
# -> Container com servidor do SonarQube que esteja ativo e pronto para receber requests
#    na porta 9000. Para tal fim, utilize o comando abaixo.
#    $ docker run -d -p 9000:9000 sonarqube


# Instala command line tool para analisar codigo no sonarqube
dotnet tool install -g dotnet-sonarscanner
# Instala command line tool para gerar relatorio de cobertura de codigo
dotnet tool install -g dotnet-reportgenerator-globaltool

# Workaround para evitar que seja exigido a mudança da senha do administrador
curl -u admin:admin -X POST "http://192.168.1.10:9000/api/users/change_password?login=admin&previousPassword=admin&password=admin1"
curl -u admin:admin1 -X POST "http://192.168.1.10:9000/api/users/change_password?login=admin&previousPassword=admin1&password=admin"

# Builda aplicação
dotnet build ImageApiWithCache.sln

# Inicia análise
MSYS2_ARG_CONV_EXCL="*" dotnet sonarscanner begin \
/k:"ImageApiWithCache" \
/d:sonar.login=admin \
/d:sonar.password=admin \
/d:"sonar.host.url=http://192.168.1.10:9000" \
/d:sonar.coverageReportPaths="..\Report\SonarQube.xml"


# Roda suite de testes unitarios
MSYS2_ARG_CONV_EXCL="*" dotnet test \
--collect:"XPlat Code Coverage" \
--settings tests-runsettings

# Monta arquivo XML no formato esperado pelo SonarQube com metricas de cobertura de codigo
reportgenerator -reports:**/coverage.opencover.xml -targetdir:Report -reporttypes:"SonarQube"

# Finaliza análise no SonarQube
MSYS2_ARG_CONV_EXCL="*" dotnet sonarscanner end \
/d:sonar.login=admin \
/d:sonar.password=admin

# Remove diretorio .sonarqube que, por algum motivo, impede que uma segunda analise seja executada
rm -rf .sonarqube

# remove diretóriso criados para gerar o relatório
rm -rf Report
#rm -rf **/TestResults

# Abre site com a análise
start http://192.168.1.10:9000/dashboard?id=ImageApiWithCache

read -p "Pressione enter para sair"
