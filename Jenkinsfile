pipeline {
    agent any
     
	environment {
        TEST_URL = 'http://localhost:5000/Identity/Account/Register'
    }

    stages {

        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build --no-restore'
            }
        }

        stage('Run App') {
			steps {
				bat '''
				start /B dotnet run --project PruebasMetricasProject --urls=http://localhost:5000
				ping 127.0.0.1 -n 15 > nul
				'''
			}
		}

        stage('Unit Tests') {
            steps {
				catchError(buildResult: 'UNSTABLE', stageResult: 'FAILURE') {
					bat 'dotnet test --filter Category!=Selenium'
				}
            }
        }

        stage('Selenium Tests') {
            steps {
				catchError(buildResult: 'UNSTABLE', stageResult: 'FAILURE') {
					bat 'dotnet test --filter Category=Selenium'
				}
            }
        }

        stage('SonarQube Analysis') {
            steps {
                withSonarQubeEnv('SonarQubeServer') {
                    bat '''
                    dotnet sonarscanner begin /k:"utb-selenium"
                    dotnet build
                    dotnet sonarscanner end
                    '''
                }
            }
        }
    }
}