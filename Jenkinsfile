pipeline {
    agent any

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

        stage('Unit Tests') {
            steps {
                bat 'dotnet test --no-build --logger trx'
            }
        }

        stage('Selenium Tests') {
            steps {
                bat 'dotnet test --filter Category=Selenium'
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