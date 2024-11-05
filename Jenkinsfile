pipeline {
    agent any
    environment {
        BUILD_DIR = "D:/My Projects/Build/ToDo_Build" // Directory for published files
        IIS_SITE_NAME = "ToDo" // IIS site name
        IIS_APP_POOL = "ToDo" // IIS app pool name
        IIS_PATH = "C:/inetpub/wwwroot/ToDo" // IIS site path
    }
    stages {
        stage('Checkout') {
            steps {
                git credentialsId: 'Vinciio', url: 'https://github.com/AbdulHannan20411/CleanArchitecture.git'
            }
        }
        stage('Restore Dependencies') {
            steps {
                dir('D:/My Projects/ToDoApplication') {
                    bat 'dotnet restore ToDoApplication.sln'
                }
            }
        }
        stage('Build') {
            steps {
                dir('D:/My Projects/ToDoApplication') {
                    bat 'dotnet build ToDoApplication.sln --configuration Release'
                }
            }
        }
        stage('Publish') {
            steps {
                dir('D:/My Projects/ToDoApplication') {
                    bat 'dotnet publish "ToDoApplication.sln" --configuration Release -o "D:/My Projects/Build/ToDo_Build"'
                }
            }
        }
        stage('Deploy') {
            steps {
                script {
                    echo "Stopping IIS Application Pool..."
                    // Stop the IIS Application Pool
                    bat "appcmd stop apppool /apppool.name:${env.IIS_APP_POOL}"

                    echo "Deploying to IIS..."
                    
                    // Remove old files
                    bat "if exist ${env.IIS_PATH}\\* del /q ${env.IIS_PATH}\\*"
                    
                    // Copy new files to the IIS directory
                    bat "xcopy /E /I /Y ${env.BUILD_DIR}\\* ${env.IIS_PATH}\\"

                    echo "Starting IIS Application Pool..."
                    // Start the IIS Application Pool again
                    bat "appcmd start apppool /apppool.name:${env.IIS_APP_POOL}"
                }
            }
        }
    }
    post {
        failure {
            echo 'Build or deployment failed.'
            cleanWs()
        }
    }
}
