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
        // Remove old files
        echo "Removing old files from ${env.IIS_PATH}..."
        bat "if exist \"${env.IIS_PATH}\\*\" del /q \"${env.IIS_PATH}\\*\""

        script {
            // Ensure the BUILD_DIR path is properly formatted for Windows
            def formattedBuildDir = "${env.BUILD_DIR.replaceAll('\\\\', '/').replaceAll(' ', '\\ ')}"

            // Publish the build output
            dir('D:/My Projects/ToDoApplication') {
                bat """
                    dotnet publish "ToDoApplication.sln" --configuration Release -o "${formattedBuildDir}"
                """
            }
        }
    }
}


       stage('Deploy') {
    steps {
        script {
            def appcmdPath = "C:\\Windows\\System32\\inetsrv\\appcmd"
            
            echo "Checking if IIS Application Pool exists..."
            def appPoolExists = bat(script: "${appcmdPath} list apppool /name:\"${env.IIS_APP_POOL}\"", returnStatus: true) == 0
            
            if (appPoolExists) {
                echo "Stopping IIS Application Pool..."
                bat "${appcmdPath} stop apppool /appPool.name:\"${env.IIS_APP_POOL}\""
            } else {
                echo "Application Pool ${env.IIS_APP_POOL} does not exist. Skipping stop command."
            }

            echo "Deploying to IIS..."

            // Remove old files
           // echo "Removing old files from ${env.IIS_PATH}..."
           // bat "if exist \"${env.IIS_PATH}\\*\" del /q \"${env.IIS_PATH}\\*\""
            
            // Copy new files to the IIS directory
           // echo "Copying new files to ${env.IIS_PATH}..."
           // bat "xcopy /E /I /Y \"${env.BUILD_DIR}\\*\" \"${env.IIS_PATH}\\\""

            echo "Starting IIS Application Pool..."
            if (appPoolExists) {
                bat "${appcmdPath} start apppool /appPool.name:\"${env.IIS_APP_POOL}\""
            } else {
                echo "Skipping start command since the Application Pool does not exist."
            }
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
