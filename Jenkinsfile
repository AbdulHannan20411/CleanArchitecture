pipeline {
    agent any
    environment {
        TEMP_BUILD_DIR = "D:/My Projects/Build/ToDo_Build_Temp" // Temporary directory for build output
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
                script {
                    echo "Stopping IIS Application Pool before publishing..."
                    // Stop the IIS application pool if it exists, to prevent deployment conflicts
                    bat """
                    if exist "C:\\Windows\\System32\\inetsrv\\appcmd.exe" (
                        C:\\Windows\\System32\\inetsrv\\appcmd stop apppool /appPool.name:"${env.IIS_APP_POOL}" || echo "App pool ${env.IIS_APP_POOL} might already be stopped."
                    ) else (
                        echo "AppCmd not found. Ensure IIS is installed and accessible."
                    )
                    """
                    
                    echo "Publishing build output directly to the IIS path: ${env.IIS_PATH}..."
                    // Publish output directly to the IIS directory
                    dir('D:/My Projects/ToDoApplication') {
                        bat "dotnet publish ToDoApplication.sln --configuration Release -o \"${env.IIS_PATH}\""
                    }
                }
            }
        }
        stage('Deploy') {
            steps {
                script {
                    echo "Starting IIS Application Pool..."
                    // Start the IIS application pool to serve the new deployment
                    bat """
                    if exist "C:\\Windows\\System32\\inetsrv\\appcmd.exe" (
                        C:\\Windows\\System32\\inetsrv\\appcmd start apppool /appPool.name:"${env.IIS_APP_POOL}"
                    ) else (
                        echo "AppCmd not found. Ensure IIS is installed and accessible."
                    )
                    """
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
