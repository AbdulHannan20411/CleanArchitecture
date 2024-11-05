pipeline {
    agent any

    environment {
        BUILD_DIR = "D:\\My Projects\\Build\\ToDo_Build"  // Directory for the compiled build files
        DEPLOY_DIR = "D:\\Path\\To\\Live\\Server\\ToDo"    // Directory on the server where the app should be deployed
    }

    stages {
        stage('Checkout') {
            steps {
                // Clone the repository
                git branch: 'master', url: 'https://github.com/AbdulHannan20411/CleanArchitecture.git'
            }
        }

        stage('Restore Dependencies') {
            steps {
                dir('ToDo') {  // Change to the directory where the solution is located
                    // Restore dependencies using .NET CLI
                    bat 'dotnet restore ToDoApplication.sln'
                }
            }
        }

        stage('Build') {
            steps {
                dir('ToDo') {
                    // Build the project in Release mode
                    bat 'dotnet build --configuration Release ToDoApplication.sln'
                }
            }
        }

        stage('Test') {
            steps {
                dir('ToDo') {
                    // Run tests
                    bat 'dotnet test ToDoApplication.sln'
                }
            }
        }

        stage('Publish') {
            steps {
                dir('ToDo') {
                    // Publish the build to a folder for deployment
                    bat "dotnet publish -c Release -o ${env.BUILD_DIR}"
                }
            }
        }

        stage('Deploy') {
            steps {
                // Copy files to the deployment directory
                echo "Deploying files to ${env.DEPLOY_DIR}..."
                bat "xcopy /s /y \"${env.BUILD_DIR}\\*\" \"${env.DEPLOY_DIR}\""

                // Optionally, restart IIS or a service if required
                // Example for restarting IIS (if hosting on IIS):
                bat 'iisreset'

                // Example for restarting a Windows service:
                // bat 'net stop "YourServiceName" && net start "YourServiceName"'
            }
        }
    }

    post {
        always {
            echo 'Cleaning up workspace'
            deleteDir()  // Clean up the workspace after build
        }
        success {
            echo 'Build and deployment successful!'
        }
        failure {
            echo 'Build or deployment failed.'
        }
    }
}
