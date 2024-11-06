pipeline {
    agent any
    environment {
        TEMP_BUILD_DIR = "D:/My Projects/Build/ToDo_Build" // Temporary directory for build output
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
            echo "Publishing build output to temporary directory: ${env.TEMP_BUILD_DIR}..."
            
            // Publish output to the temporary directory
            dir('D:/My Projects/ToDoApplication') {
                bat "dotnet publish \"D:/My Projects/ToDoApplication/ToDoApplication.sln\" --configuration Release -o \"${env.TEMP_BUILD_DIR}\""
            }

            // Add a check to list files in the temporary build directory to confirm publishing
            echo "Listing files in the build directory:"
            bat "dir \"${env.TEMP_BUILD_DIR}\""
        }
    }
}
stage('Deploy') {
    steps {
        script {
            echo "Stopping IIS to release file locks..."
            bat "Get-Process w3wp | Stop-Process"  // Stop IIS Worker Process to release locks

            echo "Deploying build output to IIS path: ${env.IIS_PATH}..."
            
            // Copy files from build directory to IIS directory
            bat "xcopy /E /I /Y \"${env.TEMP_BUILD_DIR}\\*\" \"${env.IIS_PATH}\\\""
            
            echo "Starting IIS Application Pool..."
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
