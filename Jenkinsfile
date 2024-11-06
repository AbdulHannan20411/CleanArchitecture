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
                    // Stop the IIS application pool and handle cases where it might not be running
                    bat """
                    if exist "C:\\Windows\\System32\\inetsrv\\appcmd.exe" (
                        C:\\Windows\\System32\\inetsrv\\appcmd stop apppool /appPool.name:"${env.IIS_APP_POOL}" || echo "App pool ${env.IIS_APP_POOL} might already be stopped."
                    ) else (
                        echo "AppCmd not found, ensure IIS is installed and accessible."
                    )
                    """
                    
                    // Publish to a temporary build directory with quotes for paths
                    echo "Publishing build output to ${env.TEMP_BUILD_DIR}..."
                    dir('D:/My Projects/ToDoApplication') {
                        bat "dotnet publish ToDoApplication.sln --configuration Release -o \"${env.TEMP_BUILD_DIR}\""
                    }
                }
            }
        }
        stage('Deploy') {
            steps {
                script {
                    // Clear the old files in IIS path
                    echo "Removing old files from ${env.IIS_PATH}..."
                    bat "if exist \"${env.IIS_PATH}\" rmdir /S /Q \"${env.IIS_PATH}\""
                    
                    // Recreate the IIS directory to ensure it's present for deployment
                    bat "mkdir \"${env.IIS_PATH}\""
                    
                    // Copy new files to IIS path
                    echo "Deploying new files to ${env.IIS_PATH}..."
                    bat "xcopy /E /H /I /Y \"${env.TEMP_BUILD_DIR}\\*\" \"${env.IIS_PATH}\""
                    
                    echo "Starting IIS Application Pool..."
                    bat """
                    if exist "C:\\Windows\\System32\\inetsrv\\appcmd.exe" (
                        C:\\Windows\\System32\\inetsrv\\appcmd start apppool /appPool.name:"${env.IIS_APP_POOL}"
                    ) else (
                        echo "AppCmd not found, ensure IIS is installed and accessible."
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
