pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
    }

    stages {

        stage('Checkout') {
            steps {
                git branch: 'master', url: 'https://github.com/alexcruz91/utb_selenium.git'
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('SonarQube Analysis + Coverage') {
            steps {
                withSonarQubeEnv('SonarQubeServer') {
                    withCredentials([string(credentialsId: 'sonar-token', variable: 'SONAR_TOKEN')]) {
                        bat '''
                        echo ================================
                        echo INICIANDO ANALISIS SONAR
                        echo ================================

                        "C:\\Users\\AlexanderCruz\\.dotnet\\tools\\dotnet-sonarscanner" begin ^
                        /k:"utb-selenium" ^
                        /d:sonar.host.url=%SONAR_HOST_URL% ^
                        /d:sonar.token=%SONAR_TOKEN% ^
                        /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml" ^
                        /d:sonar.exclusions="**/CoverageReport/**,**/*.html,**/*Selenium*/**"

                        dotnet build --no-restore

                        dotnet test ".\\TestPruebasSonarqube\\TestPruebasSonarqube.csproj" ^
                        /p:CollectCoverage=true ^
                        /p:CoverletOutputFormat=opencover ^
                        /p:CoverletOutput=".\\coverage\\coverage.opencover.xml"

                        echo ================================
                        echo VALIDANDO COVERAGE
                        echo ================================
                        dir TestPruebasSonarqube\\coverage

                        "C:\\Users\\AlexanderCruz\\.dotnet\\tools\\dotnet-sonarscanner" end ^
                        /d:sonar.token=%SONAR_TOKEN%
                        '''
                    }
                }
            }
        }

        stage('Selenium Tests') {
            steps {
                bat '''
                echo ================================
                echo INICIANDO APP PARA SELENIUM
                echo ================================
 
                start /B dotnet run --project PruebasMetricasProject --urls=http://localhost:5000 --no-build
 
                echo Esperando que la app responda en http://localhost:5000...
                :WAIT
                powershell -Command "try { Invoke-WebRequest http://localhost:5000 -UseBasicParsing | Out-Null; exit 0 } catch { exit 1 }"
                if errorlevel 1 (
                    ping 127.0.0.1 -n 3 > nul
                    goto WAIT
                )
                echo App lista!
 
                echo ================================
                echo EJECUTANDO TESTS SELENIUM
                echo ================================
 
                dotnet test ".\\TestProjectUnit\\TestProjectUnit.csproj" --filter Category=Selenium
 
                echo ================================
                echo DETENIENDO APP
                echo ================================
 
                taskkill /IM PruebasMetricasProject.exe /F || exit 0
                '''
            }
        }
    }

    post {
        always {
            echo 'Pipeline finalizado'
        }
        success {
            echo 'Build exitoso'
        }
        failure {
            echo 'Build fallido'
        }
    }
}