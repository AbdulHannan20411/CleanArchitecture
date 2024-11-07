pipeline {
    agent any

    environment {
        TEMP_BUILD_DIR = 'D:/My Projects/Build/ToDo_Build' // Temporary directory for build output
        IIS_SITE_NAME = 'ToDo' // IIS site name
        IIS_APP_POOL = 'ToDo' // IIS app pool name
        IIS_PATH = 'C:/inetpub/wwwroot/ToDo' // IIS site path
        GIT_CREDENTIALS_ID = 'Vinciio'
        GIT_URL = 'https://github.com/AbdulHannan20411/CleanArchitecture.git'
        PROJECT_DIR = 'D:/My Projects/ToDoApplication'
    }

    stages {
        stage('Checkout Code') {
            steps {
                git credentialsId: "${env.GIT_CREDENTIALS_ID}", url: "${env.GIT_URL}"
            }
        }

        stage('Restore Packages') {
            steps {
                dir("${env.PROJECT_DIR}") {
                    bat 'dotnet restore ToDoApplication.sln'
                }
            }
        }

        stage('Build Solution') {
            steps {
                dir("${env.PROJECT_DIR}") {
                    bat 'dotnet build ToDoApplication.sln --configuration Release'
                }
            }
        }

        stage('Publish Build') {
            steps {
                dir("${env.PROJECT_DIR}") {
                    bat "dotnet publish \"${env.PROJECT_DIR}/ToDoApplication.sln\" --configuration Release -o \"${env.TEMP_BUILD_DIR}\""
                }
            }
        }

        stage('Deploy to IIS') {
            steps {
                script {
                    // Stop the IIS app pool
                    bat "appcmd stop apppool /apppool.name:\"${env.IIS_APP_POOL}\""

                    // Copy the published files to the IIS directory
                    bat "xcopy /E /Y /I \"${env.TEMP_BUILD_DIR}\\*\" \"${env.IIS_PATH}\""

                    // Start the IIS app pool
                    bat "appcmd start apppool /apppool.name:\"${env.IIS_APP_POOL}\""
                }
            }
        }
    }

    post {
        success {
            echo 'Deployment succeeded!'
        }
        failure {
            echo 'Deployment failed. Please check the logs for more details.'
        }
    }
}
