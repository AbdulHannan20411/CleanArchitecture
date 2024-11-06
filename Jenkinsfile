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
            echo "Publishing build output directly to the IIS path: ${env.IIS_PATH}..."
            // Publish output directly to the IIS directory
            dir('D:/My Projects/ToDoApplication') {
                bat "dotnet publish D:/My Projects/ToDoApplication/ToDoApplication.sln --configuration Release -o \"${env.IIS_PATH}\""
            }
            
            // Add a check to list files in the IIS directory
            echo "Listing files in the IIS path:"
            bat "dir \"${env.IIS_PATH}\""
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
